using System;
using Xunit;

namespace DesignPatterns.Structural
{
    using MAmp = Int32;
    using Amp = Decimal;

    public class Adapter : DesignPattern
    {
        public override void Execute()
        {
            MAmp maxUsbPower = 200;

            var phone = new Phone(maxMAmps: maxUsbPower);
            Assert.False(phone.IsCharging);

            var outlet = new AcOutlet(amps: 15);
            var adapter = new UsbAdapter(socket: outlet, maxMAmps: maxUsbPower);

            phone.ConnectTo(adapter);
            Assert.True(phone.IsCharging);

            var badAdapter = new BadUsbAdapter(socket: outlet);
            Assert.Throws<OverflowException>(() => phone.ConnectTo(badAdapter));
        }

        #region Definition

        /// <summary>
        /// Client
        /// </summary>
        /// <remarks>
        /// Collaborates with objects conforming to the Target interface.
        /// </remarks>
        public class Phone
        {
            private readonly MAmp _maxMAmps;
            private MAmp _mAmps;

            public Phone(MAmp maxMAmps)
            {
                this._maxMAmps = maxMAmps;
            }

            public bool IsCharging => this._mAmps > 0;

            public void ConnectTo(IUsbSocket socket)
            {
                MAmp mAmps = socket.GetPower();
                if (mAmps > this._maxMAmps)
                {
                    throw new OverflowException();
                }
                this._mAmps = mAmps;
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

        #endregion

        #region Concrete Implementations

        /// <inheritdoc />
        public class AcOutlet : IAcSocket
        {
            private readonly Amp _amps;

            public AcOutlet(Amp amps)
            {
                this._amps = amps;
            }

            public Amp GetPower()
            {
                return this._amps;
            }
        }

        /// <summary>
        /// Adapter
        /// </summary>
        /// <remarks>
        /// Adapts the interface Adaptee to the Target interface.
        /// </remarks>
        public class UsbAdapter : IUsbSocket
        {
            private readonly IAcSocket _socket;
            private readonly MAmp _maxMAmps;

            public UsbAdapter(IAcSocket socket, MAmp maxMAmps)
            {
                this._socket = socket;
                this._maxMAmps = maxMAmps;
            }

            public MAmp GetPower()
            {
                Amp amps = this._socket.GetPower();
                var mAmps = (MAmp)(amps * 100);
                return Math.Min(this._maxMAmps, mAmps);
            }
        }

        /// <summary>
        /// Adapter
        /// </summary>
        /// <remarks>
        /// Adapts the interface Adaptee to the Target interface.
        /// </remarks>
        public class BadUsbAdapter : IUsbSocket
        {
            private readonly IAcSocket _socket;

            public BadUsbAdapter(IAcSocket socket)
            {
                this._socket = socket;
            }

            public MAmp GetPower()
            {
                Amp amps = this._socket.GetPower();
                return (MAmp)(amps * 100);
            }
        }

        #endregion
    }
}