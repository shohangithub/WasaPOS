using System;
using System.Collections;
using System.Web;

namespace Utility
{
    public class SessionManager
    {
        private static Hashtable _hshRepositry = new Hashtable();

        /// <summary>
        /// Description : Save session object in a hash table 
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="objValue"></param>
        public static void SaveSession(object objKey, object objValue)
        {
            try
            {
                object obj = HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                if (obj != null)
                {
                    _hshRepositry = (Hashtable)HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                    if (_hshRepositry.ContainsKey(objKey))
                    {
                        RemoveSession(objKey);

                    }
                    _hshRepositry.Add(objKey, objValue);
                    HttpContext.Current.Session.Add(GlobalConstant.SESSION_REPOSITORY, _hshRepositry);
                }
                else
                {
                    _hshRepositry = new Hashtable();
                    _hshRepositry.Add(objKey, objValue);
                    HttpContext.Current.Session.Add(GlobalConstant.SESSION_REPOSITORY, _hshRepositry);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Description : Get session object from hashtable
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        public static object GetSession(object objKey)
        {
            try
            {
                object obj = HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                if (obj != null)
                {
                    _hshRepositry = (Hashtable)HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                    return (_hshRepositry[objKey]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Description : check wheather session object already exist in hashtable or not 
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        public static bool IsItemExist(object objKey)
        {
            try
            {
                object obj = HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                if (obj != null)
                {
                    _hshRepositry = (Hashtable)HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                    return (_hshRepositry[objKey] != null);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Description : remove a particulat session variable from hashtable and session
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        public static bool RemoveSession(object objKey)
        {
            try
            {
                object obj = HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                if (obj != null)
                {
                    _hshRepositry = (Hashtable)HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                    _hshRepositry.Remove(objKey);
                    HttpContext.Current.Session.Add(GlobalConstant.SESSION_REPOSITORY, _hshRepositry);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Description : Clear all Session object from hashtable
        /// </summary>
        /// <returns></returns>
        public static bool RemoveAllSession()
        {
            try
            {
                object obj = HttpContext.Current.Session[GlobalConstant.SESSION_REPOSITORY];
                if (obj != null)
                {
                    HttpContext.Current.Session.RemoveAll();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
