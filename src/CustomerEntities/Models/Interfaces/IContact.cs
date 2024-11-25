using InfraEntities.Interfaces;

namespace CustomerEntities.Interfaces;

public interface IContact<T> : IHasAsset<T>, IAuditEntity, IHasPriorityOrder {}