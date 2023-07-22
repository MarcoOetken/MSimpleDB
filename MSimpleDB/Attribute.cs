namespace MSimpleDB
{
    /// <summary>
    /// An attribute class. Contains a single pair name, value.
    /// </summary>
    public class Attribute
    {
        #region -------- Constructor ----------------------------------------
        #endregion

        /// <summary>
        /// Create a new object with given name and value.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="val">Value of attribute.</param>
        public Attribute(string name, string val)
        {
            Name = name;
            Value = val;
        }

        #region -------- Events ---------------------------------------------
        #endregion

        #region -------- Properties -----------------------------------------
        #endregion

        /// <summary>
        /// Set and get name of attribute object.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Set and get value of attribute object.
        /// </summary>
        public string Value { get; internal set; }

        #region -------- Public Methods -------------------------------------
        #endregion

        #region -------- Internal Methods -----------------------------------
        #endregion

        #region -------- Private Methods ------------------------------------
        #endregion
    }
}
