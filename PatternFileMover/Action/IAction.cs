using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PatternFileMover.Action
{
    abstract class Action
    {
        public abstract bool ExecuteAction();

        public abstract bool Matches(DataGridViewRow row);

        public abstract void Prepare();
    }

    abstract class AbstractAction : Action
    {
        private readonly List<NameAssociationsData_v3> associations;
        private NameAssociationsData_v3 currentAssociation;
        private DataGridViewRow currentDataGridViewRow { get; set; }
        private string sourcePath;

        protected AbstractAction()
        {
            this.associations = new List<NameAssociationsData_v3>();
            this.associations = NameAssociations.LoadFromExistingConfigFile();
        }

        public override bool ExecuteAction()
        {
            Type type = this.GetType();
            
            MethodInfo methodInfoAction = type.GetMethod(
                "Do" + this.GetType().Name,
                BindingFlags.Public | BindingFlags.Instance
            );

            MethodInfo methodInfoMatches = type.GetMethod(
                "Matches",
                BindingFlags.Public | BindingFlags.Instance
            );

            MethodInfo methodInfoPrepare = type.GetMethod(
                "Prepare",
                BindingFlags.Public | BindingFlags.Instance
            );

            if (methodInfoAction == null || methodInfoMatches == null || methodInfoPrepare == null)
            {
                throw new NotImplementedException();
            }

            foreach (NameAssociationsData_v3 _association in this.associations)
            {
                this.currentAssociation = _association;

                var matches = (bool) methodInfoMatches.Invoke(
                    this,
                    new object[] { this.currentDataGridViewRow }
                );

                if (matches)
                {
                    methodInfoPrepare.Invoke(this, new object[] { });
                    return (bool) methodInfoAction.Invoke(this, new object[] { });
                }
            }

            return false;
        }

        public override bool Matches(DataGridViewRow row)
        {
            this.currentDataGridViewRow = row;

            if (this.currentAssociation.Action.ToString() == this.GetType().Name)
            {
                return true;
            }

            return false;
        }

        public override void Prepare()
        {
            this.sourcePath = this.currentDataGridViewRow.Cells[0].Value.ToString();
        }

        protected DataGridViewRow GetCurrent()
        {
            return this.currentDataGridViewRow;
        }

        protected string GetFileExtension()
        {
            return this.currentAssociation.FileExtension;
        }

        protected string GetSearchPattern()
        {
            return this.currentAssociation.SearchPattern;
        }

        protected string GetSourcePath()
        {
            return this.sourcePath;
        }

        protected string GetTargetPath()
        {
            return this.currentAssociation.TargetDirectory;
        }

        public void SetCurrent(DataGridViewRow row)
        {
            this.currentDataGridViewRow = row;
        }

        public static List<Type> GetActions()
        {
            Type[] availableActions =  Assembly.GetExecutingAssembly().GetTypes().Where(
                t => String.Equals(t.Namespace, "PatternFileMover.Action", StringComparison.Ordinal)
            ).ToArray();
                
            var list = new List<Type>();
            foreach (var action in availableActions)
            {
                if (action.BaseType.ToString() == "PatternFileMover.Action.AbstractAction")
                {
                    list.Add(action);
                }
            }
            
            return list;
        }
    }
}
