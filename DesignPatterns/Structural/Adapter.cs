namespace DesignPatterns.Structural;

using MAmp = int;
using Amp = decimal;

public class Adapter
{
    [Fact]
    public void Execute()
    {
        const int maxUsbPower = 200;

        var phone = new Phone(maxUsbPower);
        Assert.False(phone.IsCharging);

        var outlet = new AcOutlet(15);
        var adapter = new UsbAdapter(outlet, maxUsbPower);

        phone.ConnectTo(adapter);
        Assert.True(phone.IsCharging);

        var badAdapter = new BadUsbAdapter(outlet);
        Assert.Throws<OverflowException>(() => phone.ConnectTo(badAdapter));
    }

    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Collaborates with objects conforming to the Target interface.
    /// </remarks>
    public class Phone(MAmp maxMAmps)
    {
        private MAmp _mAmps;

        public bool IsCharging => _mAmps > 0;

        public void ConnectTo(IUsbSocket socket)
        {
            MAmp mAmps = socket.GetPower();
            if (mAmps > maxMAmps)
            {
                throw new OverflowException();
            }
            _mAmps = mAmps;
        }
    }

    /// <summary>
    /// Target
    /// </summary>
    /// <remarks>
    /// Defines the domain-specific interface that Client uses.
    /// </remarks>
    public interface IUsbSocket
    {
        MAmp GetPower();
    }

    /// <summary>
    /// Adaptee
    /// </summary>
    /// <remarks>
    /// Defines an existing interface that needs adapting.
    /// </remarks>
    public interface IAcSocket
    {
        Amp GetPower();
    }

    /// <inheritdoc />
    public class AcOutlet(Amp amps) : IAcSocket
    {
        public Amp GetPower() => amps;
    }

    /// <summary>
    /// Adapter
    /// </summary>
    /// <remarks>
    /// Adapts the interface Adaptee to the Target interface.
    /// </remarks>
    public class UsbAdapter(IAcSocket socket, MAmp maxMAmps) : IUsbSocket
    {
        public MAmp GetPower()
        {
            Amp amps = socket.GetPower();
            var mAmps = (MAmp)(amps * 100);
            return Math.Min(maxMAmps, mAmps);
        }
    }

    /// <summary>
    /// Adapter
    /// </summary>
    /// <remarks>
    /// Adapts the interface Adaptee to the Target interface.
    /// </remarks>
    public class BadUsbAdapter(IAcSocket socket) : IUsbSocket
    {
        public MAmp GetPower()
        {
            Amp amps = socket.GetPower();
            return (MAmp)(amps * 100);
        }
    }
}