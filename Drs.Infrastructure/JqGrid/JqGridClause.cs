using System;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;

namespace Drs.Infrastructure.JqGrid
{
    public static class JqGridClause
    {
        private const string JQGRID_DESC = "desc";
        private const string SQL_DESC = "descending";
        private const string JQGRID_ASC = "asc";

        private const string SQL_ASC = "ascending";
        private const string SQL_OPERATOR_AND = "and";
        private const string SQL_OPERATOR_BW = "bw";

        private const string SQL_OPERATOR_BW_SW = ".Contains(@{0})";
        public static string OrderBy(JqGridFilterModel opt)
        {

            if (opt.sord.Trim().ToLower() == JQGRID_DESC)
            {
                return string.Format("{0} {1}", opt.sidx, SQL_DESC);
            }

            return string.Format("{0} {1}", opt.sidx, SQL_ASC);
        }

        public static JqGridQueryWhere ExpresionToExec(JqGridMultipleFilterModel mulFilter)
        {

            int count = 0;
            var sQuery = string.Empty;
            var respQry = new JqGridQueryWhere();


            foreach (JqGridRulesModel rule in mulFilter.rules)
            {
                dynamic qRule = string.Format("{0}{1}", SafeName(rule.field), ConcatOp(rule.op, count));

                respQry.LstParams.Add(rule.data);
                if (count == 0)
                {
                    sQuery = qRule;
                }
                else
                {
                    sQuery = string.Format("{0} {1} {2}", sQuery, ConcatOp(mulFilter.groupOp), qRule);
                }

                count = count + 1;
            }

            respQry.Query = sQuery;
            return respQry;

        }

        private static string ConcatOp(string sField, int? iPosition = null)
        {

            switch (sField.ToLower())
            {
                case SQL_OPERATOR_AND:
                    return SQL_OPERATOR_AND;
                case SQL_OPERATOR_BW:
                    return string.Format(SQL_OPERATOR_BW_SW, iPosition);
            }

            throw new ArgumentException(ResShared.ERROR_NOT_VALID_OPERATOR);

        }

        private static string SafeName(string field)
        {
            //Como mejora se debe revisar si el campo pertenece a una propiedad de la clase
            return field;
        }
    }
}