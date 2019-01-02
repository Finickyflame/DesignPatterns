using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class ChainOfResponsibility : DesignPattern
    {
        /// <summary>
        /// Client
        /// </summary>
        /// <remarks>
        /// Initiates the request to a ConcreteHandler object on the chain.
        /// </remarks>
        public override void Execute()
        {
            // Create the customer support team
            CustomerSupport frontDesk = new FrontDesk();
            CustomerSupport lead = new Lead();
            CustomerSupport engineer = new Engineer();
            CustomerSupport manager = new Manager();

            // Define the hierarchy: [FrontDesk] -> [Lead] -> [Engineer] -> [Manager]
            frontDesk.SetEscalation(lead);
            lead.SetEscalation(engineer);
            engineer.SetEscalation(manager);


            var simpleProblem = new Problem(ProblemSeverity.Simple);
            frontDesk.SolveProblem(simpleProblem);
            // The problem should be directly resolved by the FrontDesk
            Assert.True(simpleProblem.Solved);
            Assert.Equal(frontDesk, simpleProblem.SolvedBy);


            var criticalProblem = new Problem(ProblemSeverity.Critical);
            frontDesk.SolveProblem(criticalProblem);
            // The problem should go through [FrontDesk] -> [Lead] -> [Engineer]
            Assert.True(criticalProblem.Solved);
            Assert.NotEqual(frontDesk, criticalProblem.SolvedBy);
            Assert.NotEqual(lead, criticalProblem.SolvedBy);
            Assert.Equal(engineer, criticalProblem.SolvedBy);
        }

        #region Definition

        public enum ProblemSeverity
        {
            NoProblem,
            Simple,
            Troublesome,
            Urgent,
            Critical
        }

        /// <summary>
        /// Handler
        /// </summary>
        /// <remarks>
        /// - Defines an interface for handling the requests.
        /// - Can implements the successor link.
        /// </remarks>
        public abstract class CustomerSupport
        {
            private CustomerSupport _escalatedSupport;

            public void SetEscalation(CustomerSupport escalatedSupport)
            {
                this._escalatedSupport = escalatedSupport;
            }

            public void SolveProblem(Problem problem)
            {
                if (this.CanSolveProblem(problem))
                {
                    problem.SolveBy(this);
                }
                else
                {
                    this.EscalateProblem(problem);
                }
            }

            /* The concrete implementations must define the severities they can handle */
            protected abstract IEnumerable<ProblemSeverity> Responsibilities { get; }

            private bool CanSolveProblem(Problem problem)
            {
                return this.Responsibilities?.Any(severity => severity == problem.Severity) ?? false;

                /*
                 * Longer version:
                 *
                 * if(this.Responsibilities != null)
                 * {
                 *     return this.Responsibilities.Any(severity => severity == problem.Severity);
                 * }
                 * return false;
                 */
            }

            private void EscalateProblem(Problem problem)
            {
                this._escalatedSupport?.SolveProblem(problem);
            }
        }

        public class Problem
        {
            public Problem(ProblemSeverity severity)
            {
                this.Severity = severity;
            }


            public ProblemSeverity Severity { get; }

            public bool Solved => this.SolvedBy != null;

            public CustomerSupport SolvedBy { get; private set; }


            public void SolveBy(CustomerSupport customerSupport)
            {
                this.SolvedBy = customerSupport;
            }
        }

        #endregion

        #region Concrete Implementations

        /// <summary>
        /// Concrete Handler
        /// </summary>
        /// <remarks>
        /// Handles requests it is responsible for.
        /// </remarks>
        public class FrontDesk : CustomerSupport
        {
            protected override IEnumerable<ProblemSeverity> Responsibilities
            {
                get
                {
                    yield return ProblemSeverity.NoProblem;
                    yield return ProblemSeverity.Simple;
                }
            }
        }

        /// <summary>
        /// Concrete Handler
        /// </summary>
        /// <remarks>
        /// Handles requests it is responsible for.
        /// </remarks>
        public class Lead : CustomerSupport
        {
            protected override IEnumerable<ProblemSeverity> Responsibilities
            {
                get
                {
                    yield return ProblemSeverity.Troublesome;
                }
            }
        }

        /// <summary>
        /// Concrete Handler
        /// </summary>
        /// <remarks>
        /// Handles requests it is responsible for.
        /// </remarks>
        public class Engineer : CustomerSupport
        {
            protected override IEnumerable<ProblemSeverity> Responsibilities
            {
                get
                {
                    yield return ProblemSeverity.Critical;
                }
            }
        }

        /// <summary>
        /// Concrete Handler
        /// </summary>
        /// <remarks>
        /// Handles requests it is responsible for.
        /// </remarks>
        public class Manager : CustomerSupport
        {
            protected override IEnumerable<ProblemSeverity> Responsibilities
            {
                get
                {
                    yield return ProblemSeverity.Urgent;
                }
            }
        }

        #endregion
    }
}