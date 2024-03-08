namespace DesignPatterns.Behavioral;

public class Visitor
{
    [Fact]
    public void Execute()
    {
        Residence apartment = 
        [
            new Bathroom(),
            new Bedroom(),
            new Kitchen()
        ];

        var apartmentMaid = new Maid();
        Assert.Equal(0, apartmentMaid.ServiceCost);

        apartment.Accept(apartmentMaid);
        Assert.Equal(Maid.BathroomPrice + Maid.BedroomPrice, apartmentMaid.ServiceCost);

        var apartmentPlumber = new Plumber();
        Assert.Equal(Plumber.BasePrice, apartmentPlumber.ServiceCost);

        apartment.Accept(apartmentPlumber);
        Assert.Equal(Plumber.BasePrice + Plumber.BathroomPrice + Plumber.KitchenPrice, apartmentPlumber.ServiceCost);


        Residence house =
        [
            new Bathroom(),
            new Bathroom(),

            new Bedroom(),
            new Bedroom(),
            new Bedroom(),

            new Kitchen(),
            new LivingRoom()
        ];

        var houseMaid = new Maid();

        house.Accept(houseMaid);
        Assert.Equal(Maid.BathroomPrice * 2 + Maid.BedroomPrice * 3, houseMaid.ServiceCost);

        var housePlumber = new Plumber();

        house.Accept(housePlumber);
        Assert.Equal(Plumber.BasePrice + Plumber.BathroomPrice * 2 + Plumber.KitchenPrice, housePlumber.ServiceCost);
    }

    /// <summary>
    /// Element
    /// </summary>
    /// <remarks>
    /// Defines an Accept operation that takes a visitor as an argument.
    /// </remarks>
    public interface IRoom
    {
        void Accept(IResidenceVisitor visitor);
    }


    /// <summary>
    /// Visitor
    /// </summary>
    /// <remarks>
    /// Declares a Visit operation for each Concrete Element in the object structure.
    /// The operation's name and signature identifies the class that sends the Visit request to the visitor.
    /// That lets the visitor determine the concrete class of the element being visited, then the visitor can access the
    /// elements directly through its particular interface
    /// </remarks>
    public interface IResidenceVisitor
    {
        void Visit(Bathroom room);

        void Visit(Bedroom room);

        void Visit(Kitchen room);

        void Visit(LivingRoom room);
    }

    /// <summary>
    /// Object Structure
    /// </summary>
    /// <remarks>
    /// - Can enumerate its elements.
    /// - May provide a high-level interface to allow the visitor to visit its elements.
    /// - May either be a Composite(pattern) or a collection such as a list or a set.
    /// </remarks>
    public class Residence : IEnumerable<IRoom>
    {
        private readonly List<IRoom> _rooms = [];

        public void Accept(IResidenceVisitor visitor)
        {
            foreach (IRoom room in this)
            {
                room.Accept(visitor);
            }
        }

        public void Add(IRoom room)
        {
            _rooms.Add(room);
        }

        public IEnumerator<IRoom> GetEnumerator() => _rooms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_rooms).GetEnumerator();
    }

    /// <summary>
    /// Concrete Element
    /// </summary>
    /// <remarks>
    /// Implements an Accept operation that takes a visitor as an argument.
    /// </remarks>
    public class Bathroom : IRoom
    {
        public void Accept(IResidenceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Concrete Element
    /// </summary>
    /// <remarks>
    /// Implements an Accept operation that takes a visitor as an argument.
    /// </remarks>
    public class Bedroom : IRoom
    {
        public void Accept(IResidenceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Concrete Element
    /// </summary>
    /// <remarks>
    /// Implements an Accept operation that takes a visitor as an argument.
    /// </remarks>
    public class Kitchen : IRoom
    {
        public void Accept(IResidenceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Concrete Element
    /// </summary>
    /// <remarks>
    /// Implements an Accept operation that takes a visitor as an argument.
    /// </remarks>
    public class LivingRoom : IRoom
    {
        public void Accept(IResidenceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Concrete Visitor
    /// </summary>
    /// <remarks>
    /// Implements each operation declared by the Visitor.
    /// Each operation implements a fragment of the algorithm defined for the corresponding class or object in the structure.
    /// Concrete Visitor provides the context for the algorithm and stores its local state.
    /// </remarks>
    public class Plumber : IResidenceVisitor
    {
        public const decimal BasePrice = 100;
        public const decimal BathroomPrice = 12.7m;
        public const decimal KitchenPrice = 15.3m;

        /// <summary>
        /// State
        /// </summary>
        /// <remarks>
        /// Accumulates results during the traversal of the structure.
        /// </remarks>
        public decimal ServiceCost { get; private set; } = BasePrice;

        public void Visit(Bathroom room)
        {
            ServiceCost += BathroomPrice;
        }

        public void Visit(Bedroom room)
        {
            // Does nothing here
        }

        public void Visit(Kitchen room)
        {
            ServiceCost += KitchenPrice;
        }

        public void Visit(LivingRoom room)
        {
            // Does nothing here
        }
    }

    /// <summary>
    /// Concrete Visitor
    /// </summary>
    /// <remarks>
    /// Implements each operation declared by the Visitor.
    /// Each operation implements a fragment of the algorithm defined for the corresponding class or object in the structure.
    /// Concrete Visitor provides the context for the algorithm and stores its local state.
    /// </remarks>
    public class Maid : IResidenceVisitor
    {
        public const decimal BathroomPrice = 62.5m;
        public const decimal BedroomPrice = 54.8m;

        /// <summary>
        /// State
        /// </summary>
        /// <remarks>
        /// Accumulates results during the traversal of the structure.
        /// </remarks>
        public decimal ServiceCost { get; private set; }

        public void Visit(Bathroom room)
        {
            ServiceCost += BathroomPrice;
        }

        public void Visit(Bedroom room)
        {
            ServiceCost += BedroomPrice;
        }

        public void Visit(Kitchen room)
        {
            // Does nothing here
        }

        public void Visit(LivingRoom room)
        {
            // Does nothing here
        }
    }
}