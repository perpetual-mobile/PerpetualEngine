using System;
using System.Collections.Generic;

namespace PerpetualEngine.Storage
{
    /// <summary>
    /// Special unit testing implementation to let EditGroup(string) return an MockedSimpleStorage object
    /// </summary>
    public partial class SimpleStorage
    {
        static SimpleStorage()
        {
            SimpleStorage.EditGroup = (name) => {
                return new MockedSimpleStorage(name);
            };
        }

        public void Clear()
        {
            MockedSimpleStorage.database.Clear();
        }
    }

    /// <summary>
    /// Does not persistent right now... only used for unit tests
    /// </summary>
    public class MockedSimpleStorage : SimpleStorage
    {
        public static Dictionary<string,string> database = new Dictionary<string,string>();

        public MockedSimpleStorage(string groupName) : base(groupName)
        {
        }

        /// <summary>
        /// Persists a value with given key.
        /// </summary>
        /// <param name="value">if value is null, the key will be deleted</param>
        override public void Put(string key, string value)
        {
            if (value == null) {
                Delete(key);
                return;
            }
            var id = Group + "_" + key;
            Console.WriteLine("saving " + value.Ellipsis(8) + " with id " + id);
            if (database.ContainsKey(id))
                database.Remove(id);

            database.Add(id, value);
        }

        /// <summary>
        /// Retrieves value with given key.
        /// </summary>
        /// <returns>null, if key can not be found</returns>
        override public string Get(string key)
        {
            try {
                var data = database[Group + "_" + key];
                Console.WriteLine("loading " + data.Ellipsis(8) + " from id " + Group + "_" + key);

                return data;
            } catch (KeyNotFoundException) {
                return null;
            }
        }

        /// <summary>
        /// Delete the specified key.
        /// </summary>
        override public void Delete(string key)
        {
            database.Remove(Group + "_" + key);
        }
    }
}