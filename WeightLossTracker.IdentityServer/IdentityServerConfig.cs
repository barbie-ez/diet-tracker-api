﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WeightLossTracker.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="WeightLossTrackerAuth",
                    ClientName="WeightLossTrackerAuth Client",
                    AllowedGrantTypes= new List<string>(){GrantType.Implicit},
                    RedirectUris={"http://localhost:44343/signin-odic"},
                    PostLogoutRedirectUris= new List<string>{"http://localhost:44343/signout-callback-odic"},
                    AllowedScopes= new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Numiii",
                    Password="test",
                    Claims= new[]
                    {
                        new Claim("name","Numiii")
                    }
                }
            };
        }
    }
}
