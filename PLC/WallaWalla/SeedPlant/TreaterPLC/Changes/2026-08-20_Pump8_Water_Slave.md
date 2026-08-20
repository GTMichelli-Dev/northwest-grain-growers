# Change Record — Treater_Pump[8] as water slave of Treater_Pump[9]

| | |
|---|---|
| **Controller** | `NWGG_Treater_WW` — 5069-L330ER, firmware v35.11 |
| **Location** | Walla Walla Seed Plant — Treater |
| **Baseline export** | `NWGG_Treater_WW.L5X`, exported Thu Aug 20 14:59:22 2026 |
| **Date drafted** | 2026-08-20 |
| **Status** | **DRAFTED — NOT YET APPLIED.** No PLC change has been made. |

## Goal

Add water volume by running `Treater_Pump[8]` alongside `Treater_Pump[9]` on the
water manifold. Pump 8 is a hydraulic slave: starting, stopping and adjusting
pump 9 affects pump 8 identically.

## Decisions taken

Two questions had to be answered before this could be designed. Both were
decided by the plant side:

1. **Pump 8 hardware** — pump 8 receives the *same pump head/tubing as pump 9*
   and is *recalibrated on water*. Its cal curve will therefore top out near
   pump 9's 328 oz/min rather than its present 144.
2. **Meaning of the water setpoint** — the setpoint remains the **TOTAL** water
   delivered, split across both pumps. It does not become a per-pump number.

With matched heads and matched cals, the capacity split is 50/50.

---

## Background — how the existing code works

Findings from the baseline export, all verified against `NWGG_Treater_WW.L5X`.

### Pump control path

Every pump is driven by one AOI call in `Pumps/MainRoutine` (rungs 15–38):

```
rung 23   Pump_Control(Pump_Control[8], Pump_8:O, Pump_8:I, Treater_Pump[8], Pump_Rate_Multliplier, 1)
rung 24   Pump_Control(Pump_Control[9], Pump_9:O, Pump_9:I, Treater_Pump[9], Pump_Rate_Multliplier, 1)
```

All 24 drives are identical `PowerFlex 525-EENET` rev 5.1.

`Pump_Control` branches on `TreaterScale.Use_Scale`:

- `XIO(Use_Scale)` → `Pump_Volume_Control` — open loop, speed derived from Rate
  through the cal curve
- `XIC(Use_Scale)` → `Pump_Scale_Control` — closed loop on scale/flow meter

**`Treater_Pump[9].TreaterScale.Use_Scale = 0`.** Pump 9 runs **open loop**.
Its `Use_Flow_Meter = 1` flag only matters inside `Pump_Scale_Control`, which
never executes for this pump — so the flow meter currently feeds reporting
only, and there is no closed loop that would fight a second pump.

### Start/stop is derived from Active_Treatment

`Pumps/MainRoutine` rung 39 → `FOR(Auto_Start_Pump, IDX, 1, 19, 1)`:

```
XIC(Treater_Pump[IDX].Active_Treatment) OTL(Treater_Pump[IDX].Start_Pump);
XIO(Treater_Pump[IDX].Active_Treatment) OTU(Treater_Pump[IDX].Start_Pump);
```

Pump 8 is inside the `1..19` range. **Mirroring `Active_Treatment` is therefore
sufficient** — the existing loops handle start, stop, fault rollup
(`Check_Pump_Fault`, 0..23), manual stop (`Stop_Manual`, 0..23) and fault clear
(`Clear_All_Faults`, 0..23) without further changes. `Start_Pump` must **not**
be mirrored directly; that would fight rung 39.

### The silent-zero trap

`Pump_Volume_Control` rung 6:

```
GRT(Treater_Pump.Rate, Cal.Cal_Points.Cal_Point[4].Rate) XIO(Cal.Calibrating) MOV(0, Treater_Pump.Rate);
```

