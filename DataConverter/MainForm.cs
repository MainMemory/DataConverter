using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataConverter
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		Properties.Settings Settings;

		static readonly string[] types = {
							"Bytes",
							"UTF-8",
							"ASCII",
							"Unicode",
							"UTF-16",
							"Base64",
							"Byte",
							"SByte",
							"Short",
							"Int16",
							"UShort",
							"UInt16",
							"Int",
							"Int32",
							"UInt",
							"UInt32",
							"Long",
							"Int64",
							"ULong",
							"UInt64",
							"Single",
							"Float",
							"Double",
							"DateTime",
							"TimeSpan",
						 };

		private void Form1_Load(object sender, EventArgs e)
		{
			Settings = Properties.Settings.Default;
			List<string> t = new List<string>(types);
			foreach (var item in Encoding.GetEncodings())
				if (!t.Contains(item.Name, StringComparer.OrdinalIgnoreCase))
					t.Add(item.Name);
			string[] t2 = t.ToArray();
			comboBox1.Items.AddRange(t2);
			comboBox2.Items.AddRange(t2);
			comboBox1.Text = Settings.Type1;
			comboBox2.Text = Settings.Type2;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			byte[] bytes;
			string type = comboBox1.Text.ToLowerInvariant();
			string data = richTextBox1.Text;
			switch (type)
			{
				case "bytes":
					StringBuilder d2 = new StringBuilder(data.Length);
					foreach (char item in data)
						if (!char.IsWhiteSpace(item))
							d2.Append(item);
					if (d2.Length % 2 == 1)
						d2.Append('0');
					List<byte> result = new List<byte>();
					for (int i = 0; i < data.Length / 2; i++)
						result.Add(byte.Parse(data.Substring(i * 2, 2), NumberStyles.HexNumber));
					bytes = result.ToArray();
					break;
				case "byte":
					bytes = new byte[] { Convert.ToByte(data) };
					break;
				case "sbyte":
					bytes = new byte[] { (byte)Convert.ToSByte(data) };
					break;
				case "short":
				case "int16":
				case "sword":
					bytes = BitConverter.GetBytes(Convert.ToInt16(data));
					break;
				case "ushort":
				case "uint16":
				case "word":
					bytes = BitConverter.GetBytes(Convert.ToUInt16(data));
					break;
				case "int":
				case "int32":
				case "integer":
				case "sdword":
					bytes = BitConverter.GetBytes(Convert.ToInt32(data));
					break;
				case "uint":
				case "uint32":
				case "uinteger":
				case "dword":
					bytes = BitConverter.GetBytes(Convert.ToUInt32(data));
					break;
				case "long":
				case "int64":
				case "sqword":
					bytes = BitConverter.GetBytes(Convert.ToInt64(data));
					break;
				case "ulong":
				case "uint64":
				case "qword":
					bytes = BitConverter.GetBytes(Convert.ToUInt64(data));
					break;
				case "single":
				case "float":
					bytes = BitConverter.GetBytes(Convert.ToSingle(data));
					break;
				case "double":
					bytes = BitConverter.GetBytes(Convert.ToDouble(data));
					break;
				case "datetime":
				case "date":
				case "time":
					bytes = BitConverter.GetBytes(GetDate(data).Value.ToBinary());
					break;
				case "timespan":
					bytes = BitConverter.GetBytes(GetTimeSpan(data).Value.Ticks);
					break;
				case "base64":
					bytes = Convert.FromBase64String(data);
					break;
				case "ascii":
					bytes = System.Text.Encoding.ASCII.GetBytes(data);
					break;
				case "utf8":
				case "utf-8":
				case "text":
				case "string":
					bytes = System.Text.Encoding.UTF8.GetBytes(data);
					break;
				case "unicode":
				case "utf16":
				case "utf-16":
					bytes = System.Text.Encoding.Unicode.GetBytes(data);
					break;
				default:
					int cdpg = 0;
					if (int.TryParse(type, out cdpg))
						bytes = System.Text.Encoding.GetEncoding(cdpg).GetBytes(data);
					else
						bytes = System.Text.Encoding.GetEncoding(type).GetBytes(data);
					break;
			}
			type = comboBox2.Text.ToLowerInvariant();
			switch (type)
			{
				case "bytes":
					richTextBox2.Text = string.Join(" ", bytes.Select((item) => item.ToString("X2")).ToArray());
					break;
				case "byte":
					richTextBox2.Text = bytes[0].ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "sbyte":
					richTextBox2.Text = ((sbyte)bytes[0]).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "short":
				case "int16":
				case "sword":
					richTextBox2.Text = BitConverter.ToInt16(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "ushort":
				case "uint16":
				case "word":
					richTextBox2.Text = BitConverter.ToUInt16(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "int":
				case "int32":
				case "integer":
				case "sdword":
					richTextBox2.Text = BitConverter.ToInt32(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "uint":
				case "uint32":
				case "uinteger":
				case "dword":
					richTextBox2.Text = BitConverter.ToUInt32(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "long":
				case "int64":
				case "sqword":
					richTextBox2.Text = BitConverter.ToInt64(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "ulong":
				case "uint64":
				case "qword":
					richTextBox2.Text = BitConverter.ToUInt64(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "single":
				case "float":
					richTextBox2.Text = BitConverter.ToSingle(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "double":
					richTextBox2.Text = BitConverter.ToDouble(bytes, 0).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "datetime":
				case "date":
				case "time":
					richTextBox2.Text = DateTime.FromBinary(BitConverter.ToInt64(bytes, 0)).ToString(NumberFormatInfo.InvariantInfo);
					break;
				case "timespan":
					richTextBox2.Text = TimeSpan.FromTicks(BitConverter.ToInt64(bytes, 0)).ToString();
					break;
				case "base64":
					richTextBox2.Text = Convert.ToBase64String(bytes);
					break;
				case "ascii":
					richTextBox2.Text = Encoding.ASCII.GetString(bytes);
					break;
				case "utf8":
				case "utf-8":
				case "text":
				case "string":
					richTextBox2.Text = Encoding.UTF8.GetString(bytes);
					break;
				case "unicode":
				case "utf16":
				case "utf-16":
					richTextBox2.Text = Encoding.Unicode.GetString(bytes);
					break;
				default:
					int cdpg = 0;
					if (int.TryParse(type, out cdpg))
						richTextBox2.Text = Encoding.GetEncoding(cdpg).GetString(bytes);
					else
						richTextBox2.Text = Encoding.GetEncoding(type).GetString(bytes);
					break;
			}
		}

		public static DateTime? GetDate(string str)
		{
			if (str.Equals("yesterday", StringComparison.OrdinalIgnoreCase))
				return DateTime.UtcNow.Date.AddDays(-1);
			if (str.Equals("today", StringComparison.OrdinalIgnoreCase))
				return DateTime.UtcNow.Date;
			if (str.Equals("now", StringComparison.OrdinalIgnoreCase))
				return DateTime.UtcNow;
			if (str.Equals("localtime", StringComparison.OrdinalIgnoreCase))
				return DateTime.Now;
			if (str.Equals("tomorrow", StringComparison.OrdinalIgnoreCase))
				return DateTime.UtcNow.Date.AddDays(1);
			DateTime result = DateTime.MinValue;
			if (DateTime.TryParse(str, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AllowWhiteSpaces | System.Globalization.DateTimeStyles.AdjustToUniversal, out result))
				return result;
			return null;
		}

		public static char[] numberchars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

		public static TimeSpan? GetTimeSpan(string str)
		{
			TimeSpan result = TimeSpan.Zero;
			if (TimeSpan.TryParse(str, out result))
				return result;
			str = str.Replace(",", "").Replace(" ", "");
			string num = string.Empty;
			string type = string.Empty;
			List<KeyValuePair<string, string>> parts = new List<KeyValuePair<string, string>>();
			bool negative = false;
			if (str.StartsWith("-"))
			{
				negative = true;
				str = str.Substring(1);
			}
			foreach (char item in str)
			{
				if (Array.IndexOf(numberchars, item) > -1)
				{
					if (!string.IsNullOrEmpty(type))
					{
						if (string.IsNullOrEmpty(num)) return null;
						parts.Add(new KeyValuePair<string, string>(num, type));
						num = string.Empty;
						type = string.Empty;
					}
					num += item;
				}
				else
					type += item;
			}
			if (string.IsNullOrEmpty(num)) return null;
			parts.Add(new KeyValuePair<string, string>(num, type));
			result = TimeSpan.Zero;
			foreach (KeyValuePair<string, string> item in parts)
			{
				double number = double.Parse(item.Key, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
				switch (item.Value.ToLowerInvariant())
				{
					case "sweep":
					case "sweeps":
						result += TimeSpan.FromDays(790.833333333333 * number);
						break;
					case "y":
					case "yr":
					case "year":
					case "years":
						result += TimeSpan.FromDays(365 * number);
						break;
					case "w":
					case "wk":
					case "week":
					case "weeks":
						result += TimeSpan.FromDays(7 * number);
						break;
					case "d":
					case "day":
					case "days":
						result += TimeSpan.FromDays(number);
						break;
					case "h":
					case "hr":
					case "hour":
					case "hours":
						result += TimeSpan.FromHours(number);
						break;
					case "m":
					case "min":
					case "minute":
					case "minutes":
						result += TimeSpan.FromMinutes(number);
						break;
					case "":
					case "s":
					case "sec":
					case "second":
					case "seconds":
						result += TimeSpan.FromSeconds(number);
						break;
					case "cs":
					case "centisecond":
					case "centiseconds":
						result += TimeSpan.FromMilliseconds(number * 10);
						break;
					case "f":
					case "frame":
					case "frames":
					case "ntsc":
					case "ntscframe":
					case "ntscframes":
						result += TimeSpan.FromTicks((long)(number * (TimeSpan.TicksPerSecond / 60.0)));
						break;
					case "pal":
					case "palframe":
					case "palframes":
						result += TimeSpan.FromTicks((long)(number * (TimeSpan.TicksPerSecond / 50.0)));
						break;
					case "ms":
					case "millisecond":
					case "milliseconds":
						result += TimeSpan.FromMilliseconds(number);
						break;
					case "tick":
					case "ticks":
						result += TimeSpan.FromTicks((long)number);
						break;
					default:
						return null;
				}
			}
			if (negative) result = -result;
			return result;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Type1 = comboBox1.Text;
			Settings.Type2 = comboBox2.Text;
			Settings.Save();
		}
	}
}