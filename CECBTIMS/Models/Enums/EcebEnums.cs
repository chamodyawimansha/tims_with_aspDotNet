namespace CECBTIMS.Models.Enums
{
    public enum Title
    {
        Dr = 1,
        Eng = 2,
        Mr = 3,
        Mrs = 4,
        Miss = 5,
        Null = 6
    }

    public enum RecruitmentType
    {
        Permanent = 1,
        FixedTermContract,
        JobContract,
        Enhance,
        //LabourContract
        Assignment,
        //ManPower
        Null
    }

    public enum EmployeeStatus
    {
        Working = 1,
        Resigned,
        Retired,
        Demise,
        NoPayLeave,
        StudyLeave,
        VacatedPost,
        ReleasedToCESL,
        Terminated,
        Null
    }

    public enum ContractType
    {
        test = 1,
    }
}