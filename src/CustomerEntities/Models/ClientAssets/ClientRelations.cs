using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomerEntities.Models;
using InfraEntities.Interfaces;
using InfraEntities.ModelType;

namespace CustomerEntities;

public abstract class ClientRelations<TRelation, TAsset>
where TRelation : IHasPriorityOrder, IHasAsset<TAsset>
{
    /*
    private IList<TRelation> _relations;
    private Predicate<IHasPriorityOrder> primaryCheck ;

    public IReadOnlyCollection<IClientRelation<TAsset>> Relations 
    {
       get => _relations.AsReadOnly();
    }

    public  IClientRelation<TAsset, > 
    GetPrimary(Client client, Predicate<> precedentPredicate) 
    {
        var preferred =   _relations.FirstOrDefault(c => 
        c.ClientId == client.Id && c.IsType(precedentPredicate));
        return preferred;
    }

    
    public IReadOnlyCollection<IClientRelation<TAsset, >>
    GetSecondary(Client client, Predicate<> precedentPredicate) 
    {
        return  _relations.Where(c => c.ClientId == client.Id 
                    && c.IsType(precedentPredicate)).ToList().AsReadOnly();
    }
    public void SetAsPrimaryRelation(
            Client client, TAsset primaryInfo, 
            Predicate<> primaryCheck)
    {
        var existingPrimary = _relations?.FirstOrDefault(c => c.IsType(primaryCheck) && c.ClientId == client.Id );
        Type ty = _relations.GetType().GetGenericArguments()[0];
        if(existingPrimary != null )
        {
            
            
            if(primaryInfo.Equals(existingPrimary.Value)) return;
            existingPrimary.Type = Activator.CreateInstance<>();  ;
        }
        if(_relations == null) _relations = new  Collection<IClientRelation<TAsset, >>();
        var Primary =  Activator.CreateInstance(type : ty, );
            
            Primary.Client = client;
            Primary.Value = primaryInfo;
            Primary.Type = Activator.CreateInstance<Secondary>();                  
        
        _relations.Add(Primary);

    }

    public void AddSecondaryRelation(
        Client client, TAsset secondaryInfo)
    {

    }

    public void DeleteRelation( Client client, TAsset assetInfo, Predicate<> primaryCheck)
    {
        if (assetInfo == null) throw new ArgumentNullException(nameof(assetInfo));

        var relationToRemove = _relations.FirstOrDefault(c =>
        c.Value != null && c.Value.Equals(assetInfo) && c.ClientId == client.Id);
        if (relationToRemove.IsType(primaryCheck))
        {
            throw new InvalidOperationException("Cannot delete a primary relation.");
        }

        if (relationToRemove != null)            
        _relations.Remove(relationToRemove);

    } 
    */
}