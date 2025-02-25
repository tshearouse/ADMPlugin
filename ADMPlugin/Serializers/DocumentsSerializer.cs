using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgGateway.ADAPT.ADMPlugin.Converters;
using AgGateway.ADAPT.ADMPlugin.Models;
using AgGateway.ADAPT.ApplicationDataModel.ADM;
using AgGateway.ADAPT.ApplicationDataModel.Documents;
using AgGateway.ADAPT.ApplicationDataModel.Equipment;
using AgGateway.ADAPT.ApplicationDataModel.Guidance;
using AgGateway.ADAPT.ApplicationDataModel.LoggedData;

namespace AgGateway.ADAPT.ADMPlugin.Serializers
{
  public class DocumentsSerializer : ISerializer<Documents>
  {
      private readonly ISpatialRecordConverter _spatialRecordConverter;

      public DocumentsSerializer() : this(new SpatialRecordConverter())
      {
      }

      public DocumentsSerializer(ISpatialRecordConverter spatialRecordConverter)
      {
          _spatialRecordConverter = spatialRecordConverter;
      }

    public void Serialize(IBaseSerializer baseSerializer, Documents documents, string dataPath)
    {
      if (documents == null)
      {
        return;
      }

      var documentsPath = Path.Combine(dataPath, DatacardConstants.DocumentsFolder);
      if (!Directory.Exists(documentsPath))
      {
        Directory.CreateDirectory(documentsPath);
      }

      WriteLoggedData(baseSerializer, documents, documentsPath);
      WriteGuidanceAllocations(baseSerializer, documents, documentsPath);
      WritePlans(baseSerializer, documents, documentsPath);
      WriteRecommendations(baseSerializer, documents, documentsPath);
      WriteSummaries(baseSerializer, documents, documentsPath);
      WriteWorkRecords(baseSerializer, documents, documentsPath);
      WriteWorkItemOperations(baseSerializer, documents, documentsPath);
      WriteWorkItems(baseSerializer, documents, documentsPath);
      WriteWorkOrders(baseSerializer, documents, documentsPath);
      WriteLoads(baseSerializer, documents, documentsPath);
    }

    public Documents Deserialize(IBaseSerializer baseSerializer, string dataPath)
    {
      var documentsPath = Path.Combine(dataPath, DatacardConstants.DocumentsFolder);
      if (!Directory.Exists(documentsPath))
      {
        return null;
      }

      var documents = new Documents();
      documents.LoggedData = ReadLoggedData(baseSerializer, documentsPath);
      documents.GuidanceAllocations = ReadGuidanceAllocations(baseSerializer, documentsPath);
      documents.Plans = ReadPlans(baseSerializer, documentsPath);
      documents.Recommendations = ReadRecommendations(baseSerializer, documentsPath);
      documents.Summaries = ReadSummaries(baseSerializer, documentsPath);
      documents.WorkRecords = ReadWorkRecords(baseSerializer, documentsPath);
      documents.WorkItemOperations = ReadWorkItemOperations(baseSerializer, documentsPath);
      documents.WorkItems = ReadWorkItems(baseSerializer, documentsPath);
      documents.WorkOrders = ReadWorkOrders(baseSerializer, documentsPath);
      documents.Loads = ReadLoads(baseSerializer, documentsPath);

      return documents;
    }

    private void WriteWorkRecords(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.WorkRecords == null)
      {
        return;
      }

      foreach (var workRecord in documents.WorkRecords)
      {
        if (workRecord != null)
        {
          WriteObject(baseSerializer, documentsPath, workRecord, workRecord.Id.ReferenceId, DatacardConstants.WorkRecordFile);
        }
      }
    }

    private void WriteWorkOrders(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.WorkOrders == null)
      {
        return;
      }