**A Rate above the pump's top cal point is silently set to zero** — no fault, no
alarm, no flow. This is why a naive `Rate[8] := Rate[9]` copy is unsafe, and why
this change adds an explicit clamp.

`Pump_Volume_Control` rung 4 also shows `Speed` is *computed from* `Rate` via
`Pump_Speed_Calc`, so mirroring `Speed` directly is not possible — it is
overwritten every scan.

### Baseline pump data

| | Pump 8 (before) | Pump 9 |
|---|---|---|
| `Rate` | 120.0 | 260.0 |
| `Product.Cal_Factor` | 1.1308095 | 1.0568 |
| `Product.Product_List_IDX` | 42 | 0 |
| `TreaterScale.Lbs_Gal` | 9.2 | 0.0 |
| `TreaterScale.Use_Scale` | 0 | 0 |
| `TreaterScale.Use_Flow_Meter` | 0 | 1 |
| Cal_Point[1] 2250 | 68 | 108 |
| Cal_Point[2] 4500 | 105 | 216 |
| Cal_Point[3] 6750 | 128 | 292 |
| **Cal_Point[4] 9000** | **144** | **328** |

Pump 8's curve was calibrated on its previous chemical (9.2 lb/gal, product
index 42). It is not a water calibration.

### Writers of the water setpoint

`Treater_Pump[9].Rate` is written by `Pumps/MainRoutine` rungs 6, 48, 49, read
at rung 47, and by the HMI. `Pumps/Flush_Manifold` also writes it but that
routine is never called — the project contains only two JSRs
(`MainProgram/MainRoutine` → `PLC_Tags`, `SetScaleValues`).

---

## The change

### 1. New controller-scoped tags

| Tag | Type | Initial | Purpose |
|---|---|---|---|
| `Water_Total_Rate` | REAL | 260.0 | **Total** water rate (oz/min) across pumps 8+9. This is the operator/recipe setpoint. |
| `Water_Slave_Enable` | BOOL | 0 | Enables pump 8 as water slave. Leave 0 until pump 8 is calibrated. |
| `Water_Rate_Share` | REAL | 0.5 | Pump 9's fraction of the total; pump 8 gets `1 - share`. |
| `Water_Split_Seeded` | BOOL | 0 | One-shot seed flag for rung A. |

### 2. New rungs — `Pumps/MainRoutine`, inserted immediately BEFORE existing rung 23

Placement is deliberate: rung 23 is pump 8's AOI call and rung 24 is pump 9's,
so a block placed ahead of rung 23 is consumed by both AOIs in the same scan
with no one-scan lag.

**Rung A** — seed the total from pump 9 on first scan, so behaviour is unchanged
until the HMI is retargeted:

```
XIO(Water_Split_Seeded)OTL(Water_Split_Seeded)MOV(Treater_Pump[9].Rate,Water_Total_Rate);
```

**Rung B** — keep the share sane:

```
[LES(Water_Rate_Share,0.05) ,GRT(Water_Rate_Share,0.95) ]MOV(0.5,Water_Rate_Share);
```

**Rung C** — mirror pump 9's command bits onto pump 8:

```
XIC(Water_Slave_Enable)[XIC(Treater_Pump[9].Active_Treatment) OTE(Treater_Pump[8].Active_Treatment) ,XIC(Treater_Pump[9].Manual_Start_Pump) OTE(Treater_Pump[8].Manual_Start_Pump) ,XIC(Treater_Pump[9].Start_Pump_Reverse) OTE(Treater_Pump[8].Start_Pump_Reverse) ,XIC(Treater_Pump[9].Clear_Faults) OTE(Treater_Pump[8].Clear_Faults) ,XIC(Treater_Pump[9].Purge_Forward) OTE(Treater_Pump[8].Purge_Forward) ,XIC(Treater_Pump[9].Purge_Reverse) OTE(Treater_Pump[8].Purge_Reverse) ,XIC(Treater_Pump[9].Require_Water_Flush) OTE(Treater_Pump[8].Require_Water_Flush) ];
```

