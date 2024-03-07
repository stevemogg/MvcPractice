using System.Diagnostics.CodeAnalysis;
namespace MvcPractice.Models
{
    //Strings to be used throughoutproject
    public class Strings
    {
        public string Title { get; set; }
        public string Add { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string Restore { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }
        public string SearchPlacholder { get; set; }
        public string AriaLabel { get; set; }
        public string Table { get; set; }
        //Record
        public string AddSuccess { get; set; }
        public string AddFail { get; set; }
        public string EditSuccess { get; set; }
        public string EditFail { get; set; }
        public string DeleteSuccess { get; set; }
        public string DeleteFail { get; set; }
        public string RestoreSuccess { get; set; }
        public string RestoreFail { get; set; }
        //Modal
        public string AddModalSuccess { get; set; }
        public string AddModalFail { get; set; }
        public string EditModalSuccess { get; set; }
        public string EditModalFail { get; set; }
        public string DeleteModalSuccess { get; set; }
        public string DeleteModalFail { get; set; }
        public string RestoreModalSuccess { get; set; }
        public string RestoreModalFail { get; set; }
        public string GetTypeMessage(bool modalMessage, string command, bool success)
        {
            string message = "";
            switch (modalMessage)
            {
                case true:
                    ;
                    switch (success)
                    {
                        case true:
                            if (command == "add")
                                return this.AddModalSuccess;
                            else if (command == "edit")
                                return this.EditModalSuccess;
                            else if (command == "delete")
                                return this.DeleteModalSuccess;
                            else if (command == "restore")
                                return this.RestoreModalSuccess;
                            break;
                        case false:
                            if (command == "add")
                                return this.AddModalFail;
                            else if (command == "edit")
                                return this.EditModalFail;
                            else if (command == "delete")
                                return this.DeleteModalFail;
                            else if (command == "restore")
                                return this.RestoreModalFail;
                            break;
                    }
                    break;
                case false:
                    switch (success)
                    {
                        case true:
                            if (command == "add")
                                return this.AddSuccess;
                            else if (command == "edit")
                                return this.EditSuccess;
                            else if (command == "delete")
                                return this.DeleteSuccess;
                            else if (command == "restore")
                                return this.RestoreSuccess;
                            break;
                        case false:
                            if (command == "add")
                                return this.AddFail;
                            else if (command == "edit")
                                return this.EditFail;
                            else if (command == "delete")
                                return this.DeleteFail;
                            else if (command == "restore")
                                return this.RestoreFail;
                            break;
                    }
                    break;
            }
            return message;
        }
        public Strings(string selectorName, string titleName)
        {
            Title = titleName;
            Add = $"add-{selectorName}-record";
            Edit = $"edit-{selectorName}-record";
            Delete = $"delete-{selectorName}-record";
            Restore = $"restore-{selectorName}-record";
            Filter = $"{selectorName}-filter";
            Search = $"{selectorName}-search";
            SearchPlacholder = $"Search {titleName}";
            AriaLabel = $"{selectorName}";
            Table = $"{selectorName}-table";

            //Record
            AddSuccess = $"The {selectorName} record has been added";
            DeleteSuccess = $"The {selectorName} record has been deleted";
            EditSuccess = $"The {selectorName} record has been edited";
            RestoreSuccess = $"The {selectorName} record has been restored";

            AddFail = $"Something went wrong adding the {selectorName} record";
            DeleteFail = $"Something went wrong deleting the {selectorName} record";
            EditFail = $"Something went wrong editing the {selectorName} record";
            RestoreFail = $"Something went wrong restoring the {selectorName} record";

            //Modal
            AddModalSuccess = $"The {selectorName} add modal has been shown";
            DeleteModalSuccess = $"The {selectorName} delete modal has been shown";
            EditModalSuccess = $"The {selectorName} edit modal has been shown";
            RestoreModalSuccess = $"The {selectorName} restore modal has been shown";

            AddModalFail = $"Something went wrong showing the {selectorName} add modal";
            DeleteModalFail = $"Something went wrong showing the {selectorName} delete modal";
            EditModalFail = $"Something went wrong showing the {selectorName} edit modal";
            RestoreModalFail = $"Something went wrong showing the {selectorName} restore modal";
        }
    }    
    //DATATABLES MODELS
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matches jQuery DataTables API Specification")]
    public class DataTablesServerSide
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public DataTablesSearch? search { get; set; }
        public DataTablesOrder[]? order { get; set; }
        public DataTablesColumns[]? columns { get; set; }
    }
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matches jQuery DataTables API Specification")]
    public class DataTablesSearch
    {
        public string? value { get; set; }
        public bool regex { get; set; }
    }
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matches jQuery DataTables API Specification")]
    public class DataTablesOrder
    {
        public int column { get; set; }
        public string? dir { get; set; }
    }
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matches jQuery DataTables API Specification")]
    public class DataTablesColumns
    {
        public string? data { get; set; }
        public string? name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DataTablesSearch[]? search { get; set; }
    }
}
