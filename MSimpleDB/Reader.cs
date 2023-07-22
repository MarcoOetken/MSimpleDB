namespace MSimpleDB
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Static class for reading an EntityCollection from a file
    /// or a TextReader.
    /// </summary>
    public static class Reader
    {
        #region -------- Notes ----------------------------------------------
        #endregion


        #region -------- Variables ------------------------------------------
        #endregion


        #region -------- Constructor ----------------------------------------
        #endregion


        #region -------- Events ---------------------------------------------
        #endregion


        #region -------- Properties -----------------------------------------
        #endregion


        #region -------- Public Methods -------------------------------------
        #endregion

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="collection">EntityCollection to fill.</param>
        /// <param name="fullName">Complete filename.</param>
        /// <exception cref="System.InvalidOperationException">For wraong filetype or formating.</exception>
        public static void Load(EntityCollection collection, string fullName)
        {
            Load(collection, fullName, Encoding.GetEncoding("windows-1252"));
        }

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="collection">EntityCollection to fill.</param>
        /// <param name="fullName">Complete filename.</param>
        /// <param name="encoding">Encoding of file.</param>
        /// <exception cref="System.InvalidOperationException">For wraong filetype or formating.</exception>
        public static void Load(EntityCollection collection, string fullName, Encoding encoding)
        {
            using (StreamReader reader = new StreamReader(fullName, encoding))
            {
                Load(collection, reader);
            }
        }

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="collection">Collection (destination).</param>
        /// <param name="reader">Reader to read from.</param>
        public static void Load(EntityCollection collection, TextReader reader)
        {
            string? line = reader.ReadLine();
            if (line == null || line != "## SimpleDB file")
            {
                throw new InvalidOperationException("wrong filetype");
            }

            // read comment
            StringBuilder sb = new StringBuilder();
            line = reader.ReadLine();
            while (line != null
                && (line.Trim().Length == 0 || line.Trim().StartsWith("#", StringComparison.Ordinal)))
            {
                sb.AppendLine(line);
                line = reader.ReadLine();
            }

            if (sb.Length > 0) { collection.Comment = sb.ToString(); }

            // Find first attribute
            if (string.IsNullOrEmpty(line) || line[0] != ':')
            {
                throw new InvalidOperationException("wrong format");
            }

            // read all objects
            while (line != null)
            {
                line = ReadObject(line, reader, collection);
            }
            
            collection.AcceptChanges();
        }

        #region -------- Internal Methods -----------------------------------
        #endregion


        #region -------- Private Methods ------------------------------------
        #endregion

        private static string? ReadObject(string line, TextReader reader, EntityCollection collection)
        {
            Entity obj = new Entity();
            string? line2 = line;
            do
            {
                string name = line2?.Substring(1) ?? "<error>";
                StringBuilder sb = new StringBuilder();
                line2 = reader.ReadLine();
                while (line2 != null && (line2.Length == 0 || (line2[0] != ':' && line2[0] != '.')))
                {
                    if (sb.Length > 0 || line2.Length > 0)
                    {
                        sb.AppendLine(line2);
                    }

                    line2 = reader.ReadLine();
                }
                
                obj.Add(name, sb.ToString().TrimEnd());
            }
            while (line2 != null && line2[0] == '.');
            collection.Add(obj);
            return line2;
        }
    }
}
