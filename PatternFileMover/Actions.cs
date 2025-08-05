using System;
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