**Rung D** — mirror the analog limits:

```
XIC(Water_Slave_Enable)MOV(Treater_Pump[9].Purge_Percent,Treater_Pump[8].Purge_Percent)MOV(Treater_Pump[9].SpeedLimitPercent,Treater_Pump[8].SpeedLimitPercent);
```

**Rung E** — split the total:

```
XIC(Water_Slave_Enable)CPT(Treater_Pump[9].Rate,Water_Total_Rate*Water_Rate_Share)CPT(Treater_Pump[8].Rate,Water_Total_Rate*(1-Water_Rate_Share));
```

**Rung F** — slave disabled: pump 9 carries the whole total, pump 8 parked:

```
XIO(Water_Slave_Enable)MOV(Water_Total_Rate,Treater_Pump[9].Rate)MOV(0,Treater_Pump[8].Rate)OTU(Treater_Pump[8].Active_Treatment)OTU(Treater_Pump[8].Manual_Start_Pump);
```

**Rungs G, H** — clamp each pump to its own top cal point, so
`Pump_Volume_Control` rung 6 can never silently zero the rate:

```
GRT(Treater_Pump[8].Rate,Treater_Pump[8].Cal.Cal_Points.Cal_Point[4].Rate)MOV(Treater_Pump[8].Cal.Cal_Points.Cal_Point[4].Rate,Treater_Pump[8].Rate);
```

```
GRT(Treater_Pump[9].Rate,Treater_Pump[9].Cal.Cal_Points.Cal_Point[4].Rate)MOV(Treater_Pump[9].Cal.Cal_Points.Cal_Point[4].Rate,Treater_Pump[9].Rate);
```

### 3. Edits to existing rungs — `Pumps/MainRoutine`

Each retargets the setpoint from `Treater_Pump[9].Rate` to `Water_Total_Rate`.

**Rung 6** — final MOV destination changed:

```
XIC(Treater_Remote_In.4)OTL(Treater_Pump[9].Active_Treatment)OTL(Treater_Pump[10].Active_Treatment)MOV(Default_Oil,Treater_Pump[10].Rate)MOV(Default_Water,Water_Total_Rate);
```

**Rung 47** — save the total, not pump 9's share:

```
XIO(Start_Water_Flush)XIO(Start_Water_Flush_Running)MOV(Water_Total_Rate,Current_Rate);
```

**Rung 48** — `MOV` becomes `CPT`. Dividing by the share means each pump lands
on its own maximum, preserving the original per-pump flush velocity rather than
halving it:

```
XIC(Start_Water_Flush)OTL(Start_Water_Flush_Running)OTL(Treater_Pump[9].Manual_Start_Pump)CPT(Water_Total_Rate,Treater_Pump[9].Cal.Cal_Points.Cal_Point[4].Rate/Water_Rate_Share);
```

**Rung 49** — restore the total:

```
XIC(Start_Water_Flush_Running)XIO(Start_Water_Flush)MOV(Current_Rate,Water_Total_Rate)OTU(Start_Water_Flush_Running)OTU(Check_For_Flush)OTU(Treater_Pump[9].Manual_Start_Pump);
```

### 4. Required HMI change

Rung E writes `Treater_Pump[9].Rate` every scan. **Any HMI object that writes
`Treater_Pump[9].Rate` must be retargeted to `Water_Total_Rate`** — both the
setpoint entry and the display. Without this the operator's water setpoint will
appear to do nothing.

The Walla Walla HMI project is not in this repository (only Dayton and
Prescott), so this change could not be verified or drafted here.

---

## Commissioning procedure

Order matters. Out of order, pump 8 runs water on a chemical calibration.

1. Re-plumb pump 8 onto the water manifold with the head/tubing matched to pump 9.
2. Offline in Studio 5000: create the four tags, insert rungs A–H before rung 23,
   apply the edits to rungs 6, 47, 48, 49. Leave `Water_Slave_Enable = 0`.