      foreach (var workOrder in documents.WorkOrders)
      {
        if (workOrder != null)
        {
          WriteObject(baseSerializer, documentsPath, workOrder, workOrder.Id.ReferenceId, DatacardConstants.WorkOrderFile);
        }
      }
    }

    private void WriteWorkItems(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.WorkItems == null)
      {
        return;
      }

      foreach (var workItem in documents.WorkItems)
      {
        if (workItem != null)
        {
          WriteObject(baseSerializer, documentsPath, workItem, workItem.Id.ReferenceId, DatacardConstants.WorkItemFile);
        }
      }
    }

    private void WriteWorkItemOperations(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.WorkItemOperations == null)
      {
        return;
      }

      foreach (var workItemOperation in documents.WorkItemOperations)
      {
        if (workItemOperation != null)
        {
          WriteObject(baseSerializer, documentsPath, workItemOperation, workItemOperation.Id.ReferenceId, DatacardConstants.WorkItemOperationFile);
        }
      }
    }

    private void WriteSummaries(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.Summaries == null)
      {
        return;
      }

      foreach (var summary in documents.Summaries)
      {
        if (summary != null)
        {
          WriteObject(baseSerializer, documentsPath, summary, summary.Id.ReferenceId, DatacardConstants.SummaryFile);
        }
      }
    }

    private void WriteRecommendations(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.Recommendations == null)
      {
        return;
      }

      foreach (var recommendation in documents.Recommendations)
      {
        if (recommendation != null)
        {
          WriteObject(baseSerializer, documentsPath, recommendation, recommendation.Id.ReferenceId, DatacardConstants.RecommendationFile);
        }
      }
    }

    private void WriteLoads(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.Loads == null)
      {
        return;
      }

      foreach (var load in documents.Loads)
      {
        if (load != null)
        {
          WriteObject(baseSerializer, documentsPath, load, load.Id.ReferenceId, DatacardConstants.LoadFile);
        }
      }
    }

    private void WritePlans(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.Plans == null)
      {
        return;
      }

      foreach (var plan in documents.Plans)
      {
        if (plan != null)
        {
          WriteObject(baseSerializer, documentsPath, plan, plan.Id.ReferenceId, DatacardConstants.PlanFile);
        }
      }
    }

    private void WriteGuidanceAllocations(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.GuidanceAllocations == null)
      {
        return;
      }

      foreach (var guidanceAllocation in documents.GuidanceAllocations)
      {
        if (guidanceAllocation != null)
        {
          WriteObject(baseSerializer, documentsPath, guidanceAllocation, guidanceAllocation.Id.ReferenceId, DatacardConstants.GuidanceAllocationFile);
        }
      }
    }

    private void WriteLoggedData(IBaseSerializer baseSerializer, Documents documents, string documentsPath)
    {
      if (documents.LoggedData == null)
      {
        return;
      }

      foreach (var loggedData in documents.LoggedData)
      {
        if (loggedData != null)
        {
          if (loggedData.OperationData != null)
          {
            loggedData.OperationData = loggedData.OperationData.ToList();
            foreach (var operationData in loggedData.OperationData)
            {
              var deviceElementUses = GetAllDeviceElementUses(operationData);
              var workingData = deviceElementUses.SelectMany(deviceElementUse => deviceElementUse.Value.SelectMany(x => x.GetWorkingDatas())).ToList();
              // Order matters: We must export spatialRecords before Meters.
              // If WorkingData.UnitOfMeasure is not populated, then we copy it from the NumericRepresentationValue's in the SpatialRecords.
              ExportSpatialRecords(baseSerializer, documentsPath, operationData, workingData);
              ExportSectionsAndMeters(baseSerializer, documentsPath, operationData, deviceElementUses, workingData);
            }
          }
          WriteObject(baseSerializer, documentsPath, loggedData, loggedData.Id.ReferenceId, DatacardConstants.LoggedDataFile);
        }
      }
    }

    private void WriteObject<T>(IBaseSerializer baseSerializer, string documentsPath, T obj, int referenceId, string fileNameFormat)
    {
      var fileName = String.Format(fileNameFormat, referenceId);
      var filePath = Path.Combine(documentsPath, fileName);
      baseSerializer.Serialize(obj, filePath);
    }

    private void ExportSpatialRecords(IBaseSerializer baseSerializer, string documentsPath, OperationData operationData, List<WorkingData> meters)
    {
      var fileName = string.Format(DatacardConstants.SpatialRecordsFile, operationData.Id.ReferenceId);
      var filePath = Path.Combine(documentsPath, fileName);

      if (operationData.GetSpatialRecords == null)
      {
        baseSerializer.SerializeWithLengthPrefix(new List<SerializableSpatialRecord>(), filePath);
        return;
      }

      var spatialRecords = operationData.GetSpatialRecords();
      var serializableSpatialRecords = _spatialRecordConverter.ConvertToSerializableSpatialRecords(spatialRecords, meters);

      baseSerializer.SerializeWithLengthPrefix(serializableSpatialRecords, filePath);
    }

    private void ExportSectionsAndMeters(IBaseSerializer baseSerializer, string documentsPath, OperationData operationData, Dictionary<int, IEnumerable<DeviceElementUse>> deviceElementUses, List<WorkingData> workingData)
    {
      var deviceElementUseFileName = string.Format(DatacardConstants.SectionFile, operationData.Id.ReferenceId);
      var deviceElementUseFilePath = Path.Combine(documentsPath, deviceElementUseFileName);

      var workingDataFileName = string.Format(DatacardConstants.WorkingDataFile, operationData.Id.ReferenceId);
      var workingDataFilePath = Path.Combine(documentsPath, workingDataFileName);

      baseSerializer.Serialize(deviceElementUses, deviceElementUseFilePath);
      baseSerializer.Serialize(workingData, workingDataFilePath);
    }

    private Dictionary<int, IEnumerable<DeviceElementUse>> GetAllDeviceElementUses(OperationData operationData)
    {
      if (operationData == null)
      {
        return null;
      }

      var sections = new Dictionary<int, IEnumerable<DeviceElementUse>>();

      for (var depth = 0; depth <= operationData.MaxDepth; depth++)
      {
        if (operationData.GetDeviceElementUses == null)
        {
          continue;
        }

        var levelSections = operationData.GetDeviceElementUses(depth);
        sections.Add(depth, levelSections);
      }

      return sections;
    }

    private IEnumerable<WorkRecord> ReadWorkRecords(IBaseSerializer baseSerializer, string documentsPath)
    {
        var workRecordFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.WorkRecordFile));
        foreach (var workRecordFile in workRecordFiles)
        {
            yield return baseSerializer.Deserialize<WorkRecord>(workRecordFile);
        }
    }

    private IEnumerable<WorkOrder> ReadWorkOrders(IBaseSerializer baseSerializer, string documentsPath)
    {
        var loggedDataFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.WorkOrderFile));
        foreach (var loggedDataFile in loggedDataFiles)
        {
            yield return baseSerializer.Deserialize<WorkOrder>(loggedDataFile);
        }
    }

    private IEnumerable<WorkItem> ReadWorkItems(IBaseSerializer baseSerializer, string documentsPath)
    {
        var loggedDataFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.WorkItemFileOnly));
        foreach (var loggedDataFile in loggedDataFiles)
        {
            yield return baseSerializer.Deserialize<WorkItem>(loggedDataFile);
        }
    }

    private IEnumerable<WorkItemOperation> ReadWorkItemOperations(IBaseSerializer baseSerializer, string documentsPath)
    {
        var workItemOperationFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.WorkItemOperationFile));
        foreach (var workItemOperationFile in workItemOperationFiles)
        {
            yield return baseSerializer.Deserialize<WorkItemOperation>(workItemOperationFile);
        }
    }

    private IEnumerable<Summary> ReadSummaries(IBaseSerializer baseSerializer, string documentsPath)
    {
        var summaryFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.SummaryFile));
        foreach (var summaryFile in summaryFiles)
        {
            yield return baseSerializer.Deserialize<Summary>(summaryFile);
        }
    }

    private IEnumerable<Recommendation> ReadRecommendations(IBaseSerializer baseSerializer, string documentsPath)
    {
        var recommendationFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.RecommendationFile));
        foreach (var recommendationFile in recommendationFiles)
        {
            yield return baseSerializer.Deserialize<Recommendation>(recommendationFile);
        }
    }

    private IEnumerable<Plan> ReadPlans(IBaseSerializer baseSerializer, string documentsPath)
    {
        var planFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.PlanFile));
        foreach (var planFile in planFiles)
        {
            yield return baseSerializer.Deserialize<Plan>(planFile);
        }
    }

    private IEnumerable<GuidanceAllocation> ReadGuidanceAllocations(IBaseSerializer baseSerializer, string documentsPath)
    {
        var guidanceAllocationFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.GuidanceAllocationFile));
        foreach (var guidanceAllocationFile in guidanceAllocationFiles)
        {
            yield return baseSerializer.Deserialize<GuidanceAllocation>(guidanceAllocationFile);
        }
    }

    private IEnumerable<Load> ReadLoads(IBaseSerializer baseSerializer, string documentsPath)
    {
        var loadFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.LoadFileReadOnly));
        foreach (var loadFile in loadFiles)
        {
            yield return baseSerializer.Deserialize<Load>(loadFile);
        }
    }

    private IEnumerable<LoggedData> ReadLoggedData(IBaseSerializer baseSerializer, string documentsPath)
    {
        var loggedDataFiles = Directory.EnumerateFiles(documentsPath, ConvertToSearchPattern(DatacardConstants.LoggedDataFile));
        foreach (var loggedDataFile in loggedDataFiles)
        {
           yield return MapLoggedData(baseSerializer, documentsPath, loggedDataFile);
        }
    }

      private LoggedData MapLoggedData(IBaseSerializer baseSerializer, string documentsPath, string loggedDataFile)
      {
          var loggedData = baseSerializer.Deserialize<LoggedData>(loggedDataFile);
          foreach (var operationData in loggedData.OperationData)
          {
              ImportSections(baseSerializer, documentsPath, operationData);
              var meters = ImportMeters(baseSerializer, documentsPath, operationData);
              ImportSpatialRecords(baseSerializer, documentsPath, operationData, meters);
          }

          return loggedData;
      }

    private IEnumerable<WorkingData> ImportMeters(IBaseSerializer baseSerializer, string documentsPath, OperationData operationData)
    {
      var deviceElementUses = GetAllDeviceElementUses(operationData).Where(x => x.Value != null).SelectMany(x => x.Value);

      var workingDataFileName = string.Format(DatacardConstants.WorkingDataFile, operationData.Id.ReferenceId);
      var workingDataFilePath = Path.Combine(documentsPath, workingDataFileName);
      var allWorkingData = baseSerializer.Deserialize<IEnumerable<WorkingData>>(workingDataFilePath);

      foreach (var deviceElementUse in deviceElementUses)
      {
        var deviceElementUseWorkingData = allWorkingData.Where(x => x.DeviceElementUseId == deviceElementUse.Id.ReferenceId);
        deviceElementUse.GetWorkingDatas = () => deviceElementUseWorkingData;
      }

      return allWorkingData;
    }

    private void ImportSections(IBaseSerializer baseSerializer, string documentsPath, OperationData operationData)
    {
      var sectionsFileName = string.Format(DatacardConstants.SectionFile, operationData.Id.ReferenceId);
      var sectionsFilePath = Path.Combine(documentsPath, sectionsFileName);
      var sections = baseSerializer.Deserialize<Dictionary<int, IEnumerable<DeviceElementUse>>>(sectionsFilePath);

      if (sections != null && sections.Any())
        operationData.GetDeviceElementUses = x => sections[x] ?? new List<DeviceElementUse>();
    }

    private void ImportSpatialRecords(IBaseSerializer baseSerializer, string documentsPath, OperationData operationData, IEnumerable<WorkingData> workingDatas)
    {
      var protobufSpatialRecordsFileName = string.Format(DatacardConstants.SpatialRecordsFile, operationData.Id.ReferenceId);
      var protobufSpatialRecordsFilePath = Path.Combine(documentsPath, protobufSpatialRecordsFileName);
      if (File.Exists(protobufSpatialRecordsFilePath))
      {
          var spatialRecords = baseSerializer.DeserializeWithLengthPrefix<SerializableSpatialRecord>(protobufSpatialRecordsFilePath);
          operationData.GetSpatialRecords = () => _spatialRecordConverter.ConvertToSpatialRecords(spatialRecords, workingDatas);
      }
      else
      {
          var spatialRecordFileName = string.Format(DatacardConstants.OperationDataFile, operationData.Id.ReferenceId);
          var spatialRecordFilePath = Path.Combine(documentsPath, spatialRecordFileName);

          operationData.GetSpatialRecords = () => baseSerializer.DeserializeWithLengthPrefix<SpatialRecord>(spatialRecordFilePath);
      }
    }

    private string ConvertToSearchPattern(string filePattern)
    {
      return string.Format(filePattern, "*");
    }
  }
}
