using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Data;

namespace easyMoney.Utilities
{
    public class ObjectDescriptor
    {
        private const String TypeFormat = "[{0}]";
        private const String ValueFoundFormat = "{0}: {1}";
        private const String ValueExceptionFormat = "!{0}: {1} - {2}";
        private int depth = 1;
        private int level = 0;
        private TextWriter writer = null;

        public ObjectDescriptor(TextWriter writer, int depth = 2)
        {
            this.depth = depth;
            this.writer = writer;

        }

        public void Describe(object toDescribe)
        {
            if (toDescribe != null)
            {

                writeObject(String.Format(TypeFormat, toDescribe.GetType().Name), toDescribe);
            }
            else
            {
                writeValue(null, toDescribe);
            }
        }

        private void writeObject(String name, object value)
        {
            writeIndent();
            writer.WriteLine(name);
            writeIndent();
            writer.WriteLine("{");
            level++;
            IEnumerable enumerableElement = value as IEnumerable;
            if (enumerableElement != null)
            {
                foreach (object item in enumerableElement)
                {
                    if (item is IEnumerable && !(item is string))
                    {
                        writeValue(name, "...");
                    }
                    if (level < depth)
                    {
                        writeObject(String.Format(TypeFormat, item.GetType().Name), item);
                    }
                }
            }
            else
            {
                MemberInfo[] members = value.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                bool isDataRow = value.GetType().IsSubclassOf(typeof(DataRow));
                foreach (MemberInfo m in members)
                {
                    try
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            // some protections for DataSet items (no need to show these in log)
                            if (isDataRow)
                            {
                                if ((f != null) && ((f.Name.Equals("Table")) || (f.Name.Equals("Item")))) continue;
                                if ((p != null) && ((p.Name.Equals("Table")) || (p.Name.Equals("Item")))) continue;
                            }

                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if ((t.IsValueType) || (t == typeof(string)) || (level >= depth))
                            {
                                writeValue(m.Name, f != null ? f.GetValue(value) : p.GetValue(value, null));
                            }
                            else
                            {
                                writeObject(m.Name, f != null ? f.GetValue(value) : p.GetValue(value, null));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        writeIndent();
                        writer.WriteLine(ValueExceptionFormat, m.Name, e.GetType().Name, e.Message);
                    }
                }
            }

            level--;
            writeIndent();
            writer.WriteLine("}");
        }

        private void writeValue(String name, object value)
        {
            writeIndent();
            String formattedValue = String.Empty;
            if (value == null)
            {
                formattedValue = "null";
            }
            else if (value is DateTime)
            {
                formattedValue = ((DateTime)value).ToString();
            }
            else if ((value is ValueType) || (value is string))
            {
                formattedValue = value.ToString();
            }
            else if (value is IEnumerable)
            {
                formattedValue = String.Format("IEnumerable({0})", value.GetType().Name);
            }
            else
            {
                formattedValue = String.Format("{0} ({1})", value.GetType().Name, value.ToString());
            }
            writer.WriteLine(ValueFoundFormat, name, formattedValue);
        }

        private void writeIndent()
        {
            for (int i = 0; i < level; i++) writer.Write("   ");
        }

    }
}
