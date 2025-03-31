namespace Axon_Job_App.Features.Jobs;

public enum JobStatus
{
    Draft,
    Published,
    Closed,
    Archived
}

public enum JobType
{
    Permanent,
    Contract,
    PartTime,
    Temporary,
    Internship
}

public enum PaymentType
{
    PerHour,
    PerAnnum,
    Daily,
    Monthly,
    ProjectBased
}

public enum AssignmentStatus
{
    Active,
    Completed,
    Terminated,
    OnHold
}