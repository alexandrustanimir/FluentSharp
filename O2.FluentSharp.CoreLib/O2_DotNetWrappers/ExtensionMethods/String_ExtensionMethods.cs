using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using O2.Kernel.CodeUtils;
using System.Net;
using O2.Kernel;

//O2File:../PublicDI.cs
//O2File:../PublicDI.cs
//O2File:../CodeUtils/O2Kernel_Processes.cs

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class String_ExtensionMethods
    {
        public static string    str(this object _object)
        {
            return (_object != null) ? _object.ToString() : "[null value]";
        }        
        public static string    str(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }
        
        public static bool      eq(this string string1, string string2)
        {
            return (string1 == string2);
        }
        public static void      eq(this string string1, string stringToFind, Action onMatch)
        {
            string1.eq(new [] {stringToFind}, onMatch);
        }        
        public static void      eq(this string string1, List<string> stringsToFind, Action onMatch)
        {
            string1.eq(stringsToFind.ToArray(), onMatch);
        }
        public static void      eq(this string string1, string[] stringsToFind, Action onMatch) 
        {
            foreach(var stringToFind in stringsToFind)
                if (string1 == stringToFind)
                {
                    onMatch();
                    return;
                }
        }        
        public static bool      neq(this string string1, string string2)
        {
            return (string1 != string2);
        }        

        public static bool      contains(this string targetString, string stringToFind)
        {
            return (stringToFind != null)
                        ? targetString.Contains(stringToFind)
                        : false;
        }
        public static bool      contains(this string targetString, List<string> stringsToFind)
        {
            return targetString.contains(stringsToFind.ToArray());
        }
        public static bool      contains(this string targetString, params string[] stringsToFind)
        {
            if (stringsToFind.notNull())
            {
                foreach (var stringToFind in stringsToFind)
                    if (targetString.Contains(stringToFind))
                        return true;
            }
            return false;
        }   

        public static bool      starts(this string textToSearch, List<string> stringsToFind)
        {
            if (textToSearch.valid() && stringsToFind.notNull())
                foreach(var stringToFind in stringsToFind)
                    if (textToSearch.starts(stringToFind))
                        return true;
            return false;
        }
        public static bool      starts(this string stringToSearch, string stringToFind)
        {
            return stringToSearch.StartsWith(stringToFind);
        }
        public static void      starts(this string stringToSearch, string[] stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }
        public static void      starts(this string stringToSearch, List<string> stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }
        public static void      starts(this string stringToSearch, List<string> stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind.ToArray(), invokeOnMatchIfEqual, onMatch);
        }
        public static void      starts(this string stringToSearch, string[] stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            foreach(var stringToFind in stringsToFind)
                if (stringToSearch.starts(stringToFind, invokeOnMatchIfEqual, onMatch))
                    return;
        }
        public static void      starts(this string stringToSearch, string textToFind, Action<string> onMatch)
        {
            stringToSearch.starts(textToFind, true, onMatch);
        }
        public static bool      starts(this string stringToSearch, string textToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            if (stringToSearch.starts(textToFind))
            {
                var textForCallback = stringToSearch.remove(textToFind);
                if (invokeOnMatchIfEqual || textForCallback.valid())
                {
                    onMatch(textForCallback);
                    return true;
                }
            }
            return false;
        }
        public static bool      nstarts(this string stringToSearch, string stringToFind)
        {
            return ! starts(stringToSearch, stringToFind);
        }
        public static bool      ends(this string string1, string string2)
        {
            return string1.EndsWith(string2);
        }

        public static bool      inValid(this string _string)
        {
            return !_string.valid();
        }

        public static bool notValid(this string _string)
        {
            return !_string.valid();
        }

        public static bool      valid(this string _string)
        {
            if (_string != null && false == string.IsNullOrEmpty(_string))
                if (_string.Trim() != "")
                    return true;
            return false;
        }
        public static bool      empty(this string _string)
        {
            return !(_string.valid());
        }
        public static bool      validString(this object _object)
        {
            return _object.str().valid();
        }

        public static string    format(this string format, params object[] parameters)
        {
            if (format == null)
                return "";
            if (parameters == null)
                return format;
            try
            {
                return string.Format(format, parameters);
            }
            catch (Exception ex)
            {
                ex.log("error applying string format: " + format ?? "[null]");
                return "";
            }
        }
        public static string    remove(this string _string, params string[] stringsToRemove)
        {
            return _string.replaceAllWith("", stringsToRemove);
        }
        public static string    toSpace(this string _string, params string[] stringsToChange)
        {
            return _string.replaceAllWith(" ", stringsToChange);
        }
        public static string    replace(this string targetString, string stringToFind, string stringToReplaceWith)
        {
            if (stringToFind.notNull())
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            // need to find a better way to do this replace (maybe using regex) since this pattern was causing some nasty side effects (for example when replacing \n with Environment.NewLine)
            //targetString = targetString.Replace(stringToFind.lower(), stringToReplaceWith);
            //targetString = targetString.Replace(stringToFind.upper(), stringToReplaceWith);
            return targetString;
        }
        public static string    replaceAllWith(this string targetString, string stringToReplaceWith, params string[] stringsToFind)
        {
            foreach (var stringToFind in stringsToFind)
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            return targetString;
        }
        public static int       size(this string _string)
        {
            if (_string.notNull())
                return _string.Length;
            return 0;
        }

        public static string    line(this string firstString, string secondString)
        {
            return firstString.line() + secondString;
        }
        public static string    line(this string firstString)
        {
            return firstString + Environment.NewLine;
        }
        public static string    lineBefore(this string targetString)
        {
            return Environment.NewLine + targetString;
        }
        public static string    lineBeforeAndAfter(this string targetString)
        {
            return Environment.NewLine + targetString + Environment.NewLine;
        }

        public static bool      isInt(this string value)
        {
            int a = 0;
            return Int32.TryParse(value, out a);
        }
        public static int       toInt(this string _string)
        {
            Int32 value;
            Int32.TryParse(_string, out value);
            return value;
        }

        public static string    hex(this byte value)
        {
            return Convert.ToString(value, 16).caps();
        }
        public static string    hex(this int value)
        {
            return Convert.ToString(value, 16).caps();
        }

        public static string    caps(this string value)
        {
            return value.ToUpper();
        }
        public static string    lower(this string _string)
        {
            return _string.ToLower();
        }
        public static string    upper(this string _string)
        {
            return _string.ToUpper();
        }

        public static string    lowerCaseFirstLetter(this string targetString)
        {
            if (targetString.valid())
                return targetString[0].str().lower() + targetString.removeFirstChar();
            return targetString;
        }

        public static string    fixCRLF(this string stringToFix)
        {
            //old method (had some misses)
           /* if (stringToFix.contains(Environment.NewLine))
                return stringToFix;
            if (stringToFix.contains("\n"))
                return stringToFix.Replace("\n", Environment.NewLine);
            return stringToFix;*/
            return stringToFix.Replace(Environment.NewLine, "\n")
                              .Replace("\n", Environment.NewLine);
        }    

        public static string    ascii(this byte value)
        {
            return Encoding.ASCII.GetString(new[] { value });
        }        
        public static string    ascii(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
        public static string    unicode(this byte value)
        {
            return Encoding.Unicode.GetString(new[] { value });
        }
        public static string    unicode(this byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes);
        }
        
        public static List<string>  strings(this byte[] bytes, bool ignoreCharZeroAfterValidChar, int minimumStringSize)
        {
            //this method is only really good to find ASCII binary strings
            var extractedStrings = new List<string>();
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                var value = bytes[i];
                if (value > 31 && value < 127) // see http://www.asciitable.com/
                {
                    var str = value.ascii();
                    stringBuilder.Append(str);
                    if (ignoreCharZeroAfterValidChar)
                        if (bytes[i + 1] == 0)
                            i++;
                }
                else
                {
                    if (stringBuilder.Length > 0)
                    {
                        if (minimumStringSize == -1 || stringBuilder.Length > minimumStringSize)
                            extractedStrings.Add(stringBuilder.ToString());
                        stringBuilder = new StringBuilder();
                    }
                }
            }            
            return extractedStrings;
        }

        public static void      removeLastChar(this StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
        public static string    removeLastChar(this string _string)
        {
            if (_string.Length > 0)
                return _string.Remove(_string.Length - 1, 1);
            return _string;
        }
        public static string    removeFirstChar(this string _string)
        {
            return (_string.Length > 0)
                ? _string.Substring(1)
                : _string;

        }
        public static string    replaceLast(this string stringToSearch, string findString, string replaceString)
        {
            var lastIndexOf = stringToSearch.LastIndexOf(findString);
            lastIndexOf.str().info();
            if (lastIndexOf > -1)
            {
                var beforeSubstring = stringToSearch.Substring(0, lastIndexOf);
                var afterString_StartPosition = (lastIndexOf + findString.size());
                var afterString = (afterString_StartPosition < stringToSearch.size())
                                    ? stringToSearch.Substring(afterString_StartPosition)
                                    : "";
                return "{0}{1}{2}".format(beforeSubstring, replaceString, afterString);
            }
            return "";
        }

        public static string    appendGuid(this string _string)
        {
            return "{0} {1}".format(_string, Guid.NewGuid());
        }

        public static string  trim(this string _string)
        {           
            if (_string.valid())
                return _string.Trim();
            return _string;
        }
        public static string    pad(this string targetString, int totalWidth)
        {
            return targetString.PadLeft(totalWidth);
        }
        public static int       index(this string targetString, string stringToFind)
        {
            return targetString.IndexOf(stringToFind);
        }

        public static int       indexLast(this string targetString, string stringToFind)
        {
            return targetString.LastIndexOf(stringToFind);
        }
        
        public static string    add(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }
        public static string    insertAfter(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }
        public static string    insertBefore(this string targetString, string stringToAdd)
        {
            return stringToAdd + targetString;
        }      
        public static int       toIntFromHex(this string hexValue)
        {
            try
            {
                return Convert.ToInt32(hexValue, 16);
            }
            catch (Exception ex)
            {
                ex.log("in toIntFromHex when converting string: {0}".format(hexValue));
                return -1;
            }
        }
        public static string    repeat(this char charToRepeat, int count)
        {
            if (count > 0)
                return new String(charToRepeat, count);
            return "";
        }

        public static string    tempFile(this string postfixString)
        {
            return PublicDI.config.getTempFileInTempDirectory(postfixString);
        }
        public static string    o2Temp2Dir(this string tempFolderName)
        {
            return tempFolderName.o2Temp2Dir(true);
        }
        public static string    o2Temp2Dir(this string tempFolderName, bool appendRandomStringToFolderName)
        {
            if (tempFolderName.valid())
                if (appendRandomStringToFolderName)
                    return PublicDI.config.getTempFolderInTempDirectory(tempFolderName);
                else
                {
                    var tempFolder = Path.Combine(PublicDI.config.O2TempDir, tempFolderName);
                    O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(tempFolder);
                    return tempFolder;
                }
            return PublicDI.config.O2TempDir;
        }
        public static string    tempO2Dir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }
        public static string    tempDir(this string tempFolderName, bool appendRandomStringToFolderName)
        {
            return o2Temp2Dir(tempFolderName, appendRandomStringToFolderName);
        }
        public static string    tempDir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }


        public static Exception logStackTrace(this Exception ex)
		{
			"Error strackTrace: \n\n {0}".error(ex.StackTrace);
			return ex;
		}		
		public static string    ifEmptyUse(this string _string , string textToUse)
		{
			return (_string.empty() )
						?  textToUse
						: _string;
		}		
		public static string    upperCaseFirstLetter(this string _string)
		{
			if (_string.valid())
			{
				return _string[0].str().upper() + _string.subString(1); 
			}
			return _string;
		}										
		public static string    append(this string _string, string stringToAppend)
		{
			return _string + stringToAppend;
		}		
		public static string    insertAt(this string _string,  int index, string stringToInsert)
		{
			return _string.Insert(index,stringToInsert);
		}		
		public static string    subString(this string _string, int startPosition)
		{
			if (_string.size() < startPosition)
				return "";
			return _string.Substring(startPosition);
		}		
		public static string    subString(this string _string,int startPosition, int size)
		{
			var subString = _string.subString(startPosition);
			if (subString.size() < size)
				return subString;
			return subString.Substring(0,size);
		}		
		public static string    subString_After(this string _string, string _stringToFind)
		{
			var index = _string.IndexOf(_stringToFind);
			if (index >0)
			{
				return _string.subString(index + _stringToFind.size());
			}
			return "";
		}

        public static string subString_Before(this string stringToProcess, string untilString)
        {
            var lastIndex = stringToProcess.indexLast(untilString);
            return (lastIndex > 0)
                        ? stringToProcess.subString(0, lastIndex)
                        : stringToProcess;
        }

		public static string    lastChar(this string _string)
		{
			if (_string.size() > 0)
				return _string[_string.size()-1].str();
			return "";			
		}		
		public static bool      lastChar(this string _string, char lastChar)
		{
			return _string.lastChar(lastChar.str());
		}		
		public static bool      lastChar(this string _string, string lastChar)
		{
			return _string.lastChar() == lastChar;
		}		
		public static string    firstChar(this string _string)
		{
			if (_string.size() > 0)
				return _string[0].str();
			return "";			
		}		
		public static bool      firstChar(this string _string, char lastChar)
		{
			return _string.firstChar(lastChar.str());
		}		
		public static bool      firstChar(this string _string, string lastChar)
		{
			return _string.firstChar() == lastChar;
		}		

		public static string    add_RandomLetters(this string _string)
		{
			return _string.add_RandomLetters(10);
		}		
		public static string    add_RandomLetters(this string _string, int count)
		{
			return "{0}_{1}".format(_string,count.randomLetters());
		}		
		public static int       randomNumber(this int max)
		{
			return max.random();
		}				
		public static string    ascii(this int _int)
		{
			try
			{				
				return ((char)_int).str();					
			}
			catch(Exception ex)
			{
				ex.log();
				return "";
			}
		}		

		public static Guid      next(this Guid guid)
		{
			return guid.next(1);
		}		
		public static Guid      next(this Guid guid, int incrementValue)
		{			
			var guidParts = guid.str().split("-");
			var lastPartNextNumber = Int64.Parse(guidParts[4], System.Globalization.NumberStyles.AllowHexSpecifier);
			lastPartNextNumber+= incrementValue;
			var lastPartAsString = lastPartNextNumber.ToString("X12");
			var newGuidString = "{0}-{1}-{2}-{3}-{4}".format(guidParts[0],guidParts[1],guidParts[2],guidParts[3],lastPartAsString);
			return new Guid(newGuidString); 					
		}		
		public static Guid      emptyGuid(this Guid guid)
		{
			return Guid.Empty;
		}		
		public static Guid      newGuid(this string guidValue)
		{
			return Guid.NewGuid();
		}		
		public static Guid      guid(this string guidValue)
		{
			if (guidValue.inValid())
				return Guid.Empty;			
			return new Guid(guidValue);		
		}		
		public static bool      isGuid(this String guidValue)
		{
			try
			{
				new Guid(guidValue);
				return true;
			}
			catch
			{
				return false;
			}
		}		

		public static bool      toBool(this string _string)
		{
			try
			{
				if (_string.valid())
					return bool.Parse(_string);				
			}
			catch(Exception ex)
			{
				"in toBool, failed to convert provided value ('{0}') into a boolean: {2}".format(_string, ex.Message);				
			}
			return false;
		}		
		public static double    toDouble(this string _string)
		{
			try
			{
				if (_string.valid())
					return Double.Parse(_string);				
			}
			catch(Exception ex)
			{
				"in toDouble, failed to convert provided value ('{0}') into a double: {2}".format(_string, ex.Message);				
			}
			return default(double);
		}		
		public static IPAddress toIPAddress(this string _string)
		{
			try
			{
				var ipAddress = IPAddress.Loopback;
				IPAddress.TryParse(_string, out ipAddress);
				return ipAddress;
			}
			catch(Exception ex)
			{
				"Error in toIPAddress: {0}".error(ex.Message);
				return null;
			}
		}		
		public static byte      hexToByte(this string hexNumber)
		{
			try
			{
				return Byte.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToByte]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return default(byte);
			}
		}		
		public static byte[]    hexToBytes(this List<string> hexNumbers)
		{
			var bytes = new List<byte>();
			foreach(var hexNumber in hexNumbers)
				bytes.add(hexNumber.hexToByte());
			return bytes.ToArray();
		}		
		public static int       hexToInt(this string hexNumber)
		{
			try
			{
				return Int32.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToInt]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return -1;
			}
		}		
		public static long      hexToLong(this string hexNumber)
		{
			try
			{
				return long.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToLong]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return -1;
			}
		}		
		public static string    hexToAscii(this string hexNumber)
		{
			var value = hexNumber.hexToInt();
			if (value > 0)
				return value.ascii();
			return "";
		}		
		public static string    hexToAscii(this List<string> hexNumbers)
		{
			var asciiString = new StringBuilder();
			foreach(var hexNumber in hexNumbers)
				asciiString.Append(hexNumber.hexToAscii());
			return asciiString.str();
		}
    }


    public static class Long_ExtensionMethods
	{		
		public static long      toLong(this double _double)
		{
			return (long)_double;
		}		
		public static long      add(this long _long, long value)
		{
			return _long + value;
		}				
	}
	
	public static class Int_ExtensionMethods
	{		
		public static int       toInt(this double _double)
		{
			return (int)_double;
		}		
		public static int       mod( this int num1, int num2)
		{
			return num1 % num2;
		}
		public static bool      mod0( this int num1, int num2)
		{
			return num1.mod(num2) ==0;
		}		
		public static string    intToBinaryString(this int number)
		{
			return Convert.ToString(number,2);
		}			
	}	
	
	public static class UInt_ExtensionMethods
	{
		public static uint      toUInt(this string value)
		{
			return UInt32.Parse(value);
		}
	}

    public static class _Extra_DateTime_ExtensionMethods
	{
		public static long      unixTime(this DateTime dateTime)
		{
			return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds.toLong();
		}		
		public static long      unixTime_Now(this int secondsToAdd)
		{
			return DateTime.UtcNow.unixTime().add(secondsToAdd);
		}		
		public static long      unixTimeStamp_InSeconds(this int secondsToAdd)
		{
			return secondsToAdd.unixTime_Now();
		}		
		public static bool      isDate(this string date)
		{
			try
			{
				DateTime.Parse(date);
				return true;
			}
			catch
			{
				return false;
			}
		}		
		public static string    timeSpan_ToString(this DateTime startDateTime)
		{
			return startDateTime.duration_to_Now();
		}		
		public static string    duration_to_Now(this DateTime startDateTime)
		{
			return  (DateTime.Now -  startDateTime).timeSpan_ToString();
		}				
		public static string    timeSpan_ToString(this TimeSpan timeSpan)
		{
			//return timeSpan.ToString("mm'm 'ss's 'ff'ms'");  //4.0 dependent
            return timeSpan.ToString();
		}
		
	}
}