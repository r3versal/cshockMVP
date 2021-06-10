using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CS.Business.Handlers
{
    using CS.Common.Models;
    using CS.Common.Utility;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    public class MeasurementsHandler
    {

        public static async Task<Measurements> InsertMeasurments(CustomerOrderDataModel cdm, Guid measurementsId, Guid userId)
        {
            if (cdm != null)
            {

                Measurements m = new Measurements();
                m.MeasurementsId = measurementsId;
                m.bust = cdm.measurements.bust;
                m.waist = cdm.measurements.waist;
                m.hips = cdm.measurements.hips;
                m.neck = cdm.measurements.neck;
                m.shoulder = cdm.measurements.shoulder;
                m.backWidth = cdm.measurements.backWidth;
                m.frontLength = cdm.measurements.frontLength;
                m.backLength = cdm.measurements.backLength;
                m.crotch = cdm.measurements.crotch;
                m.inseam = cdm.measurements.inseam;
                m.outseam = cdm.measurements.outseam;
                m.thigh = cdm.measurements.thigh;
                m.knee = cdm.measurements.knee;
                m.leg = cdm.measurements.leg;
                m.UserId = userId;

                //insert measurements
                using (var conn = Business.Database.Connection)
                {
                    var newMeasurements = await conn.QueryAsync<Measurements>("MeasurementsInsert", new
                    {
                        m.MeasurementsId,
                        m.UserId,
                        m.bust,
                        m.waist,
                        m.hips,
                        m.neck,
                        m.shoulder,
                        m.backWidth,
                        m.frontLength,
                        m.backLength,
                        m.crotch,
                        m.inseam,
                        m.outseam,
                        m.thigh,
                        m.knee,
                        m.leg
                },
                     commandType: CommandType.StoredProcedure);
                    if (newMeasurements.Count() > 0)
                    {
                        return newMeasurements.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<Measurements> GetMeasurements(Guid userId)
        {
            using (var conn = Business.Database.Connection)
            {
                var m = await conn.QueryAsync<Measurements>("GetMeasurementsByUserId", new
                {
                    userId
                },
                    commandType: CommandType.StoredProcedure);
                if (m.Count() > 0)
                {
                    return m.AsList()[0];
                }
                return null;
            }
        }

        public static async Task<Measurements> UpdateMeasurements(Measurements m)
        {
            using (var conn = Business.Database.Connection)
            {
                var mea = await conn.QueryAsync<Measurements>("UpdateMeasurements", new
                {
                    m.UserId,
                    m.bust,
                    m.waist,
                    m.hips,
                    m.neck,
                    m.shoulder,
                    m.backLength,
                    m.frontLength,
                    m.crotch,
                    m.inseam,
                    m.outseam,
                    m.thigh,
                    m.knee,
                    m.leg
                },
                    commandType: CommandType.StoredProcedure);
                if (mea.Count() > 0)
                {
                    return mea.AsList()[0];
                }
                return null;
            }
        }
    }
}