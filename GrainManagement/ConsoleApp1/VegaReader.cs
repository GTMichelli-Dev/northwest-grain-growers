using System;
using System.Net.Sockets;
using NModbus;

public class VegaReader
{
    private readonly string _ip;
    private readonly int _port;
    private readonly byte _slaveId;

    public VegaReader(string ip, int port = 50000, byte slaveId = 1)
    {
        _ip = ip;
        _port = port;
        _slaveId = slaveId;
    }

    public double ReadPrimaryVariable()
    {
        using var tcpClient = new TcpClient(_ip, _port);
        var factory = new ModbusFactory();
        var master = factory.CreateMaster(tcpClient);

        // Example: PV mapped to holding registers 0–1 as IEEE754 float
        ushort[] regs = master.ReadHoldingRegisters(_slaveId, 0, 2);

        // Convert 2x16-bit to float
        byte[] bytes = {
            (byte)(regs[1] >> 8), (byte)(regs[1] & 0xFF),
            (byte)(regs[0] >> 8), (byte)(regs[0] & 0xFF)
        };
        float pv = BitConverter.ToSingle(bytes, 0);
        return pv;
    }
}
