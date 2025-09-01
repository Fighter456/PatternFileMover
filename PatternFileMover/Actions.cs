using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PatternFileMover
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AvailableActions
    {
        [Description("grid.Action.Move")]
        Move = 0,
        [Description("grid.Action.Copy")]
        Copy = 1,
        [Description("grid.Action.CopyOverwrite")]
        CopyOverwrite = 2,
        [Description("grid.Action.MoveOverwrite")]
        MoveOverwrite = 3,
        [Description("grid.Action.Delete")]
        Delete = 4,
    }

    public class AvailableAction
    {
        public AvailableAction() { }

        public string DisplayName { get; set; }
        public string Value { get; set; }
    }

    public class AvailableActionItems
    {
       public static List<AvailableAction> getAvailableActions()
        {
            var i18n = new ResourceManager(
                "PatternFileMover.NameAssociationsForm",
                typeof(NameAssociationsForm).Assembly
            );

            List<AvailableAction> availableActions = new List<AvailableAction>();
            foreach (AvailableActions action in (AvailableActions[])Enum.GetValues(typeof(AvailableActions)))
            {
                var actionName = (new EnumDescriptionTypeConverter(typeof(AvailableAction))).ConvertTo(
                    null,
                    CultureInfo.CurrentUICulture,
                    action,
                    typeof(string)
                );

                availableActions.Add(new AvailableAction() { 
                    DisplayName = actionName.ToString(),
                    Value = ((int)action).ToString() }
                );
            }

            return availableActions;
        }
    }

    class EnumDescriptionTypeConverter : EnumConverter
    {
        private readonly ResourceManager i18n;
        public EnumDescriptionTypeConverter(Type type) : base (type)
        {
            i18n = new ResourceManager(
                "PatternFileMover.NameAssociationsForm",
                typeof(NameAssociationsForm).Assembly
            );
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());

                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description))
                        {
                            return i18n.GetString(attributes[0].Description);
                        }

                        return value.ToString();
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
