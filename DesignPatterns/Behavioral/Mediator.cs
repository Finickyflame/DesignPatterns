using System;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Mediator : DesignPattern
    {
        public override void Execute()
        {
            IParcelService mediator = new ParcelService();

            IPostalUser company = new RejectingParcelUser("12345", mediator);
            var customer = new AcceptingParcelUser("54231", mediator);

            mediator.Register(company);
            mediator.Register(customer);

            var parcel1 = new Parcel();
            company.SendParcel("54231", parcel1);
            // The destination exists, the customer receives the parcel.
            Assert.Contains(parcel1, customer.ParcelsReceived);

            var parcel2 = new Parcel();
            customer.SendParcel("99999", parcel2);
            // The destination doesn't exists, the customer got it back.
            Assert.Contains(parcel2, customer.ParcelsReceived);

            var parcel3 = new Parcel();
            customer.SendParcel("12345", parcel3);
            // The destination is not accepting parcels, the customer got it back again.
            Assert.Contains(parcel3, customer.ParcelsReceived);
        }

        #region Definition

        /// <summary>
        /// Mediator
        /// </summary>
        /// <remarks>
        /// Defines the interface for communication between <see cref="IPostalUser">Colleague</see> objects.
        /// </remarks>
        public interface IParcelService
        {
            void Register(IPostalUser postalUser);

            void Send(IPostalUser fromPostalUser, string toPostalCode, Parcel parcel);
        }

        /// <summary>
        /// Colleague
        /// </summary>
        /// <remarks>
        /// Defines the interface for communication with other <see cref="IPostalUser">Colleagues</see> through its <see cref="IParcelService">Mediator</see>.
        /// </remarks>
        public interface IPostalUser
        {
            string PostalCode { get; }

            void ReceiveParcel(string fromPostalCode, Parcel parcel);

            void SendParcel(string toPostalCode, Parcel parcel);
        }

        public class Parcel
        {
        }

        #endregion

        #region Concrete Implementation

        /// <summary>
        /// Concrete Mediator
        /// </summary>
        /// <remarks>
        /// Implements the Mediator interface and coordinates communication between <see cref="IPostalUser">Colleague</see> objects.
        /// It is aware of all of the <see cref="IPostalUser">Colleagues</see> and their purposes with regards to inter-communication.
        /// </remarks>
        public class ParcelService : IParcelService
        {
            private readonly IDictionary<string, IPostalUser> _usersByPostalCode;

            public ParcelService()
            {
                this._usersByPostalCode = new Dictionary<string, IPostalUser>();
            }

            public void Register(IPostalUser postalUser)
            {
                if (postalUser == null)
                    throw new ArgumentNullException(nameof(postalUser));

                if (!this._usersByPostalCode.ContainsKey(postalUser.PostalCode))
                {
                    this._usersByPostalCode.Add(postalUser.PostalCode, postalUser);
                }
            }

            public void Send(IPostalUser fromPostalUser, string toPostalCode, Parcel parcel)
            {
                if (fromPostalUser == null)
                    throw new ArgumentNullException(nameof(fromPostalUser));

                if (this._usersByPostalCode.TryGetValue(toPostalCode, out IPostalUser toPostalUser))
                {
                    toPostalUser.ReceiveParcel(fromPostalUser.PostalCode, parcel);
                }
                // Make sure the sender is not rejecting parcels, otherwise we may end up with an infinite callback.
                else if (!(fromPostalUser is RejectingParcelUser))
                {
                    // Send it back, we couldn't find the recipient
                    fromPostalUser.ReceiveParcel(toPostalCode, parcel);
                }
            }
        }

        /// <summary>
        /// Concrete Colleague
        /// </summary>
        /// <remarks>
        /// Implements the <see cref="IPostalUser">Colleague</see> interface and
        /// communicates with other <see cref="IPostalUser">Colleagues</see> through its <see cref="IParcelService">Mediator</see>.
        /// </remarks>
        public class RejectingParcelUser : IPostalUser
        {
            private readonly IParcelService _parcelService;

            public RejectingParcelUser(string postalCode, IParcelService parcelService)
            {
                this.PostalCode = postalCode;
                this._parcelService = parcelService;
            }

            public string PostalCode { get; }

            public void ReceiveParcel(string fromPostalCode, Parcel parcel)
            {
                // Rejects all deliveries, send it back.
                this._parcelService.Send(this, fromPostalCode, parcel);
            }

            public void SendParcel(string toPostalCode, Parcel parcel)
            {
                this._parcelService.Send(this, toPostalCode, parcel);
            }
        }

        /// <summary>
        /// Concrete Colleague
        /// </summary>
        /// <remarks>
        /// Implements the <see cref="IPostalUser">Colleague</see> interface and
        /// communicates with other <see cref="IPostalUser">Colleagues</see> through its <see cref="IParcelService">Mediator</see>.
        /// </remarks>
        public class AcceptingParcelUser : IPostalUser
        {

            private readonly IParcelService _parcelService;
            private readonly List<Parcel> _parcelsReceived;

            public AcceptingParcelUser(string postalCode, IParcelService parcelService)
            {
                this.PostalCode = postalCode;
                this._parcelService = parcelService;
                this._parcelsReceived = new List<Parcel>();
            }

            public IReadOnlyList<Parcel> ParcelsReceived => this._parcelsReceived;

            public string PostalCode { get; }

            public void ReceiveParcel(string fromPostalCode, Parcel parcel)
            {
                this._parcelsReceived.Add(parcel);
            }

            public void SendParcel(string toPostalCode, Parcel parcel)
            {
                this._parcelService.Send(this, toPostalCode, parcel);
            }
        }

        #endregion
    }
}
