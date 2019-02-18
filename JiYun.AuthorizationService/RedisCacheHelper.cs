using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace JiYun.AuthorizationService
{
    public class RedisHelper
    {
        public static RedisHelper instance = new RedisHelper();

        //redis连接key
        private static string ConnectionString = ConfigurationManager.AppSettings["redis"];

        //声明一个委托，用于移除缓存后的事件处理
        public delegate void RedisCancheCallbackEvent();

        private ConnectionMultiplexer _connection = null;

        private RedisHelper()
        {
           
        }
        /// <summary>
        /// 初始化Redis连接
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer RedisConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;
            if (_connection != null)
            {
                _connection.Dispose();
            }
            try
            {
                _connection = ConnectionMultiplexer.Connect(ConnectionString);
            }
            catch (Exception ex)
            {
                
            }
            return _connection;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        private IDatabase Database(int dbNum = -1, object asyncState = null)
        {
            return RedisConnection().GetDatabase(dbNum, asyncState);
        } 

        #region String

        public bool StringSet<T>(string key, T values, int dbNum = -1, object asyncState = null, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringSet(key, Newtonsoft.Json.JsonConvert.SerializeObject(values), expiry, when, flags);
            }
            catch { }
            return false;

        }

        public T StringGet<T>(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                string result = this.Database(dbNum, asyncState).StringGet(key, flags);
                if (string.IsNullOrEmpty(result))
                    return default(T);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex) {

            }
            return default(T);

        }

        /// <summary>
        /// 设置 String
        /// </summary>
        /// <typeparam name="String"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <param name="db"></param>
        /// <param name="asyncState"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool StringSet(KeyValuePair<string, string>[] values, int dbNum = -1, object asyncState = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                List<KeyValuePair<RedisKey, RedisValue>> lvalues = new List<KeyValuePair<RedisKey, RedisValue>>();
                foreach (KeyValuePair<string, string> item in values)
                {
                    lvalues.Add(new KeyValuePair<RedisKey, RedisValue>(item.Key, item.Value));
                }
                if (lvalues.Count() > 0)
                    return this.Database(dbNum, asyncState).StringSet(lvalues.ToArray(), when, flags);
                return false;
            }
            catch (Exception ex) { }

            return false;

        }

        /// <summary>
        /// 设置 String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="asyncState"></param>
        /// <param name="expiry"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool StringSet(string key, string value, int dbNum = -1, object asyncState = null, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringSet(key, value, expiry, when, flags);
            }
            catch { }

            return false;
        }


        /// <summary>
        /// 获取 String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="asyncState"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public string StringGet(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringGet(key, flags);
            }
            catch { }
            return string.Empty;
        }

        /// <summary>
        /// 获取 String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="db"></param>
        /// <param name="asyncState"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public List<string> StringGet(string[] keys, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            List<string> result = new List<string>();
            try
            {
                RedisKey[] rks = new RedisKey[keys.Length];
                for (int i = 0; i < keys.Length; i++)
                {
                    rks[i] = keys[i];
                }
                var redisValue = this.Database(dbNum, asyncState).StringGet(rks, flags);

                foreach (var value in redisValue)
                {
                    if (value.HasValue)
                        result.Add(value);
                }
            }
            catch { }
            return result;
        }

        public string StringGetRange(string key, long start, long end, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringGetRange(key, start, end, flags);
            }
            catch { }

            return string.Empty;
        }

        public long StringLength(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringLength(key, flags);
            }
            catch { }

            return long.MinValue;
        }

        public string StringSetRange(string key, long offset, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringSetRange(key, offset, value, flags);
            }
            catch { }
            return string.Empty;
        }

        public long StringAppend(string key, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).StringAppend(key, value, flags);
            }
            catch { }
            return long.MinValue;
        }


        #endregion

        #region Key
        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public bool DeleteKey(string key, int dbNum = -1, object asyncState = null)
        {
            try
            {
                if (!this.Database(dbNum, asyncState).KeyExists(key)) return true;
                return this.Database(dbNum, asyncState).KeyDelete(key);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callbackEvent"></param>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public bool DeleteKey(string key, RedisCancheCallbackEvent callbackEvent, int dbNum = -1, object asyncState = null)
        {
            try
            {
                bool result = this.DeleteKey(key, dbNum, asyncState);
                callbackEvent();
                return result;
            }
            catch { }
            return false;

            
        }

        /// <summary>
        /// 检查健是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public bool ExistsKey(string key, int dbNum = -1, object asyncState = null)
        {
            try
            {
                return this.Database(dbNum, asyncState).KeyExists(key);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 设置健过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, int dbNum = -1, object asyncState = null, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None) {
            try
            {
                return this.Database(dbNum, asyncState).KeyExpire(key, expiry, flags);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 设置健过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbNum"></param>
        /// <param name="asyncState"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, int dbNum = -1, object asyncState = null, DateTime? expiry = null, CommandFlags flags = CommandFlags.None) {
            try
            {
                return this.Database(dbNum, asyncState).KeyExpire(key, expiry, flags);
            }
            catch { }
            return false;
        }

        #endregion

        #region Hash 
        public bool HashSet(string key, string hashField, string value, int dbNum = -1, object asyncState = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashSet(key, hashField, value, when, flags);
            }
            catch { }
            return false;
        }

        public void HashSet(string key, List<HashEntry> hashFields, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                this.Database(dbNum, asyncState).HashSet(key, hashFields.ToArray(), flags);
            }
            catch { }
        }

        public long HashLength(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashLength(key);
            }
            catch { }

            return long.MinValue;
        }

        public string[] HashKeys(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            List<string> result = new List<string>();
            try
            {
                var redisValue = this.Database(dbNum, asyncState).HashKeys(key);
                foreach (var item in redisValue)
                {
                    if (item.HasValue)
                        result.Add(item);
                }
            }
            catch { }
     
            return result.ToArray();
        }

        public HashEntry[] HashGetAll(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashGetAll(key, flags);
            }
            catch
            { }
            return null;
        }

        public T HashGet<T>(string key, string hashField, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var result = this.Database(dbNum, asyncState).HashGet(key, hashField, flags);
                if (string.IsNullOrEmpty(result))
                    return default(T);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }

        public string HashGet(string key, string hashField, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var result = this.Database(dbNum, asyncState).HashGet(key, hashField, flags);
                return result;
            }
            catch { }
            return string.Empty;
    
        }

        public RedisValue[] HashGet(string key, List<RedisValue> hashFields, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashGet(key, hashFields.ToArray(), flags);
            }
            catch { }
            return null;
        }

        public List<T> HashGet<T>(string key, List<RedisValue> hashFields, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            List<T> result = new List<T>();
            try
            {
                var redisvalues = this.Database(dbNum, asyncState).HashGet(key, hashFields.ToArray(), flags);
                foreach (var item in redisvalues)
                {
                    if (item.HasValue)
                        result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.ToString()));
                }
            }
            catch { }
            return result;
        }

        public bool HashDelete(string key, string hashField, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashDelete(key, hashField, flags);
            }
            catch { }
            return false;
        }

        public long HashDelete(string key, List<RedisValue> hashFields, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashDelete(key, hashFields.ToArray(), flags);
            }
            catch { }
            return long.MinValue;
        }

        public bool HashExists(string key, string hashField, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashExists(key, hashField, flags);
            }
            catch
            { }
            return false;
        }

        public RedisValue[] HashValues(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashValues(key, flags);
            }
            catch { }

            return null;
        }

        public IEnumerable<HashEntry> HashScan(string key, string pattern, int pageSize, CommandFlags flags, int dbNum = -1, object asyncState = null)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashScan(key, pattern, pageSize, flags);
            }
            catch { }
            return null;
        }

        public IEnumerable<HashEntry> HashScan(string key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).HashScan(key, pattern, pageSize, cursor, pageOffset, flags);
            }
            catch { }
            return null;
        }

        #endregion

        #region Redis List 

        public RedisValue ListGetByIndex(string key, long index, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {

            try
            {
                return this.Database(dbNum, asyncState).ListGetByIndex(key, index, flags);
            }
            catch { }
            return RedisValue.Null;
            
        }

        public long ListInsertAfter(string key, string pivot, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListInsertAfter(key, pivot, value, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long ListInsertBefore(string key, string pivot, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListInsertBefore(key, pivot, value, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue ListLeftPop(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListLeftPop(key, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public long ListLeftPush(string key, RedisValue[] values, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListLeftPush(key, values, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long ListLeftPush(string key, RedisValue value, When when = When.Always, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListLeftPush(key, value, when, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long ListLength(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListLength(key, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue[] ListRange(string key, long start = 0, long stop = -1, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListRange(key, start, stop, flags);
            }
            catch { }
            return null;
        }

        public long ListRemove(string key, string value, long count = 0, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListRemove(key, value, count, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue ListRightPop(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListRightPop(key, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public RedisValue ListRightPopLeftPush(string source, string destination, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListRightPopLeftPush(source, destination, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public long ListRightPush(string key, RedisValue[] values, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).ListRightPush(key, values, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long ListRightPush(string key, string value, When when = When.Always, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try {
                return this.Database(dbNum, asyncState).ListRightPush(key, value, when, flags);
            }
            catch { }
            return long.MinValue;

        }

        public void ListSetByIndex(string key, long index, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                this.Database(dbNum, asyncState).ListSetByIndex(key, index, value, flags);
            }
            catch { }
        }

        public void ListTrim(string key, long start, long stop, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try {
                this.Database(dbNum, asyncState).ListTrim(key, start, stop, flags);
            }
            catch { }
        }

        public bool LockExtend(string key, string value, TimeSpan expiry, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).LockExtend(key, value, expiry, flags);
            }
            catch { }
            return false;
        }

        public RedisValue LockQuery(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).LockQuery(key, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public bool LockRelease(string key, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).LockRelease(key, value, flags);
            }
            catch { }
            return false;
        }

        public bool LockTake(string key, string value, TimeSpan expiry, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).LockTake(key, value, expiry, flags);
            }
            catch { }
            return false;
        }

        #endregion

        #region Redis Set 


        public bool SetAdd(string key, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetAdd(key, value, flags);
            }
            catch { }
            return false;
        }

        public long SetAdd(string key, RedisValue[] values, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetAdd(key, values, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue[] SetCombine(SetOperation operation, RedisKey[] keys, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetCombine(operation, keys, flags);
            }
            catch { }
            return null;
        }

        public RedisValue[] SetCombine(SetOperation operation, string first, string second, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetCombine(operation, first, second, flags);
            }
            catch { }
            return null;
        }

        public long SetCombineAndStore(SetOperation operation, string destination, RedisKey[] keys, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetCombineAndStore(operation, destination, keys, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SetCombineAndStore(SetOperation operation, string destination, string first, string second, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetCombineAndStore(operation, destination, first, second, flags);
            }
            catch { }
            return long.MinValue;
        }

        public bool SetContains(string key, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetContains(key, value, flags);
            }
            catch { }
            return false;
        }

        public long SetLength(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetLength(key, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue[] SetMembers(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetMembers(key, flags);
            }
            catch { }
            return null;
        }

        public bool SetMove(string source, string destination, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetMove(source, destination, value, flags);
            }
            catch { }
            return false;
        }

        public RedisValue SetPop(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetPop(key, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public RedisValue SetRandomMember(string key, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetRandomMember(key, flags);
            }
            catch { }
            return RedisValue.Null;
        }

        public RedisValue[] SetRandomMembers(string key, long count, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetRandomMembers(key, count, flags);
            }
            catch { }
            return null;
        }

        public long SetRemove(string key, RedisValue[] values, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetRemove(key, values, flags);
            }
            catch { }
            return long.MinValue;
        }

        public bool SetRemove(string key, string value, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetRemove(key, value, flags);
            }
            catch { }
            return false;
        }

        public IEnumerable<RedisValue> SetScan(string key, string pattern, int pageSize, CommandFlags flags, int dbNum = -1, object asyncState = null)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetScan(key, pattern, pageSize, flags);
            }
            catch { }
            return null;
        }

        public IEnumerable<RedisValue> SetScan(string key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SetScan(key, pattern, pageSize, cursor, pageOffset, flags);
            }
            catch { }
            return null;
        }

        public RedisValue[] Sort(string key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).Sort(key, skip, take, order, sortType, by, get, flags);
            }
            catch { }
            return null;
        }

        public long SortAndStore(string destination, string key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SortedSetAdd(string key, SortedSetEntry[] values, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetAdd(key, values, flags);
            }
            catch { }
            return long.MinValue;
            
        }

        public bool SortedSetAdd(string key, string member, double score, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetAdd(key, member, score, flags);
            }
            catch { }
            return false;
        }

        public long SortedSetCombineAndStore(SetOperation operation, string destination, string first, string second, Aggregate aggregate = Aggregate.Sum, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {

            try
            {
                return this.Database(dbNum, asyncState).SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SortedSetCombineAndStore(SetOperation operation, string destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetLength(key, min, max, exclude, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SortedSetLengthByValue(string key, string min, string max, Exclude exclude = Exclude.None, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetLengthByValue(key, min, max, exclude, flags);
            }
            catch { }
            return long.MinValue;
        }

        public RedisValue[] SortedSetRangeByRank(string key, long start = 0, long stop = -1, Order order = Order.Ascending, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRangeByRank(key, start, stop, order, flags);
            }
            catch { }
            return null;
        }

        public SortedSetEntry[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, Order order = Order.Ascending, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRangeByRankWithScores(key, start, stop, order, flags);
            }
            catch { }
            return null;
        }

        public RedisValue[] SortedSetRangeByScore(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRangeByScore(key, start, stop, exclude, order, skip, take, flags);
            }
            catch { }
            return null;
        }

        public SortedSetEntry[] SortedSetRangeByScoreWithScores(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRangeByScoreWithScores(key, start, stop, exclude, order, skip, take, flags);
            }
            catch { }
            return null;
        }

        public RedisValue[] SortedSetRangeByValue(string key, RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRangeByValue(key, min, max, exclude, skip, take, flags);
            }
            catch { }
            return null;
        }

        public long? SortedSetRank(string key, string member, Order order = Order.Ascending, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRank(key, member, order, flags);
            }
            catch {
            }
            return long.MinValue;
        }

        public bool SortedSetRemove(string key, string member, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRemove(key, member, flags);
            }
            catch { }
            return false;
        }

        public long SortedSetRemove(string key, RedisValue[] members, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRemove(key, members, flags);
            }
            catch { }
            return long.MinValue;

        }

        public long SortedSetRemoveRangeByRank(string key, long start, long stop, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRemoveRangeByRank(key, start, stop, flags);
            }
            catch { }
            return long.MinValue;
            
        }

        public long SortedSetRemoveRangeByScore(string key, double start, double stop, Exclude exclude = Exclude.None, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);
            }
            catch { }
            return long.MinValue;
        }

        public long SortedSetRemoveRangeByValue(string key, string min, string max, Exclude exclude = Exclude.None, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetRemoveRangeByValue(key, min, max, exclude, flags);
            }
            catch { }
            return long.MinValue;
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(string key, string pattern, int pageSize, CommandFlags flags, int dbNum = -1, object asyncState = null)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetScan(key, pattern, pageSize, flags);
            }
            catch { }
            return null;
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(string key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetScan(key, pattern, pageSize, cursor, pageOffset, flags);
            }
            catch { }
            return null;
        }

        public double? SortedSetScore(string key, string member, int dbNum = -1, object asyncState = null, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return this.Database(dbNum, asyncState).SortedSetScore(key, member, flags);
            }
            catch { }
            return double.MinValue;
        }

        #endregion

    }
}
