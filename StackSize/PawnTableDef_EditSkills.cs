using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace StackSize
{
    public class PawnTable_EditSkills : PawnTable_PlayerPawns
    {
        public PawnTable_EditSkills(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight): base(def, pawnsGetter, uiWidth, uiHeight)
        {
        }
    }

    public class PawnTableDef_EditSkills : PawnTableDef
    {
        public PawnTableDef_EditSkills(): base()
        {
            workerClass = typeof(PawnTable_EditSkills);
            columns = new List<PawnColumnDef>();
            columns.Add(PawnTableDefOf.Work.columns[0]);
            foreach ( SkillDef s in DefDatabase<SkillDef>.AllDefs)
            {
                columns.Add(new PawnColumnDef_EditSkills(s));
            }
        }
    }

    public class PawnColumnDef_EditSkills : PawnColumnDef
    {
        public SkillDef skillDef;

        public PawnColumnDef_EditSkills(SkillDef skillDef)
        {
            this.skillDef = skillDef;
            workerClass = typeof(PawnColumnWorker_EditSkills);
        }
    }
}
