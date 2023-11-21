﻿global using BinaryPlate.Application.Common.Behaviours;
global using BinaryPlate.Application.Common.Contracts.Application;
global using BinaryPlate.Application.Common.Contracts.Infrastructure;
global using BinaryPlate.Application.Common.Contracts.Infrastructure.DateAndTime;
global using BinaryPlate.Application.Common.Contracts.Infrastructure.Identity;
global using BinaryPlate.Application.Common.Contracts.Infrastructure.Persistence;
global using BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;
global using BinaryPlate.Application.Common.Contracts.Infrastructure.Storage;
global using BinaryPlate.Application.Common.Exceptions;
global using BinaryPlate.Application.Common.Extensions;
global using BinaryPlate.Application.Common.Helpers;
global using BinaryPlate.Application.Common.Managers;
global using BinaryPlate.Application.Common.Models;
global using BinaryPlate.Application.Common.Models.ApplicationOptions;
global using BinaryPlate.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;
global using BinaryPlate.Application.Common.Models.Identity;
global using BinaryPlate.Application.Features.Account.Commands.Login;
global using BinaryPlate.Application.Features.Account.Manage.Commands.ChangeEmail;
global using BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;
global using BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;
global using BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetTokenSettings;
global using BinaryPlate.Application.Features.Identity.Permissions.Queries.GetPermissions;
global using BinaryPlate.Application.Features.POC.Applicants.Commands.CreateApplicant;
global using BinaryPlate.Application.Features.POC.Applicants.Queries.ExportApplicants;
global using BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicants;
global using BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicantsReferences;
global using BinaryPlate.AppResources;
global using BinaryPlate.Domain.Common.Contracts;
global using BinaryPlate.Domain.Entities;
global using BinaryPlate.Domain.Entities.Identity;
global using BinaryPlate.Domain.Entities.POC;
global using BinaryPlate.Domain.Entities.Settings;
global using BinaryPlate.Domain.Entities.Settings.IdentitySettings;
global using BinaryPlate.Domain.Enums;
global using FluentValidation;
global using FluentValidation.Results;
global using MediatR;
global using MediatR.NotificationPublishers;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Twitter;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using System;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Net;
global using System.Reflection;
global using System.Security.Authentication;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Transactions;
global using System.Web;
global using BinaryPlate.Application.Common.Models.ApplicationMailSenderOptions;