using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace StackSize
{
    public class MainTabWindow_EditSkills : MainTabWindow
    {
        //private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;

        //private static PawnTableDef Skills;

        protected PawnTableDef PawnTableDef => new PawnTableDef_EditSkills();

        protected float ExtraTopSpace => 40f;

        private PawnTable table;

        protected virtual float ExtraBottomSpace => 53f;

        protected override float Margin => 6f;

        public override Vector2 RequestedTabSize
        {
            get
            {
                if (table == null)
                {
                    return Vector2.zero;
                }
                return new Vector2(table.Size.x + Margin * 2f, table.Size.y + ExtraBottomSpace + ExtraTopSpace + Margin * 2f);
            }
        }

        protected virtual IEnumerable<Pawn> Pawns => Find.CurrentMap.mapPawns.FreeColonists;

        public void Notify_PawnsChanged()
        {
            SetDirty();
        }

        public override void Notify_ResolutionChanged()
        {
            table = CreateTable();
            base.Notify_ResolutionChanged();
        }

        private PawnTable CreateTable()
        {
            return (PawnTable)Activator.CreateInstance(PawnTableDef.workerClass, PawnTableDef, (Func<IEnumerable<Pawn>>)(() => Pawns), UI.screenWidth - (int)(Margin * 2f), (int)((float)(UI.screenHeight - 35) - ExtraBottomSpace - ExtraTopSpace - Margin * 2f));
        }

        protected void SetDirty()
        {
            if (table != null)
            {
                table.SetDirty();
            }
            SetInitialSizeAndPosition();
        }

        public override void PostOpen()
        {
            if (table == null)
            {
                table = CreateTable();
            }
            SetDirty();
            Find.World.renderer.wantedMode = WorldRenderMode.None;
        }

        public override void DoWindowContents(Rect rect)
        {
            base.DoWindowContents(rect);
            table.PawnTableOnGUI(new Vector2(rect.x, rect.y + ExtraTopSpace));
            //if (Event.current.type != EventType.Layout)
            //{
            //    DoManualPrioritiesCheckbox();
            //    GUI.color = new Color(1f, 1f, 1f, 0.5f);
            //    Text.Anchor = TextAnchor.UpperCenter;
            //    Text.Font = GameFont.Tiny;
            //    Widgets.Label(new Rect(370f, rect.y + 5f, 160f, 30f), "<= " + "HigherPriority".Translate());
            //    Widgets.Label(new Rect(630f, rect.y + 5f, 160f, 30f), "LowerPriority".Translate() + " =>");
            //    GUI.color = Color.white;
            //    Text.Font = GameFont.Small;
            //    Text.Anchor = TextAnchor.UpperLeft;
            //}
        }

        //private void DoManualPrioritiesCheckbox()
        //{
        //    Text.Font = GameFont.Small;
        //    GUI.color = Color.white;
        //    Text.Anchor = TextAnchor.UpperLeft;
        //    Rect rect = new Rect(5f, 5f, 140f, 30f);
        //    bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
        //    Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities);
        //    if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
        //    {
        //        foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_Alive)
        //        {
        //            if (item.Faction == Faction.OfPlayer && item.workSettings != null)
        //            {
        //                item.workSettings.Notify_UseWorkPrioritiesChanged();
        //            }
        //        }
        //    }
        //    if (Current.Game.playSettings.useWorkPriorities)
        //    {
        //        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        //        Text.Font = GameFont.Tiny;
        //        Widgets.Label(new Rect(rect.x, rect.y + rect.height + 4f, rect.width, 60f), "PriorityOneDoneFirst".Translate());
        //        Text.Font = GameFont.Small;
        //        GUI.color = Color.white;
        //    }
        //    if (!Current.Game.playSettings.useWorkPriorities)
        //    {
        //        UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
        //    }
        //}
    }
}
