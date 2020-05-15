using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace StackSize
{
    public class PawnColumnWorker_EditSkills : PawnColumnWorker
    {
        //private const int LabelRowHeight = 50;

        private Vector2 cachedWorkLabelSize;

        public PawnColumnDef_EditSkills CastDef
        {
            get
            {
                return (PawnColumnDef_EditSkills)def;
            }
        }

        public PawnColumnWorker_EditSkills() : base()
        {
        }



        private static void DrawWorkBoxBackground(Rect rect, Pawn p, SkillDef skillDef)
        {
            float num = p.skills.GetSkill(skillDef).Level;
            Texture2D image;
            Texture2D image2;
            float a;
            if (num < 4f)
            {
                image = WidgetsWork.WorkBoxBGTex_Awful;
                image2 = WidgetsWork.WorkBoxBGTex_Bad;
                a = num / 4f;
            }
            else if (num <= 14f)
            {
                image = WidgetsWork.WorkBoxBGTex_Bad;
                image2 = WidgetsWork.WorkBoxBGTex_Mid;
                a = (num - 4f) / 10f;
            }
            else
            {
                image = WidgetsWork.WorkBoxBGTex_Mid;
                image2 = WidgetsWork.WorkBoxBGTex_Excellent;
                a = (num - 14f) / 6f;
            }
            GUI.DrawTexture(rect, image);
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
            GUI.DrawTexture(rect, image2);
            //if (workDef.relevantSkills.Any() && num <= 2f && p.workSettings.WorkIsActive(workDef))
            //{
            //    GUI.color = Color.white;
            //    GUI.DrawTexture(rect.ContractedBy(-2f), WorkBoxOverlay_Warning);
            //}
            Passion passion = p.skills.GetSkill(skillDef).passion;
            if ((int)passion > 0)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.4f);
                Rect position = rect;
                position.xMin = rect.center.x;
                position.yMin = rect.center.y;
                switch (passion)
                {
                    case Passion.Minor:
                        GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMinorIcon);
                        break;
                    case Passion.Major:
                        GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMajorIcon);
                        break;
                }
            }
            GUI.color = Color.white;
        }


        public static void DrawSkillBoxFor(float x, float y, Pawn p, SkillDef skillDef, bool incapableBecauseOfCapacities)
        {
            Rect rect = new Rect(x, y, 50f, 25f);
            if (incapableBecauseOfCapacities)
            {
                GUI.color = new Color(1f, 0.3f, 0.3f);
            }
            DrawWorkBoxBackground(rect, p, skillDef);
            GUI.color = Color.white;
            int skillLevel = p.skills.GetSkill(skillDef).Level;
            if (skillLevel > 0)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = WidgetsWork.ColorOfPriority((int)(((float)skillLevel) / 5.0f));
                Widgets.Label(rect.ContractedBy(-3f), skillLevel.ToStringCached());
                GUI.color = Color.white;
                Text.Anchor = TextAnchor.UpperLeft;
            }
            if (Event.current.type != 0 || !Mouse.IsOver(rect))
            {
                return;
            }
            if (Event.current.button == 0)
            {
                p.skills.GetSkill(skillDef).Level++;
            }
            if (Event.current.button == 1)
            {
                p.skills.GetSkill(skillDef).Level--;
            }
            Event.current.Use();
        }

        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            if (!pawn.Dead)
            {
                Text.Font = GameFont.Medium;
                float x = rect.x + (rect.width - 50f) / 2f;
                float y = rect.y + 2.5f;
                bool incapable = false; // IsIncapableOfWholeWorkType(pawn, def.workType);
                DrawSkillBoxFor(x, y, pawn, CastDef.skillDef, incapable);
                Rect rect2 = new Rect(x, y, 50f, 25f);
                if (Mouse.IsOver(rect2))
                {
                    // TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, def.workType, incapable), pawn.thingIDNumber ^ def.workType.GetHashCode());
                }
                Text.Font = GameFont.Small;
            }
        }

        public override void DoHeader(Rect rect, PawnTable table)
        {
            base.DoHeader(rect, table);
            Text.Font = GameFont.Small;
            if (cachedWorkLabelSize == default)
            {
                cachedWorkLabelSize = Text.CalcSize(CastDef.skillDef.defName);
            }
            Rect labelRect = GetLabelRect(rect);
            MouseoverSounds.DoRegion(labelRect);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(labelRect, CastDef.skillDef.defName);
            GUI.color = new Color(1f, 1f, 1f, 0.3f);
            Widgets.DrawLineVertical(labelRect.center.x, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
            Widgets.DrawLineVertical(labelRect.center.x + 1f, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public override int GetMinHeaderHeight(PawnTable table)
        {
            return 50;
        }

        public override int GetMinWidth(PawnTable table)
        {
            return Mathf.Max(base.GetMinWidth(table), 32);
        }

        public override int GetOptimalWidth(PawnTable table)
        {
            return Mathf.Clamp(39, GetMinWidth(table), GetMaxWidth(table));
        }

        public override int GetMaxWidth(PawnTable table)
        {
            return Mathf.Min(base.GetMaxWidth(table), 80);
        }

        //private bool IsIncapableOfWholeWorkType(Pawn p, WorkTypeDef work)
        //{
        //    for (int i = 0; i < work.workGiversByPriority.Count; i++)
        //    {
        //        bool flag = true;
        //        for (int j = 0; j < work.workGiversByPriority[i].requiredCapacities.Count; j++)
        //        {
        //            PawnCapacityDef capacity = work.workGiversByPriority[i].requiredCapacities[j];
        //            if (!p.health.capacities.CapableOf(capacity))
        //            {
        //                flag = false;
        //                break;
        //            }
        //        }
        //        if (flag)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
        {
            return GetLabelRect(headerRect);
        }

        public override int Compare(Pawn a, Pawn b)
        {
            return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
        }

        private float GetValueToCompare(Pawn pawn)
        {
            return pawn.skills.GetSkill(CastDef.skillDef).Level;
        }

        private Rect GetLabelRect(Rect headerRect)
        {
            float x = headerRect.center.x;
            Rect result = new Rect(x - cachedWorkLabelSize.x / 2f, headerRect.y, cachedWorkLabelSize.x, cachedWorkLabelSize.y);
            if (def.moveWorkTypeLabelDown)
            {
                result.y += 20f;
            }
            return result;
        }

        //protected override string GetHeaderTip(PawnTable table)
        //{
        //    string str = def.workType.gerundLabel.CapitalizeFirst() + "\n\n" + def.workType.description + "\n\n" + SpecificWorkListString(def.workType);
        //    str += "\n";
        //    if (def.sortable)
        //    {
        //        str += "\n" + "ClickToSortByThisColumn".Translate();
        //    }
        //    if (Find.PlaySettings.useWorkPriorities)
        //    {
        //        return str + ("\n" + "WorkPriorityShiftClickTip".Translate());
        //    }
        //    return str + ("\n" + "WorkPriorityShiftClickEnableDisableTip".Translate());
        //}

        //private static string SpecificWorkListString(WorkTypeDef def)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    for (int i = 0; i < def.workGiversByPriority.Count; i++)
        //    {
        //        stringBuilder.Append(def.workGiversByPriority[i].LabelCap);
        //        if (def.workGiversByPriority[i].emergency)
        //        {
        //            stringBuilder.Append(" (" + "EmergencyWorkMarker".Translate() + ")");
        //        }
        //        if (i < def.workGiversByPriority.Count - 1)
        //        {
        //            stringBuilder.AppendLine();
        //        }
        //    }
        //    return stringBuilder.ToString();
        //}

        //protected override void HeaderClicked(Rect headerRect, PawnTable table)
        //{
        //    base.HeaderClicked(headerRect, table);
        //    if (!Event.current.shift)
        //    {
        //        return;
        //    }
        //    List<Pawn> pawnsListForReading = table.PawnsListForReading;
        //    for (int i = 0; i < pawnsListForReading.Count; i++)
        //    {
        //        Pawn pawn = pawnsListForReading[i];
        //        if (pawn.workSettings == null || !pawn.workSettings.EverWork || pawn.WorkTypeIsDisabled(def.workType))
        //        {
        //            continue;
        //        }
        //        if (Find.PlaySettings.useWorkPriorities)
        //        {
        //            int priority = pawn.workSettings.GetPriority(def.workType);
        //            if (Event.current.button == 0 && priority != 1)
        //            {
        //                int num = priority - 1;
        //                if (num < 0)
        //                {
        //                    num = 4;
        //                }
        //                pawn.workSettings.SetPriority(def.workType, num);
        //            }
        //            if (Event.current.button == 1 && priority != 0)
        //            {
        //                int num2 = priority + 1;
        //                if (num2 > 4)
        //                {
        //                    num2 = 0;
        //                }
        //                pawn.workSettings.SetPriority(def.workType, num2);
        //            }
        //        }
        //        else if (pawn.workSettings.GetPriority(def.workType) > 0)
        //        {
        //            if (Event.current.button == 1)
        //            {
        //                pawn.workSettings.SetPriority(def.workType, 0);
        //            }
        //        }
        //        else if (Event.current.button == 0)
        //        {
        //            pawn.workSettings.SetPriority(def.workType, 3);
        //        }
        //    }
        //    if (Find.PlaySettings.useWorkPriorities)
        //    {
        //        SoundDefOf.DragSlider.PlayOneShotOnCamera();
        //    }
        //    else if (Event.current.button == 0)
        //    {
        //        SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
        //    }
        //    else if (Event.current.button == 1)
        //    {
        //        SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
        //    }
        //}
    }
}
