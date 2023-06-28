using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AudioStreamerAPI.DAO
{
    public class MemberstatsDAO
    {
        private static MemberstatsDAO? _instance;
        private static readonly object _instanceLock = new();

        public static MemberstatsDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new();
                    return _instance;
                }
            }
        }

        public OperationalStatus GetStats(int trackId)
        {
            int totalViews = 0;
            int numberOfLikes = 0;

            try
            {
                var context = new fsnvdezgContext();
                totalViews = context.Memberstats.Where(stat => stat.TrackId == trackId).Sum(v => v.ViewCountsTotal);
                numberOfLikes = context.Memberstats.Where(stat => stat.TrackId == trackId).Sum(r => r.Rating);
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }

            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Successfully retrieved stats for track with id: {trackId}",
                Objects = new object[] { totalViews, numberOfLikes, },
            };
        }

        public Memberstat? GetStats(int userId, int trackId)
        {
            Memberstat? memberstat;
            try
            {
                var context = new fsnvdezgContext();
                memberstat = context.Memberstats.FirstOrDefault(stat => stat.MemberId == userId && stat.TrackId == trackId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return memberstat;
        }

        public OperationalStatus AddStats(Memberstat memberstat)
        {
            Memberstat? statHasId = GetStats(memberstat.MemberId, memberstat.TrackId);
            if (statHasId == null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Memberstats.Add(memberstat);
                    context.SaveChanges();

                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.Created,
                        Message = "Successfully created stats.",
                        Objects = new object[] { memberstat },
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Conflict,
                Message = "Stats already exist.",
            };
        }

        public OperationalStatus UpdateStats(Memberstat memberstat)
        {
            Memberstat? statHasId = GetStats(memberstat.MemberId, memberstat.TrackId);
            if (statHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Memberstats.Attach(statHasId);

                    statHasId.ViewCountsTotal++;
                    statHasId.Rating = memberstat.Rating;

                    if (memberstat.Tags!.Length == 0)
                    {
                        statHasId.Tags = Array.Empty<string>();
                    }
                    else
                    {
                        var newArrayLength = memberstat.Tags.Length;
                        statHasId.Tags = new string[newArrayLength];
                        memberstat.Tags.CopyTo(statHasId.Tags, 0);
                    }

                    context.SaveChanges();

                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.Ok,
                        Message = "Successfully updated stats.",
                        Objects = new object[] { statHasId },
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Stats not exist.",
            };
        }

        public OperationalStatus DeleteStats(int userId, int trackId)
        {
            Memberstat? statHasId = GetStats(userId, trackId);
            if (statHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Memberstats.Remove(statHasId);
                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.Ok,
                        Message = "Stats deleted.",
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Stats not found.",
            };
        }

        public OperationalStatus DeleteStatsOfUser(int userId)
        {
            try
            {
                var context = new fsnvdezgContext();
                //Be extra careful, this query will result in life or death of your project, Member =/= Memberstats
                //Kinda funny how I'm talking to myself in third person...
                //Schizo hours
                //Have you watched/played South Scrimshaw? It's a 1h30 long documentary of extraterrestrial life on another planet that is/would be similar to our earth
                //It's fiction, and a visual novel, which has no gameplay other than point and click
                //But the way they convey their ideas, from the behaviour and biology of certain mammals, how certain species co-exist with each other to providing additional meaning to certain context as to ensure that spectators won't be out of the loop on certain topics
                //How its ecosystem functions in comparison to ours makes the entire sitting quite engaging
                //Go check it out if you haven't yet
                //No this is not a promo, I just stumbled upon it thanks to a certain youtuber
                //Whether this query fails or not doesn't matter
                context.Database.ExecuteSqlInterpolated($"DELETE FROM Memberstats WHERE member_id = {userId};");
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = $"Deleted all stats of user with Id: {userId}.",
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
        }

        public OperationalStatus DeleteStatsOfTrack(int trackId)
        {
            try
            {
                var context = new fsnvdezgContext();
                context.Database.ExecuteSqlInterpolated($"DELETE FROM Memberstats WHERE track_id = {trackId};");
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = $"Deleted all stats of track with Id: {trackId}.",
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
        }
    }
}
