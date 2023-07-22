namespace MSimpleDB
{
    using System.Collections.Generic;

    /// <summary>
    /// Entity is an object that contains a multiset of attributes and corresponding values.
    /// </summary>
    public class Entity
    {
        #region -------- Notes ----------------------------------------------
        #endregion

        #region -------- Variables ------------------------------------------
        #endregion

        readonly List<Attribute> _attributes = new List<Attribute>();
        readonly List<EntityCollection> _container = new List<EntityCollection>();

        #region -------- Constructor ----------------------------------------
        #endregion

        /// <summary>
        /// Create new instance.
        /// </summary>
        public Entity()
        {
        }

        #region -------- Events ---------------------------------------------
        #endregion

        #region -------- Properties -----------------------------------------
        #endregion

        /// <summary>
        /// Returns number of attributes.
        /// </summary>
        public int Count
        {
            get { return _attributes.Count; }
        }

        /// <summary>
        /// Is this entity modified since last call to AcceptChanges.
        /// </summary>
        public bool HasChanges { get; private set; }

        /// <summary>
        /// Access attributes by name.
        /// </summary>
        /// <param name="name">Name for the value is retrieved.</param>
        /// <returns>Value of the object or <c>null</c>.</returns>
        public string? this[string name]
        {
            get { return GetAttribute(name); }
            set { SetAttribute(name, value ?? string.Empty); }
        }

        #region -------- Public Methods -------------------------------------
        #endregion

        /// <summary>
        /// Add a name value pair to an object.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="val">Value of the object for that name.</param>
        public void Add(string name, string val)
        {
            _attributes.Add(new Attribute(name, val));
            SetHasChanges(true);
        }

        /// <summary>
        /// Get a value by its attributename.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <returns>Value of the object or <c>null</c>.</returns>
        public string? GetAttribute(string name)
        {
            Attribute? a = _attributes.Find(attr => attr.Name == name);
            return a?.Value;
        }

        /// <summary>
        /// Set an attribute,value-pair. If a value already exists,
        /// it is overwritten. If it does not exist, nothing is done.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="val">Value of the object.</param>
        public void SetAttribute(string name, string val)
        {
            Attribute? a = _attributes.Find(attr => attr.Name == name);
            if (a != null)
            {
                SetHasChanges(!object.Equals(a.Value, val));
                a.Value = val;
            }
        }

        /// <summary>
        /// Set an attribute,value-pair. If a value already exists,
        /// it is overwritten. If it does not exist, nothing is done.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="val">Value of the object.</param>
        public void SetOrAddAttribute(string name, string val)
        {
            Attribute? a = _attributes.Find(attr => attr.Name == name);
            if (a != null)
            {
                SetHasChanges(!object.Equals(a.Value, val));
                a.Value = val;
            }
            else
            {
                _attributes.Add(new Attribute(name, val));
                SetHasChanges(true);
            }
        }

        #region // old version of GetAttribute
        ///// <summary>
        ///// Return both attribute and value of an object, which
        ///// are stored at position <c>n</c>.
        ///// </summary>
        ///// <param name="name">Attributename</param>
        ///// <param name="val">Value</param>
        ///// <param name="n">Index</param>
        //// public void GetNthAttribute(ref string name, ref string val, int n)
        //// {
        ////    if (n >= 0 && n < _attributes.Count)
        ////    {
        ////        name = _attributes[n].Name;
        ////        val = _attributes[n].Value;
        ////    }
        ////    else
        ////    {
        ////        name = null;
        ////        val = null;
        ////    }
        //// }
        #endregion

        /// <summary>
        /// Return attribute with name and value of an object, which
        /// are stored at position <c>n</c>.
        /// </summary>
        /// <param name="index">Index of attribute.</param>
        /// <returns>Nth attribute (name-value-pair).</returns>
        public Attribute? GetNthAttribute(int index)
        {
            Attribute? a = null;
            if (index >= 0 && index < _attributes.Count)
            {
                a = _attributes[index];
            }
            
            return a;
        }

        /// <summary>
        /// Return number of attribute,value-pairs.
        /// </summary>
        /// <returns>Number of attribute,value-pairs.</returns>
        public int Size()
        {
            return _attributes.Count;
        }

        /// <summary>
        /// Resets value of HasChanges.
        /// </summary>
        public void AcceptChanges()
        {
            HasChanges = false;
        }

        #region -------- Internal Methods -----------------------------------
        #endregion

        internal void AddedToCollection(EntityCollection entityCollection)
        {
            if (!_container.Contains(entityCollection))
            {
                _container.Add(entityCollection);
            }
        }

        internal void RemovedFromCollection(EntityCollection entityCollection)
        {
            _container.Remove(entityCollection);
        }

        #region -------- Private Methods ------------------------------------
        #endregion

        private void SetHasChanges(bool value)
        {
            if (!HasChanges && value)
            {
                HasChanges = true;
                foreach (EntityCollection collection in _container)
                {
                    collection.SetHasChanges(true);
                }
            }
        }
    }
}
