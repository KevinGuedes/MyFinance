using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Infra.Data.Repositories;
public class UserRepository(MyFinanceDbContext myFinanceDbContext) 
    : EntityRepository<User>(myFinanceDbContext), IUserRepository
{
}
