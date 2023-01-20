using System.ComponentModel.DataAnnotations;

namespace SGMS.Enums
{
    public enum eTournamentStatus 
    {
        Active,Cancelled,Rescheduled
    }
    public enum eDistrict
    {
        [Display(Name = "Ehlanzeni District")]
        Ehlanzeni,
        [Display(Name = "Gert Sibande")]
        Gert,
        [Display(Name = "Nkangala District")]
        Nkangala

    }
    public enum eMunicipality
    {
        [Display(Name = "Bushbuckridge Local")]
        Bushbuckridge,
        [Display(Name = "Nkomazi Local")]
        Nkomazi,
        [Display(Name = "City of Mbombela Local")]
        Mbombela,
        [Display(Name = "Thaba Chweu Local")]
        ThabaChweu,
        [Display(Name = "Chief Albert Luthuli Local")]
        AlbertLuthuli,
        [Display(Name = "Dipaleseng Local")]
        Dipaleseng,
        [Display(Name = "Dr Pixley Ka Isaka Seme Local")]
        Pixley,
        [Display(Name = "Govan Mbeki Local")]
        GovanMbeki,
        [Display(Name = "Lekwa Local")]
        Lekwa,
        [Display(Name = "Mkhondo Local")]
        Mkhondo,
        [Display(Name = "Msukaligwa Local")]
        Msukaligwa,
        [Display(Name = "Dr JS Moroka Local")]
        Moroka,
        [Display(Name = "Emakhazeni Local")]
        Emakhazeni,
        [Display(Name = "Emalahleni Local")]
        Emalahleni,
        [Display(Name = "Steve Tshwete Local")]
        SteveTshwete,
        [Display(Name = "Thembisile Local")]
        Thembisile,
        [Display(Name = "Victor Khanye Local")]
        VictorKhanye

    }
    public enum eTask
    {
        Administrator,Developer,Organizer
    }
    public enum eRole
    {
        Manager,
        Director,
        Assistant,
        Coach,
        Player,
        [Display(Name = "Match Commissioner")]
        Commissioner
    }
    public enum eSysRole
    {
        None,Player,Coach,Referee,Admin
    }
    public enum eSponsorType
    {
        Organization,Individual
    }
    public enum eMatchStatus 
    {
        Started,Finished,Cancelled,Postponed
    }
    public enum eLevelofExperience
    {
        Beginner,
        Intermediate,
        Professional
    }
    public enum eChoice
    {
        [Display(Name = "Starting Eleven")]
        P,
        [Display(Name = "Substitute")]
        S,
        [Display(Name = "Injured (0)")]
        I,
        [Display(Name = "Absent")]
        A,
        [Display(Name = "Not Eligible")]
        N
    }
    public enum eYesNo
    {
        Yes,No,Unsure 
    }
    public enum eAttachmentType
    {
        ID,Passport,Qualification
    }
    public enum eRefereeType
    {
        Main,
        [Display(Name = "Assistant Referee")]
        AssistantReferee,
        Linesman,
        VAR,
        [Display(Name = "Forth Official")]
        ForthOfficial,
    }
    public enum eRefereeLevel
    {
        [Display(Name ="Level 1 (Basic)")]
        Level1,
        [Display(Name = "Level 2")]
        Level2,
        [Display(Name = "Level 3")]
        Level3,
        [Display(Name = "Level 4")]
        Level4,
        [Display(Name = "Level 5")]
        Level5,
        [Display(Name = "Level 6")]
        Level6,
        [Display(Name = "Level 7 (Highly Experienced)")]
        Level7
    }
    public enum eGender
    {
        Male, Female, Other
    }
    public enum eTournamentCycle
    {
        Annual,
        BiAnnual,
        Semester,
        Trimester
    }
    public enum eTournamentType
    {
        Knockout,League,Other
    }
    public enum ePosition
    {
        GoalKeeper,
        Midfielder,
        Defender,
        Striker,
        Captain,
        [Display(Name = "Center Back")]
        CenterBack,
        [Display(Name = "Full Back")]
        FullBack,
        Forward,
        Wingback,
        Winger,
        [Display(Name = "Attacking Midfielder")]
        AttackingMidfielder
    }
    public enum eTitle
    {
        Mr, Miss, Ms, Dr, Prof, Bishop, Pastor, Cllr, Lady, Lord,
        General, Captain, Earl, Sir, Mx
    }
    public enum eProvince
    {
        Mpumalanga,
        Gauteng,
        Limpopo,
        [Display(Name="Eastern Cape")]
        EasternCape,
        [Display(Name = "Northern Cape")]
        NorthenCape,
        [Display(Name = "KwaZulu Natal")]
        KwaZuluNatal,
        [Display(Name = "Free-State")]
        FreeState,
        [Display(Name = "Western Cape")]
        WesternCape,
        [Display(Name = "North West")]
        NorthWest

    }

    public enum QualStatus
    {
        Completed,
        Incomplete,
        [Display(Name = "In Progress")]
        InProgress
    }

}
