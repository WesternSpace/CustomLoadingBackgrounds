using CustomScreenBackgrounds.GUI;
using CustomScreenBackgrounds.Utill;
using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace CustomScreenBackgrounds.Patches
{
    [HarmonyPatch(typeof(MyGuiScreenLoading), "DrawInternal")]
    internal class Patch_LoadingMenu
    {
        private static bool IsImageAleadyLoaded = false;
        private static string CustomOverlay = "";

        private static bool Prefix(float ___m_transitionAlpha, string ___m_customTextFromConstructor,
            MyGuiControlMultilineText ___m_multiTextControl, StringBuilder ___m_authorWithDash, string ___m_backgroundScreenTexture,
            ref MyGuiControlRotatingWheel ___m_wheel, float ___m_progress, MyGuiControlDlcBadgeWall ___m_badgeWall)
        {
            Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
            MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", fullscreenRectangle, Color.Black, false, true);
            MyGuiManager.GetSafeHeightFullScreenPictureSize(MyGuiConstants.LOADING_BACKGROUND_TEXTURE_REAL_SIZE, out Rectangle destinationRectangle);

            MyGuiManager.DrawSpriteBatch(___m_backgroundScreenTexture, destinationRectangle, new Color(new Vector4(1f, 1f, 1f, ___m_transitionAlpha)), true, true);

            if (Plugin.Instance.Config.LoadingScreenOverlay)
            {
                MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background_fade.dds", destinationRectangle, new Color(new Vector4(1f, 1f, 1f, ___m_transitionAlpha)), true, true);
            }

            if (Plugin.Instance.Config.CustomLoadingMenuOverlay && !IsImageAleadyLoaded)
            {
                if (Directory.GetFiles(FileSystem.LoadingMenuCustomOverlaysFolderPath, "*.png").Length == 0)
                {
                    if (Directory.GetFiles(FileSystem.LoadingMenuCustomOverlaysFolderPath, "*.dds").Length == 0)
                    {
                        CustomOverlay = "";
                    }
                    else
                    {
                        CustomOverlay = FileSystem.GetRandomFileFromDir(FileSystem.LoadingMenuCustomOverlaysFolderPath);
                        IsImageAleadyLoaded = true;
                    }
                }
                else
                {
                    CustomOverlay = FileSystem.GetRandomFileFromDir(FileSystem.LoadingMenuCustomOverlaysFolderPath);
                    IsImageAleadyLoaded = true;
                }
            }

            MyGuiManager.DrawSpriteBatch(CustomOverlay, destinationRectangle, new Color(new Vector4(1f, 1f, 1f, ___m_transitionAlpha)), true, true);

            if (Plugin.Instance.Config.CleanLoadingMenu)
            {
                ___m_wheel.Visible = false;
                ___m_badgeWall.Visible = false;

                if (Plugin.Instance.Config.ShowLoadingMenuPercent)
                {
                    MyGuiManager.DrawString("LoadingScreen", $"{Math.Round(___m_progress * 100)}%", new Vector2(0.94f, 0.96f), MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.1f, new Color(MyGuiConstants.LOADING_PLEASE_WAIT_COLOR * ___m_transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, true);
                }
            }
            else
            {
                MyGuiSandbox.DrawGameLogoHandler(___m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68), new Vector2(0.005f, 0.19f));
                MyGuiManager.DrawString("LoadingScreen", MyTexts.GetString(MyCommonTexts.LoadingPleaseWaitUppercase), MyGuiConstants.LOADING_PLEASE_WAIT_POSITION, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.1f, new Color(MyGuiConstants.LOADING_PLEASE_WAIT_COLOR * ___m_transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
                if (Plugin.Instance.Config.ShowLoadingMenuPercent)
                {
                    MyGuiManager.DrawString("LoadingScreen", $"{Math.Round(___m_progress * 100)}%", MyGuiConstants.LOADING_PERCENTAGE_POSITION, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.1f, new Color(MyGuiConstants.LOADING_PLEASE_WAIT_COLOR * ___m_transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
                }
                if (string.IsNullOrEmpty(___m_customTextFromConstructor))
                {
                    string font = "LoadingScreen";  
                    Vector2 positionAbsoluteBottomLeft = ___m_multiTextControl.GetPositionAbsoluteBottomLeft();
                    Vector2 textSize = ___m_multiTextControl.TextSize;
                    MyGuiManager.DrawString(normalizedCoord: positionAbsoluteBottomLeft + new Vector2((___m_multiTextControl.Size.X - textSize.X) * 0.5f + 0.025f, 0.025f), font: font, text: ___m_authorWithDash.ToString(), scale: MyGuiSandbox.GetDefaultTextScaleWithLanguage());
                }
                ___m_multiTextControl.Draw(1f, 1f);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(MyGuiScreenLoading), "GetRandomBackgroundTexture")]
    internal class Patch_LoadingMenuRandomImage
    {
        private static bool Prefix(ref string __result)
        {
            if (FileSystem.GetAllLoadingScreenFiles().Count() != 0)
            {
                __result = FileSystem.GetRandomFileFromDir(FileSystem.RootFolderPath);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(MyGuiScreenLoading), "RecreateControls")]
    internal class Patch_LoadingMenuRecreateControls
    {
        private static void Postfix(MyGuiScreenLoading __instance)
        {
            if (Plugin.Instance.Config.CleanLoadingMenu)
            {
                MyGuiControlRotatingWheel rotatingWheel = new MyGuiControlRotatingWheel(new Vector2(1.13f, 0.95f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.26f, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, manualRotationUpdate: false);
                __instance.Controls.Add(rotatingWheel);
            }
        }
    }
}
