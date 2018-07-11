#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MyObserverWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MyObserver);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NotifyObserver", _m_NotifyObserver);
			
			
			
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "NotifyMethod", _s_set_NotifyMethod);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<System.Action<UnityEngine.Vector3>>(L, 2) && translator.Assignable<object>(L, 3))
				{
					System.Action<UnityEngine.Vector3> _notifyMethod = translator.GetDelegate<System.Action<UnityEngine.Vector3>>(L, 2);
					object _notifyContext = translator.GetObject(L, 3, typeof(object));
					
					MyObserver gen_ret = new MyObserver(_notifyMethod, _notifyContext);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MyObserver constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NotifyObserver(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MyObserver gen_to_be_invoked = (MyObserver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    INotification _notification = (INotification)translator.GetObject(L, 2, typeof(INotification));
                    
                    gen_to_be_invoked.NotifyObserver( _notification );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NotifyMethod(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MyObserver gen_to_be_invoked = (MyObserver)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.NotifyMethod = translator.GetDelegate<System.Action<UnityEngine.Vector3>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
