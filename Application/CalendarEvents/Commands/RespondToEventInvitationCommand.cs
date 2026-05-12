using CalendarAPI.Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarAPI.Application.CalendarEvents.Commands;
public sealed record RespondToEventInvitationCommand(
    Guid EventId, string Status)
    : ICommand;