3. Correct pump 8's configuration:
   - `Product.Cal_Factor` → `1.0` (currently **1.1308095**, a fine-tune trim for
     the old chemical — left alone it skews water by ~13%)
   - `Product.Product_List_IDX` → match pump 9 (currently 42)
   - `TreaterScale.Lbs_Gal` → appropriate for water (currently 9.2)
   - confirm `TreaterScale.Use_Scale = 0` and `Use_Flow_Meter = 0`
4. Download.
5. **Calibrate pump 8 on water.** Confirm `Cal_Point[4].Rate` lands near pump 9's
   328 oz/min. Every rate calculation downstream depends on this number.
6. Confirm `Water_Total_Rate` seeded to 260.0 via rung A.
7. Retarget the HMI setpoint and display to `Water_Total_Rate`.
8. Set `Water_Slave_Enable = 1`.

## Verification

- Both pumps start and stop together on auto start/stop.
- Each pump's `Rate` reads half of `Water_Total_Rate`.
- Flow meter (`CurrentWaterOZPerMin`) reads approximately the full
  `Water_Total_Rate`.
- Water flush drives both pumps to their cal maximum.
- Faulting pump 8 at the drive raises `Pumps_In_Fault`.
- Setting `Water_Slave_Enable = 0` returns the system to single-pump operation
  with pump 9 carrying the full total.

## Rollback

Set `Water_Slave_Enable = 0`. Rung F restores pump 9 to the full total and parks
pump 8 — no download required. To remove entirely, delete rungs A–H and revert
rungs 6, 47, 48, 49 to the baseline export.

## Known side effects

- **The flow meter measures both pumps.** `CurrentWaterOZPerMin` becomes the
  combined total, while `Treater_Pump[9].Current_Rate` (derived by
  `Pump_Rate_Calc` from pump 9's own speed) reflects only pump 9's half. Any HMI
  screen comparing the two will appear 2× apart.
- **`Active_Treater_Pump_Count` reads one higher** once pump 8 goes active. That
  pulses `Active_Pump_Count_Changed`, which re-evaluates the `Auto_Start` /
  `Auto_Stop` latches at rungs 4 and 5. Expected to settle immediately — watch
  the first auto cycle.
- **Pump 8's scale rungs become meaningless.** `MainProgram/SetScaleValues`
  rung 7 maps `Scale_7` into `Treater_Pump[8].TreaterScale.RawWeight`. Inert
  while `Use_Scale = 0`, but misleading. Removal optional.
- Rung C mirrors on the scan before the flush rungs (47–49) execute, so pump 8
  follows pump 9 into and out of a flush one scan late. Immaterial for a pump.

## Pre-existing issues found, NOT addressed here

Noted during analysis. Out of scope for this change.

- `MainProgram/Zero_Scales` is never called and its pump↔scale map disagrees
  wholesale with `SetScaleValues` (pump 1←Scale_20 vs ←Scale_6, pump 8←Scale_5
  vs ←Scale_7, and so on). Pump 20 appears twice, once referencing two scales.
  Harmless only because the routine never scans — a landmine if anyone wires it up.
- `Pumps/Flush_Manifold`, `ColorBlending/GetNextColor`, `MainProgram/UpdateScales`
  and `Calibration/Calibrate_Pump` are never called or are empty.
- Programs `Color_Pumps`, `Scale`, `Test` and `Calibration` are unscheduled.
  `Color_Pumps` is the superseded predecessor of `ColorBlending` — see the
  analysis in that change's discussion; it should not be scheduled.

## Provenance

Every claim above was read out of `NWGG_Treater_WW.L5X` (export of
2026-08-20 14:59:22). Rung text was written to match the export's formatting but
has **not** been through the Studio 5000 parser — verify syntax on paste.
