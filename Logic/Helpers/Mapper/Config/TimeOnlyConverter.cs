using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Logic.Helpers.Mapper.Config
{
    public class TimeOnlyConverter : IValueConverter<TimeOnly, TimeSpan>, IValueConverter<TimeSpan, TimeOnly>
    {
        public TimeSpan Convert(TimeOnly sourceMember, ResolutionContext context)
            => sourceMember.ToTimeSpan();

        public TimeOnly Convert(TimeSpan sourceMember, ResolutionContext context)
            => TimeOnly.FromTimeSpan(sourceMember);
    }
}
