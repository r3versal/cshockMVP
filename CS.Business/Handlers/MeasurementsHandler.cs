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
                m.hip = cdm.measurements.hip;
                m.centerLength = cdm.measurements.centerLength;
                m.fullLength = cdm.measurements.fullLength;
                m.shoulderSlope = cdm.measurements.shoulderSlope;
                m.strap = cdm.measurements.strap;
                m.bustDepth = cdm.measurements.bustDepth;
                m.bustSpan = cdm.measurements.bustSpan;
                m.sideLength = cdm.measurements.sideLength;
                m.backNeck = cdm.measurements.backNeck;
                m.shoulderLength = cdm.measurements.shoulderLength;
                m.acrossShoulder = cdm.measurements.acrossShoulder;
                m.acrossChest = cdm.measurements.acrossChest;
                m.acrossBack = cdm.measurements.acrossBack;
                m.bustArc = cdm.measurements.bustArc;
                m.backArc = cdm.measurements.backArc;
                m.waistArc = cdm.measurements.waistArc;
                m.dartPlacement = cdm.measurements.dartPlacement;
                m.abdomenArc = cdm.measurements.abdomenArc;
                m.hipArc = cdm.measurements.hipArc;
                m.crotchDepth = cdm.measurements.crotchDepth;
                m.hipDepth = cdm.measurements.hipDepth;
                m.sideHipDepth = cdm.measurements.sideHipDepth;
                m.waistToAnkle = cdm.measurements.waistToAnkle;
                m.crotchLength = cdm.measurements.crotchLength;
                m.upperThigh = cdm.measurements.upperThigh;
                m.knee = cdm.measurements.knee;
                m.calf = cdm.measurements.calf;
                m.ankle = cdm.measurements.ankle;
                m.overarmLength = cdm.measurements.overarmLength;
                m.underarmLength = cdm.measurements.underarmLength;
                m.toElbow = cdm.measurements.toElbow;
                m.bicep = cdm.measurements.bicep;
                m.elbow = cdm.measurements.elbow;
                m.wrist = cdm.measurements.wrist;
                m.aroundHand = cdm.measurements.aroundHand;
                m.crotch = cdm.measurements.crotch;
                m.inseam = cdm.measurements.inseam;
                m.outseam = cdm.measurements.outseam;
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
                        m.hip,
                        m.centerLength,
                        m.fullLength,
                        m.shoulderSlope,
                        m.strap,
                        m.bustDepth,
                        m.bustSpan,
                        m.sideLength,
                        m.backNeck,
                        m.shoulderLength,
                        m.acrossShoulder,
                        m.acrossChest,
                        m.acrossBack,
                        m.bustArc,
                        m.backArc,
                        m.waistArc,
                        m.dartPlacement,
                        m.abdomenArc,
                        m.hipArc,
                        m.crotchDepth,
                        m.hipDepth,
                        m.sideHipDepth,
                        m.waistToAnkle,
                        m.crotchLength,
                        m.upperThigh,
                        m.knee,
                        m.calf,
                        m.ankle,
                        m.overarmLength,
                        m.underarmLength,
                        m.toElbow,
                        m.bicep,
                        m.elbow,
                        m.wrist,
                        m.aroundHand,
                        m.crotch,
                        m.inseam,
                        m.outseam
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
                var mea = await conn.QueryAsync<Measurements>("MeasurementsUpdate", new
                {
                    m.UserId,
                    m.bust,
                    m.waist,
                    m.hip,
                    m.centerLength,
                    m.fullLength,
                    m.shoulderSlope,
                    m.strap,
                    m.bustDepth,
                    m.bustSpan,
                    m.sideLength,
                    m.backNeck,
                    m.shoulderLength,
                    m.acrossShoulder,
                    m.acrossChest,
                    m.acrossBack,
                    m.bustArc,
                    m.backArc,
                    m.waistArc,
                    m.dartPlacement,
                    m.abdomenArc,
                    m.hipArc,
                    m.crotchDepth,
                    m.hipDepth,
                    m.sideHipDepth,
                    m.waistToAnkle,
                    m.crotchLength,
                    m.upperThigh,
                    m.knee,
                    m.calf,
                    m.ankle,
                    m.overarmLength,
                    m.underarmLength,
                    m.toElbow,
                    m.bicep,
                    m.elbow,
                    m.wrist,
                    m.aroundHand,
                    m.crotch,
                    m.inseam,
                    m.outseam
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