namespace DesignPatterns.Behavioral;

public class ChainOfResponsibility
{
    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Initiates the request to a ConcreteHandler object on the chain.
    /// </remarks>
    [Fact]
    public void Execute()
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
            _escalatedSupport = escalatedSupport;
        }

        public void SolveProblem(Problem problem)
        {
            if (CanSolveProblem(problem))
            {
                problem.SolveBy(this);
            }
            else
            {
                EscalateProblem(problem);
            }
        }

        /// <summary>
        /// The concrete implementations must define if it can solve the problem.
        /// </summary>
        protected abstract bool CanSolveProblem(Problem problem);

        private void EscalateProblem(Problem problem)
        {
            _escalatedSupport?.SolveProblem(problem);
        }
    }

    public class Problem(ProblemSeverity severity)
    {
        public ProblemSeverity Severity => severity;

        public bool Solved => SolvedBy != null;

        public CustomerSupport SolvedBy { get; private set; }


        public void SolveBy(CustomerSupport customerSupport)
        {
            SolvedBy = customerSupport;
        }
    }

    /// <summary>
    /// Concrete Handler
    /// </summary>
    /// <remarks>
    /// Handles requests it is responsible for.
    /// </remarks>
    public class FrontDesk : CustomerSupport
    {
        private static readonly ProblemSeverity[] HandledSeverities =
        [
            ProblemSeverity.NoProblem,
            ProblemSeverity.Simple
        ];

        protected override bool CanSolveProblem(Problem problem)
        {
            return Array.Exists(HandledSeverities, severity => problem.Severity == severity);
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
        protected override bool CanSolveProblem(Problem problem)
        {
            return problem.Severity == ProblemSeverity.Troublesome;
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
        protected override bool CanSolveProblem(Problem problem)
        {
            return problem.Severity == ProblemSeverity.Critical;
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
        protected override bool CanSolveProblem(Problem problem)
        {
            return problem.Severity == ProblemSeverity.Urgent;
        }
    }
}