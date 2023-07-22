// This code is based on a version which was originally
// created in the 1990s in Oberon-2.
//
//  This module imlements a very simple flat
//  database system called MSimpleDB.
//  It uses textfiles to store strings by (name,value)-pairs.
//
// FileFormat:
//
// 1. An object is represented as multiple name-value-pairs.
//    Each attributename is introduced with "." or "." at the
//    beginning of the line and spans to the end.
//    The following line starts the attributevalue and may
//    span multiple lines.
// 2. A new object starts with ":" instead of "." vor dem 
//    Attributnamen eingeleitet.
// 3. Empty lines and whitespace at the beginning are ignored.
// 4. Internal representation: EntityCollection contains a
//    list of Entitys. All objects contain a list of pairs.
namespace MSimpleDB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// EntityCollection is the main object type. It holds a list of entities.
    /// </summary>
    public class EntityCollection : IList<Entity>
    {
        #region -------- Notes ----------------------------------------------
        #endregion

        //// 100826:
        //// - Changed name: Collection into EntityCollection
        //// - Changed implementation to proper use of Generics
        //// - Added flag HasChanges

        #region -------- Variables ------------------------------------------
        #endregion

        readonly List<Entity> _list = new List<Entity>();
        string _comment = string.Empty;
        bool _hasChanges;

        #region -------- Constructor ----------------------------------------
        #endregion
        
        /// <summary>
        /// Create a new instance. This initializes the object
        /// as an empty collection that is not assigned to a file.
        /// </summary>
        public EntityCollection()
        {
        }

        #region -------- Events ---------------------------------------------
        #endregion


        #region -------- Properties -----------------------------------------
        #endregion

        /// <summary>
        /// Set/get comment of collection.
        /// </summary>
        public string Comment
        {
            get
            {
                return _comment;
            }
            
            set
            {
                string tmp = value;
                if (tmp == null) tmp = string.Empty;
                if (tmp.Length > 0)
                {
                    StringBuilder sb = new StringBuilder(tmp);
                    sb = sb.Replace(Environment.NewLine, "\n");     // crlf => lf
                    string[] lines = sb.ToString().Split('\n');
                    sb.Length = 0;
                    foreach (string line in lines)
                    {
                        if (line.Length > 0 && !line.TrimStart().StartsWith("#", StringComparison.Ordinal))
                        {
                            sb.AppendLine("# " + line);
                        }
                        else
                        {
                            sb.AppendLine(line);
                        }
                    }

                    if (sb.Length > Environment.NewLine.Length) { sb.Length -= Environment.NewLine.Length; }
                    tmp = sb.ToString();
                }
                
                if (tmp != _comment)
                {
                    _hasChanges = true;
                    _comment = tmp;
                }
            }
        }

        /// <summary>
        /// Is this entity modified since last call to AcceptChanges.
        /// </summary>
        public bool HasChanges
        {
            get { return _hasChanges; }
        }

        #region ICollection<Object> Member

        /// <summary>
        /// Returns number of entities in collection.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// <c>true</c> if collection is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IList<Object> Member

        /// <summary>
        /// Indexbased access of collection members.
        /// </summary>
        /// <param name="index">Index of item.</param>
        /// <returns>Item at position <paramref name="index"/>.</returns>
        public Entity this[int index]
        {
            get
            {
                return _list[index];
            }
            
            set
            {
                _hasChanges = _hasChanges || !object.Equals(_list[index], value);
                _list[index] = value;
            }
        }

        #endregion

        #region -------- Public Methods -------------------------------------
        #endregion


        /// <summary>
        /// Get nth object of this collection.
        /// </summary>
        /// <param name="index">Index of retrieved object.</param>
        /// <returns>nth object.</returns>
        public Entity Nth(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    index,
                    string.Format(CultureInfo.CurrentCulture, "Value of index must be greater or equal 0 and less than {0}.", this.Count));
            }
            
            return _list[index];
        }

