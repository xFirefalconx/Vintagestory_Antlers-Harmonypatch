using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace AntlersHarmonypatch;

/// <summary>
/// Applies the Harmony patch that moves antler shedding from entity loading to the regular game tick.
/// </summary>
public class AntlersHarmonypatchModSystem : ModSystem
{
    private Harmony? harmonyInstance;

    /// <summary>
    /// Restricts this mod system to the server, because antler shedding is only evaluated there.
    /// </summary>
    /// <param name="side">The side that asks whether this mod system should be loaded</param>
    /// <returns>True on the server side, false on the client side</returns>
    public override bool ShouldLoad(EnumAppSide side)
    {
        return side == EnumAppSide.Server;
    }

    /// <summary>
    /// Creates the Harmony instance and applies all patches contained in this mod.
    /// </summary>
    /// <param name="serverApi">The server API of the currently loaded world</param>
    public override void StartServerSide(ICoreServerAPI serverApi)
    {
        harmonyInstance = new Harmony(Mod.Info.ModID);
        harmonyInstance.PatchAll(typeof(AntlersHarmonypatchModSystem).Assembly);

        serverApi.Logger.Notification("[{0}] Antler shedding while loading a creature is now deferred to the regular game tick.", Mod.Info.ModID);
    }

    /// <summary>
    /// Removes all patches of this mod so they do not stay active after the world has been closed.
    /// </summary>
    public override void Dispose()
    {
        harmonyInstance?.UnpatchAll(harmonyInstance.Id);
        harmonyInstance = null;
    }

}
