using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Observer : DesignPattern
    {
        public override void Execute()
        {
            var temperatureSensor = new TemperatureSensor();

            var heater = new Heater(temperatureSensor, maxHeatingTemperature: 18); // 64° F
            var airConditioner = new AirConditioner(temperatureSensor, minCoolingTemperature: 21); // 70° F

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

        #region Definition

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
            private readonly IList<ISensorListener> _listeners;

            protected Sensor()
            {
                this._listeners = new List<ISensorListener>();
            }

            public void AddListener(ISensorListener listener)
            {
                this._listeners.Add(listener);
            }

            public void RemoveListener(ISensorListener listener)
            {
                this._listeners.Remove(listener);
            }

            protected void NotifyListeners()
            {
                foreach (ISensorListener listener in this._listeners)
                {
                    listener.Update();
                }
            }
        }

        #endregion

        #region Concrete implementation

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
                get => this._temperature;
                set
                {
                    this._temperature = value;
                    this.NotifyListeners();
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
                this.MaxHeatingTemperature = maxHeatingTemperature;
                this._sensor = sensor;
                this._sensor.AddListener(this);
            }

            public bool IsHeating { get; private set; }

            public decimal MaxHeatingTemperature { get; }

            public void Update()
            {
                this.IsHeating = this._sensor.Temperature < this.MaxHeatingTemperature;
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
                this.MinCoolingTemperature = minCoolingTemperature;
                this._sensor = sensor;
                this._sensor.AddListener(this);
            }

            public bool IsCooling { get; private set; }

            public decimal MinCoolingTemperature { get; }

            public void Update()
            {
                this.IsCooling = this._sensor.Temperature > this.MinCoolingTemperature;
            }
        }

        #endregion
    }
}