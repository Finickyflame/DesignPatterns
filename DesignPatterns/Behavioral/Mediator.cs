namespace DesignPatterns.Behavioral;

public class Mediator
{
    [Fact]
    public void Execute()
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
    /// Defines the interface for communication with other <see cref="IPostalUser">Colleagues</see> through its
    /// <see cref="IParcelService">Mediator</see>.
    /// </remarks>
    public interface IPostalUser
    {
        string PostalCode { get; }

        void ReceiveParcel(string fromPostalCode, Parcel parcel);

        void SendParcel(string toPostalCode, Parcel parcel);
    }

    public class Parcel;

    /// <summary>
    /// Concrete Mediator
    /// </summary>
    /// <remarks>
    /// Implements the Mediator interface and coordinates communication between <see cref="IPostalUser">Colleague</see>
    /// objects.
    /// It is aware of all of the <see cref="IPostalUser">Colleagues</see> and their purposes with regards to
    /// inter-communication.
    /// </remarks>
    public class ParcelService : IParcelService
    {
        private readonly Dictionary<string, IPostalUser> _usersByPostalCode = [];

        public void Register(IPostalUser postalUser)
        {
            ArgumentNullException.ThrowIfNull(postalUser);

            _usersByPostalCode.TryAdd(postalUser.PostalCode, postalUser);
        }

        public void Send(IPostalUser fromPostalUser, string toPostalCode, Parcel parcel)
        {
            ArgumentNullException.ThrowIfNull(fromPostalUser);

            if (_usersByPostalCode.TryGetValue(toPostalCode, out IPostalUser toPostalUser))
            {
                toPostalUser.ReceiveParcel(fromPostalUser.PostalCode, parcel);
            }
            // Make sure the sender is not rejecting parcels, otherwise we may end up with an infinite callback.
            else if (fromPostalUser is not RejectingParcelUser)
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
    public class RejectingParcelUser(string postalCode, IParcelService parcelService) : IPostalUser
    {
        public string PostalCode { get; } = postalCode;

        public void ReceiveParcel(string fromPostalCode, Parcel parcel)
        {
            // Rejects all deliveries, send it back.
            parcelService.Send(this, fromPostalCode, parcel);
        }

        public void SendParcel(string toPostalCode, Parcel parcel)
        {
            parcelService.Send(this, toPostalCode, parcel);
        }
    }

    /// <summary>
    /// Concrete Colleague
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IPostalUser">Colleague</see> interface and
    /// communicates with other <see cref="IPostalUser">Colleagues</see> through its <see cref="IParcelService">Mediator</see>.
    /// </remarks>
    public class AcceptingParcelUser(string postalCode, IParcelService parcelService) : IPostalUser
    {
        private readonly List<Parcel> _parcelsReceived = [];

        public IReadOnlyList<Parcel> ParcelsReceived => _parcelsReceived.AsReadOnly();

        public string PostalCode => postalCode;

        public void ReceiveParcel(string fromPostalCode, Parcel parcel)
        {
            _parcelsReceived.Add(parcel);
        }

        public void SendParcel(string toPostalCode, Parcel parcel)
        {
            parcelService.Send(this, toPostalCode, parcel);
        }
    }
}