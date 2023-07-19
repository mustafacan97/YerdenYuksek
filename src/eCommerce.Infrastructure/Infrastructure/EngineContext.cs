using System.Runtime.CompilerServices;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives.Singleton;

namespace YerdenYuksek.Web.Framework.Infrastructure;

public class EngineContext
{
    #region Public Properties

    public static IEngine Current
    {
        get
        {
            if (Singleton<IEngine>.Instance == null)
            {
                Create();
            }

            return Singleton<IEngine>.Instance!;
        }
    }

    #endregion

    #region Public Methods

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static IEngine Create()
    {
        return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new Engine());
    }

    public static void Replace(IEngine engine)
    {
        Singleton<IEngine>.Instance = engine;
    }

    #endregion
}
