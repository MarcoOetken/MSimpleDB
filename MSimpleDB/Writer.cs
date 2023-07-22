namespace MSimpleDB
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Static class for writing an EntityCollection to a
    /// file or a TextWriter.
    /// </summary>
    public static class Writer
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
        /// Store the whole collection.
        /// </summary>
        /// <param name="collection">EntityCollection to store.</param>
        /// <param name="fullName">Complete filename.</param>
        public static void Store(EntityCollection collection, string fullName)
        {
            Store(collection, fullName, Encoding.GetEncoding("windows-1252"));
        }

        /// <summary>
        /// Store the whole collection.
        /// </summary>
        /// <param name="collection">EntityCollection to store.</param>
        /// <param name="fullName">Complete filename.</param>
        /// <param name="encoding">Encoding of file.</param>
        public static void Store(EntityCollection collection, string fullName, Encoding encoding)
        {
            if (fullName.Length == 0)
            {
                throw new InvalidOperationException("Filename is empty.");
            }
            
            using (StreamWriter writer = new StreamWriter(fullName, false /*=append*/, encoding))
            {
                Store(collection, writer);

                //// implizit �ber using: writer.Close();
            }
        }

        /// <summary>
        /// Store the whole collection.
        /// </summary>
        /// <param name="collection">Collection to be stored.</param>
        /// <param name="writer">Textwriter to use for storing.</param>
        public static void Store(EntityCollection collection, TextWriter writer)
        {
            writer.WriteLine("## SimpleDB file");
            writer.Write(collection.Comment);
            for (int i = 0; i < collection.Count; i++)
            {
                WriteObject(collection[i], writer);
            }
        }


        #region -------- Internal Methods -----------------------------------
        #endregion


        #region -------- Private Methods ------------------------------------
        #endregion

        /// <summary>
        /// Write all attributes to the given writer. This procedure
        /// prepends dots and colons where appriopriate.
        /// </summary>
        /// <param name="obj">Object to write.</param>
        /// <param name="writer">Writer to write to.</param>
        private static void WriteObject(Entity obj, TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer), "Initialized TextWriter must be passed to this method.");
            }
            
            if (obj.Count > 0)
            {
                writer.Write(':');
                writer.WriteLine(obj.GetNthAttribute(0)?.Name);
                writer.WriteLine(obj.GetNthAttribute(0)?.Value);
                for (int i = 1; i < obj.Count; i++)
                {
                    writer.Write('.');
                    writer.WriteLine(obj.GetNthAttribute(i)?.Name);
                    writer.WriteLine(obj.GetNthAttribute(i)?.Value);
                }
            }
        }
    }
}
