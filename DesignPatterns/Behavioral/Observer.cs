namespace DesignPatterns.Behavioral;

public class Observer
{
    [Fact]
    public void Execute()
    {
        var temperatureSensor = new TemperatureSensor();

        var heater = new Heater(temperatureSensor, 18); // 64° F
        var airConditioner = new AirConditioner(temperatureSensor, 21); // 70° F

        temperatureSensor.Temperature = 17; // 62° F
        Assert.True(heater.IsHeating);
        Assert.False(airConditioner.IsCooling);

        temperatureSensor.Temperature = 20; // 68° F
        Assert.False(heater.IsHeating);
        Assert.False(airConditioner.IsCooling);

        temperatureSensor.Temperature = 22; // 72° F
        Assert.False(heater.IsHeating);
        Assert.True(airConditioner.IsCooling);


        temperatureSensor.RemoveListener(heater);
        temperatureSensor.Temperature = 17; // 62° F
        Assert.False(heater.IsHeating);
        Assert.False(airConditioner.IsCooling);
        // heater is no longer subscribed, so it wasn't notified of the temperature changes.
    }

    /// <summary>
    /// Observer
    /// </summary>
    /// <remarks>
    /// Defines an updating interface for objects that should be notified of changes in a subject.
    /// </remarks>
    public interface ISensorListener
    {
        void Update();
    }

    /// <summary>
    /// Subject
    /// </summary>
    /// <remarks>
    /// - Knows its observers. Any number of Observer objects may observe a subject.
    /// - Provides a way of attaching and detaching Observer objects.
    /// </remarks>
    public abstract class Sensor
    {
        private readonly List<ISensorListener> _listeners = [];

        public void AddListener(ISensorListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(ISensorListener listener)
        {
            _listeners.Remove(listener);
        }

        protected void NotifyListeners()
        {
            foreach (ISensorListener listener in _listeners)
            {
                listener.Update();
            }
        }
    }

    /// <summary>
    /// Concrete subject
    /// </summary>
    /// <remarks>
    /// - Stores state of interest to Concrete Observer.
    /// - Sends a notification to its observers when its state changes.
    /// </remarks>
    public class TemperatureSensor : Sensor
    {
        private decimal _temperature;

        public decimal Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                NotifyListeners();
            }
        }
    }

    /// <summary>
    /// Concrete Observer
    /// </summary>
    /// <remarks>
    /// - Maintains a reference to the Concrete Subject.
    /// - Stores state that should stay consistent with the Subject's.
    /// - Implements the Observer updating interface to keep its state consistent with the subject's.
    /// </remarks>
    public class Heater : ISensorListener
    {
        private readonly TemperatureSensor _sensor;

        public Heater(TemperatureSensor sensor, decimal maxHeatingTemperature)
        {
            MaxHeatingTemperature = maxHeatingTemperature;
            _sensor = sensor;
            _sensor.AddListener(this);
        }

        public bool IsHeating { get; private set; }

        public decimal MaxHeatingTemperature { get; }

        public void Update()
        {
            IsHeating = _sensor.Temperature < MaxHeatingTemperature;
        }
    }

    /// <summary>
    /// Concrete Observer
    /// </summary>
    /// <remarks>
    /// - Maintains a reference to the Concrete Subject.
    /// - Stores state that should stay consistent with the Subject's.
    /// - Implements the Observer updating interface to keep its state consistent with the subject's.
    /// </remarks>
    public class AirConditioner : ISensorListener
    {
        private readonly TemperatureSensor _sensor;

        public AirConditioner(TemperatureSensor sensor, decimal minCoolingTemperature)
        {
            MinCoolingTemperature = minCoolingTemperature;
            _sensor = sensor;
            _sensor.AddListener(this);
        }

        public bool IsCooling { get; private set; }

        public decimal MinCoolingTemperature { get; }

        public void Update()
        {
            IsCooling = _sensor.Temperature > MinCoolingTemperature;
        }
    }
}