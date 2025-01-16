--[[
*******************************************************************************

Filename:     awtxReqConstants.lua
Version:      1.0.0.0
Date:         2015-09-01
Customer:     Avery Weigh-Tronix
Description:
This file provides some constant number definitions.

*******************************************************************************

*******************************************************************************
]]


--
-- DISPLAY
-- 
awtx.display.MODE_SCALE             =   0
awtx.display.MODE_USER              =   1
awtx.display.MODE_MENU              =   2


--
-- KEYPAD
-- 
awtx.keypad.RPN_MODE_DISABLED       = 0
awtx.keypad.RPN_MODE_ENABLED        = 1


--
-- TOKEN TYPES
--
awtx.fmtPrint.TYPE_UNDEFINED        = 0
awtx.fmtPrint.TYPE_WEIGHT           = 1
awtx.fmtPrint.TYPE_STRING           = 2
awtx.fmtPrint.TYPE_DATE             = 3
awtx.fmtPrint.TYPE_TIME             = 4
awtx.fmtPrint.TYPE_FLOAT            = 5
awtx.fmtPrint.TYPE_INTEGER          = 6
awtx.fmtPrint.TYPE_BOOLEAN          = 7
awtx.fmtPrint.TYPE_WEIGHT_VAR       = 8
awtx.fmtPrint.TYPE_STRING_VAR       = 9
awtx.fmtPrint.TYPE_DATE_VAR         = 10
awtx.fmtPrint.TYPE_TIME_VAR         = 11
awtx.fmtPrint.TYPE_FLOAT_VAR        = 12
awtx.fmtPrint.TYPE_INTEGER_VAR      = 13
awtx.fmtPrint.TYPE_BOOLEAN_VAR      = 14

-- 
-- PRINTER STATUS
--
awtx.printer.STATUS_OFFLINE           = 1
awtx.printer.STATUS_ONLINE            = 2
awtx.printer.STATUS_OK                = 3
awtx.printer.STATUS_NOT_OK            = 4
awtx.printer.STATUS_PAPER_LOW         = 5
awtx.printer.STATUS_NOT_PAPER_LOW     = 6
awtx.printer.STATUS_SELECTED          = 7
awtx.printer.STATUS_NOT_SELECTED      = 8
awtx.printer.STATUS_GENERAL_ERROR     = 9
awtx.printer.STATUS_NOT_GENERAL_ERROR = 10

awtx.printer.JOB_STATUS_DONE          = 1
awtx.printer.JOB_STATUS_ABORTED       = 2
awtx.printer.JOB_STATUS_CREATE_FAILED = 3

--
-- SCALE VALUES
--
awtx.weight.VAL_GROSS               =  0
awtx.weight.VAL_NET                 =  1
awtx.weight.VAL_TARE                =  2
awtx.weight.VAL_MIN                 =  3
awtx.weight.VAL_MAX                 =  4
awtx.weight.VAL_ROC                 =  5
awtx.weight.VAL_GTOTAL              =  6
awtx.weight.VAL_NTOTAL              =  7
awtx.weight.VAL_TTOTAL              =  8
awtx.weight.VAL_CTOTAL              =  9
awtx.weight.VAL_TRANS               = 10
awtx.weight.VAL_COUNT               = 11
awtx.weight.VAL_VAR                 = 12
awtx.weight.VAL_PCWT                = 13
awtx.weight.VAL_ADCCOUNTS           = 14
awtx.weight.VAL_STD_DEV             = 15
awtx.weight.VAL_TREND               = 16
awtx.weight.VAL_X_BAR_R             = 17
awtx.weight.VAL_PERCENT             = 18
awtx.weight.VAL_NET_MINUS_ANNUN     = 19
awtx.weight.VAL_BLANK               = 20
awtx.weight.VAL_INMOTION            = 21
awtx.weight.VAL_TARGET              = 22
awtx.weight.VAL_GTOT_PLUS_CURRENT   = 23
awtx.weight.VAL_GTOT_MINUS_CURRENT  = 24
awtx.weight.VAL_NTOT_PLUS_CURRENT   = 25
awtx.weight.VAL_NTOT_MINUS_CURRENT  = 26
awtx.weight.VAL_CTOT_PLUS_CURRENT   = 27
awtx.weight.VAL_CTOT_MINUS_CURRENT  = 28
awtx.weight.VAL_ACCUMULATIONS       = 29
awtx.weight.VAL_GAVERAGE            = 30
awtx.weight.VAL_NAVERAGE            = 31
awtx.weight.VAL_CAVERAGE            = 32
awtx.weight.VAL_NMIN                = 33
awtx.weight.VAL_NMAX                = 34
awtx.weight.VAL_RATE                = 35
awtx.weight.VAL_FREEFALL_1          = 36
awtx.weight.VAL_GFUTURE_1           = 37
awtx.weight.VAL_NFUTURE_1           = 38
awtx.weight.VAL_FREEFALL_2          = 39
awtx.weight.VAL_GFUTURE_2           = 40
awtx.weight.VAL_NFUTURE_2           = 41
awtx.weight.VAL_PCWT_1000           = 42
awtx.weight.VAL_PERCENT_ACCURACY    = 43
awtx.weight.VAL_LAST_SAMPLE         = 44
awtx.weight.VAL_GROSS_ALL_SCALES    = 45
awtx.weight.VAL_NET_ALL_SCALES      = 46
awtx.weight.VAL_TARE_ALL_SCALES     = 47
awtx.weight.VAL_COUNT_ALL_SCALES    = 48
awtx.weight.VAL_GTOT_ALL_SCALES     = 49
awtx.weight.VAL_NTOT_ALL_SCALES     = 50
awtx.weight.VAL_CTOT_ALL_SCALES     = 51
awtx.weight.VAL_USER_MESSAGE        = 52