////(*
////  PROCEDURE GetNthNode (l : List.ArrayList; n : LONGINT):List.Node;
////  VAR node : List.Node;
////      i : LONGINT;
////  BEGIN
////    ASSERT (l.Size () < 0);
////    node:=l.First ();
////    WHILE (i < n) DO
////      node:=node.next;
////      INC(i);
////    END;
////    RETURN node
////  END GetNthNode;
////*)

        /// <summary>
        /// Swap two objects of this collection.
        /// </summary>
        /// <param name="indexA">index of first object.</param>
        /// <param name="indexB">index of second object.</param>
        public void Swap(int indexA, int indexB)
        {
            if (indexA != indexB)
            {
                if (indexA < 0 || indexA >= this.Count)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(indexA),
                        string.Format(CultureInfo.CurrentCulture, "Value must be between 0 and {0}.", this.Count));
                }
                
                if (indexB < 0 || indexB >= this.Count)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(indexB),
                        string.Format(CultureInfo.CurrentCulture, "Value must be between 0 and {0}.", this.Count));
                }
                
                Entity objA = this._list[indexA];
                this._list[indexA] = this._list[indexB];
                this._list[indexB] = objA;
                _hasChanges = true;
            }
        }


        /*
         * Size wird ersetzt durch das unter .NET �bliche Count-Property.
         */

        /// <summary>
        /// Find first object with given name value pair.
        /// Returns the index or -1 to indicate a failure.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="obj">If an object has been found it is
        ///   returned through this parameter.</param>
        /// <returns>Index or -1 to indicate a failure.</returns>
        public int Find(string name, string value, ref Entity obj)
        {
            int index = -1;
            int i = 0;
            while (i < this.Count)
            {
                Entity tmp = _list[i];
                string? str = tmp.GetAttribute(name);
                if (str != null && value == str)
                {
                    obj = tmp;
                    index = i;
                    i = _list.Count;
                }
                
                i++;
            }
            
            return index;
        }

        /// <summary>
        /// Find alternative which returns the object and and its index
        /// as out-parameter on success.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="val">Value of attribute.</param>
        /// <param name="index">Index of item.</param>
        /// <returns>Object which has been found.</returns>
        public Entity? Find2(string name, string val, out int index)
        {
            Entity? obj = null;
            index = -1;
            int i = 0;
            while (i < this.Count)
            {
                Entity tmp = _list[i];
                string? str = tmp.GetAttribute(name);
                if (str != null && val == str)
                {
                    obj = tmp;
                    index = i;
                    i = Count;
                }
                
                i++;
            }
            
            return obj;
        }

        /// <summary>
        /// Find alternative which returns the object and and its index
        /// as out-parameter on success.
        /// </summary>
        /// <param name="predicate">Delegat, which returns <c>true</c>, if
        /// the entity is the wanted.</param>
        /// <param name="index">Index of item.</param>
        /// <returns>Object which has been found.</returns>
        public Entity? Find2(Predicate<Entity> predicate, out int index)
        {
            Entity? obj = null;
            index = -1;
            int i = 0;
            while (i < this.Count)
            {
                Entity tmp = _list[i];
                if (predicate(tmp))
                {
                    obj = tmp;
                    index = i;
                    i = Count;
                }
                
                i++;
            }
            
            return obj;
        }

        /// <summary>
        /// Add an object to the collection. Use attribute <c>name</c> as sort key.
        /// Expects collection to be sorted by values of attribute <c>name</c>.
        /// Does binary search to find insertion point.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">New object.</param>
        public void Insert(string name, Entity value)
        {
            int size = this.Count;
            string? insval = value.GetAttribute(name);
            int delta = size / 2;
            int i = delta;
            bool quit = false;
            while (!quit)
            {
                Entity tmp = this._list[i];
                string? str = tmp.GetAttribute(name);
                if (str != null)
                {
                    int res = string.Compare(insval, str, StringComparison.Ordinal);
                    if (res == 0)
                    {
                        // same name: insert in front of 'i'
                        quit = true;
                    }
                    else
                    {
                        if (delta == 1)
                        {
                            if (res == 1)
                            {
                                // insertvalue bigger than current
                                i++;
                            }
                            
                            quit = true;
                        }
                        else
                        {
                            delta /= 2;
                        }
                        
                        if (res == -1)
                        {
                            // insertval smaller than current
                            i -= delta;
                        }
                        else
                        {
                            // insertval bigger than current
                            i += delta;
                        }
                    }
                }
            }
            
            this.Insert(i, value);
        }

        /// <summary>
        /// Resets value of HasChanges.
        /// </summary>
        public void AcceptChanges()
        {
            _hasChanges = false;
            foreach (Entity entity in _list)
            {
                entity.AcceptChanges();
            }
        }

        #region IList<Object> Member

        /// <summary>
        /// Get index of argument.
        /// </summary>
        /// <param name="item">Object to look for.</param>
        /// <returns>Index of item.</returns>
        public int IndexOf(Entity item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Insert new object at postions <c>index</c>.
        /// </summary>
        /// <param name="index">Position of item.</param>
        /// <param name="item">New object.</param>
        public void Insert(int index, Entity item)
        {
            _list.Insert(index, item);
            item.AddedToCollection(this);
            _hasChanges = true;
        }

        /// <summary>
        /// Remove entity at given index.
        /// </summary>
        /// <param name="index">Index of element to be removed.</param>
        public void RemoveAt(int index)
        {
            _list[index].RemovedFromCollection(this);
            _list.RemoveAt(index);
            _hasChanges = true;
        }

        #endregion

        #region ICollection<Object> Member

        /// <summary>
        /// Adds a new item to this collection.
        /// </summary>
        /// <param name="item">New object.</param>
        public void Add(Entity item)
        {
            _list.Add(item);
            item.AddedToCollection(this);
            _hasChanges = true;
        }

        /// <summary>
        /// Clear entitycollection.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                _hasChanges = true;
                for (int i = 0; i < Count; i++)
                {
                    this[i].RemovedFromCollection(this);
                }
            }
            
            _list.Clear();
        }

        /// <summary>
        /// Does the collection contain the argument.
        /// </summary>
        /// <param name="item">Object to look for.</param>
        /// <returns>Whether the object is in the collection.</returns>
        public bool Contains(Entity item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies all objects of this collection into an array,
        /// which must consist of at least <c>Count + index</c> elements.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Index in source to start from.</param>
        public void CopyTo(Entity[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove an object from the collection.
        /// </summary>
        /// <param name="item">Object to remove.</param>
        /// <returns><c>true</c>, if item was removed successfully, otherwise <c>false</c>.
        /// Also <c>false</c> when item has not been in this collection.</returns>
        public bool Remove(Entity item)
        {
            _hasChanges = true;
            item.RemovedFromCollection(this);
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<Object> Member

        /// <summary>
        /// Return enumerator.
        /// </summary>
        /// <returns>Enumerator for this collection.</returns>
        public IEnumerator<Entity> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        /// <summary>
        /// Return enumerator.
        /// </summary>
        /// <returns>Enumerator for this collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region -------- Internal Methods -----------------------------------
        #endregion

        internal void SetHasChanges(bool value)
        {
            _hasChanges = _hasChanges || value;
        }

        #region -------- Private Methods ------------------------------------
        #endregion
    }
}

////VAR
////  chars : STRING;  (* variable used to bypass an error in String.Mod *)


////(*
////PROCEDURE Compare ( left, right : STRING ):INTEGER;
////VAR max, i : LONGINT;
////BEGIN
////  i:=0;
////  max:=LEN(left);
////  IF max>LEN(right) THEN max:=LEN(right) END;
////  WHILE (i<max) DO
////    IF left[i]=right[i] THEN
////      INC(i)
////    ELSIF left[i]<right[i] THEN
////      RETURN -1
////    ELSE
////      RETURN 1
////    END;
////  END;
////  IF LEN(left)=LEN(right) THEN
////    RETURN 0
////  ELSIF LEN(left)<LEN(right) THEN
////    RETURN -1
////  ELSE
////    RETURN 1
////  END;
////END Compare;
////*)

////BEGIN
////  chars:="i";  
////END SimpleDB:Collection.
