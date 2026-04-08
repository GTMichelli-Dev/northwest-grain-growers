/**
* DevExpress Analytics (dist\js\dx-querybuilder.js)
* Version:  25.2.5
* Build date: Feb 25, 2026
* Copyright (c) 2012 - 2026 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/universal.xml
*/
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	// The require scope
/******/ 	var __webpack_require__ = {};
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/

// UNUSED EXPORTS: default

// NAMESPACE OBJECT: ./dist/analytics-diagram.js
var analytics_diagram_namespaceObject = {};
__webpack_require__.r(analytics_diagram_namespaceObject);
__webpack_require__.d(analytics_diagram_namespaceObject, {
  ConnectingPointDragHandler: () => (ConnectingPointDragHandler),
  ConnectingPointSurface: () => (ConnectingPointSurface),
  ConnectingPointViewModel: () => (ConnectingPointViewModel),
  ConnectionPointDragHandler: () => (ConnectionPointDragHandler),
  ConnectionPointSurface: () => (ConnectionPointSurface),
  ConnectionPointViewModel: () => (ConnectionPointViewModel),
  ConnectorSurface: () => (ConnectorSurface),
  ConnectorViewModel: () => (ConnectorViewModel),
  DiagramElementBaseSurface: () => (DiagramElementBaseSurface),
  DiagramElementBaseViewModel: () => (DiagramElementBaseViewModel),
  DiagramElementSurface: () => (DiagramElementSurface),
  DiagramElementViewModel: () => (DiagramElementViewModel),
  DiagramSurface: () => (DiagramSurface),
  DiagramViewModel: () => (DiagramViewModel),
  GRID_SIZE: () => (GRID_SIZE),
  PointSide: () => (PointSide),
  RoutedConnectorSurface: () => (RoutedConnectorSurface),
  RoutedConnectorViewModel: () => (RoutedConnectorViewModel),
  connectingPointSerializationInfo: () => (connectingPointSerializationInfo),
  connectionPointSerializationInfo: () => (connectionPointSerializationInfo),
  createDiagramDesigner: () => (createDiagramDesigner),
  determineConnectingPoints: () => (determineConnectingPoints),
  diagramControlsFactory: () => (diagramControlsFactory),
  diagramElementSerializationInfo: () => (diagramElementSerializationInfo),
  diagramSerializationsInfo: () => (diagramSerializationsInfo),
  groups: () => (groups),
  location: () => (metadata_location),
  margins: () => (margins),
  name: () => (metadata_name),
  pageHeight: () => (pageHeight),
  pageWidth: () => (pageWidth),
  registerControls: () => (registerControls),
  size: () => (size),
  sizeLocation: () => (sizeLocation),
  text: () => (metadata_text),
  unknownSerializationsInfo: () => (unknownSerializationsInfo)
});

// NAMESPACE OBJECT: ./dist/analytics-data.js
var analytics_data_namespaceObject = {};
__webpack_require__.r(analytics_data_namespaceObject);
__webpack_require__.d(analytics_data_namespaceObject, {
  ConnectionOptions: () => (ConnectionOptions),
  CustomSqlQuery: () => (CustomSqlQuery),
  DBColumn: () => (DBColumn),
  DBColumnType: () => (DBColumnType),
  DBForeignKey: () => (DBForeignKey),
  DBSchema: () => (DBSchema),
  DBSchemaProvider: () => (DBSchemaProvider),
  DBStoredProcedure: () => (DBStoredProcedure),
  DBStoredProcedureArgument: () => (DBStoredProcedureArgument),
  DBStoredProcedureArgumentDirection: () => (DBStoredProcedureArgumentDirection),
  DBTable: () => (DBTable),
  DataSourceParameter: () => (DataSourceParameter),
  FederatedQueriesContainer: () => (FederatedQueriesContainer),
  FederatedQueryExpression: () => (FederatedQueryExpression),
  FederatedQueryExpressionType: () => (FederatedQueryExpressionType),
  FederationDataSource: () => (FederationDataSource),
  FederationMasterDetailRelation: () => (FederationMasterDetailRelation),
  FederationSource: () => (FederationSource),
  FederationTransformationRule: () => (FederationTransformationRule),
  JsonAuthenticationInfo: () => (JsonAuthenticationInfo),
  JsonDataSource: () => (JsonDataSource),
  JsonNode: () => (JsonNode),
  JsonNodeType: () => (JsonNodeType),
  JsonParameter: () => (JsonParameter),
  JsonParameterType: () => (JsonParameterType),
  JsonSchemaNode: () => (JsonSchemaNode),
  JsonSchemaProvider: () => (JsonSchemaProvider),
  JsonSchemaRootNode: () => (JsonSchemaRootNode),
  JsonSource: () => (JsonSource),
  MasterDetailRelation: () => (MasterDetailRelation),
  MongoDBDataConnection: () => (MongoDBDataConnection),
  MongoDBDataSource: () => (MongoDBDataSource),
  MongoDBQuery: () => (MongoDBQuery),
  MongoDBSchemaProvider: () => (MongoDBSchemaProvider),
  ObjectCtor: () => (ObjectCtor),
  ObjectDataMember: () => (ObjectDataMember),
  ObjectDataMemberBase: () => (ObjectDataMemberBase),
  ObjectDataSource: () => (ObjectDataSource),
  ObjectDataSourceMethodBase: () => (ObjectDataSourceMethodBase),
  ObjectParameter: () => (ObjectParameter),
  ObjectType: () => (ObjectType),
  ResultSet: () => (ResultSet),
  ResultTable: () => (ResultTable),
  SelectQuery: () => (SelectQuery),
  SerializableDataFederationDataSource: () => (SerializableDataFederationDataSource),
  SourceQuery: () => (SourceQuery),
  SqlDataConnection: () => (SqlDataConnection),
  SqlDataSource: () => (SqlDataSource),
  StoredProcQuery: () => (StoredProcQuery),
  SubNode: () => (SubNode),
  TableQuery: () => (TableQuery),
  TransformQuery: () => (TransformQuery),
  UnionQuery: () => (UnionQuery),
  UnionTypes: () => (UnionTypes),
  _resetGetMongoDBSchemaCallback: () => (_resetGetMongoDBSchemaCallback),
  _setGetMongoDBSchemaCallback: () => (_setGetMongoDBSchemaCallback),
  expressionSerializationInfo: () => (expressionSerializationInfo),
  getMongoDBSchemaCallback: () => (getMongoDBSchemaCallback),
  mongoDBQuerySerializationInfo: () => (mongoDBQuerySerializationInfo),
  selectQuerySerializationsInfo: () => (selectQuerySerializationsInfo),
  sourceQuerySerializationsInfo: () => (sourceQuerySerializationsInfo),
  subNodeSerializationInfo: () => (subNodeSerializationInfo),
  transformQuerySerializationsInfo: () => (transformQuerySerializationsInfo),
  transformationRuleSerializationsInfo: () => (transformationRuleSerializationsInfo),
  unionQuerySerializationsInfo: () => (unionQuerySerializationsInfo)
});

// NAMESPACE OBJECT: ./dist/analytics-data-internal.js
var analytics_data_internal_namespaceObject = {};
__webpack_require__.r(analytics_data_internal_namespaceObject);
__webpack_require__.d(analytics_data_internal_namespaceObject, {
  DBSchemaFederationDataSourceProvider: () => (DBSchemaFederationDataSourceProvider),
  _processJsonSchemaResponse: () => (_processJsonSchemaResponse),
  _resetGetJsonSchemaCallback: () => (_resetGetJsonSchemaCallback),
  _setGetJsonSchemaCallback: () => (_setGetJsonSchemaCallback),
  deserializeToCollection: () => (deserializeToCollection),
  generateQueryUniqueName: () => (generateQueryUniqueName),
  getDBSchemaCallback: () => (getDBSchemaCallback),
  getDBStoredProceduresCallback: () => (getDBStoredProceduresCallback),
  getJsonSchemaCallback: () => (getJsonSchemaCallback)
});

// NAMESPACE OBJECT: ./dist/analytics-data-utils.js
var analytics_data_utils_namespaceObject = {};
__webpack_require__.r(analytics_data_utils_namespaceObject);
__webpack_require__.d(analytics_data_utils_namespaceObject, {
  FederationQueryType: () => (FederationQueryType),
  JsonSourceType: () => (JsonSourceType),
  SqlQueryType: () => (SqlQueryType)
});

// NAMESPACE OBJECT: ./dist/analytics-data-metadata.js
var analytics_data_metadata_namespaceObject = {};
__webpack_require__.r(analytics_data_metadata_namespaceObject);
__webpack_require__.d(analytics_data_metadata_namespaceObject, {
  DataConnectionBase: () => (DataConnectionBase),
  customQuerySerializationsInfo: () => (customQuerySerializationsInfo),
  dataConnectionSerializationInfo: () => (dataConnectionSerializationInfo),
  dsParameterNameValidationRules: () => (dsParameterNameValidationRules),
  dsParameterSerializationInfo: () => (dsParameterSerializationInfo),
  federationDataSourceSerializationInfo: () => (federationDataSourceSerializationInfo),
  masterDetailRelationSerializationsInfo: () => (masterDetailRelationSerializationsInfo),
  parameterValueSerializationsInfo: () => (parameterValueSerializationsInfo),
  sourceSerializationInfo: () => (sourceSerializationInfo),
  storedProcParameterSerializationsInfo: () => (storedProcParameterSerializationsInfo),
  storedProcQuerySerializationsInfo: () => (storedProcQuerySerializationsInfo),
  tableQuerySerializationsInfo: () => (tableQuerySerializationsInfo),
  validateName: () => (validateName)
});

// NAMESPACE OBJECT: ./dist/analytics-wizard.js
var analytics_wizard_namespaceObject = {};
__webpack_require__.r(analytics_wizard_namespaceObject);
__webpack_require__.d(analytics_wizard_namespaceObject, {
  AutoQueryPreload: () => (AutoQueryPreload),
  BaseWizard: () => (BaseWizard),
  ChooseAvailableDataSourcePage: () => (ChooseAvailableDataSourcePage),
  ChooseAvailableItemPage: () => (ChooseAvailableItemPage),
  ChooseDataSourceTypePage: () => (ChooseDataSourceTypePage),
  ChooseJsonConnectionPage: () => (ChooseJsonConnectionPage),
  ChooseJsonSchemaPage: () => (ChooseJsonSchemaPage),
  ChooseJsonSchemaPageBase: () => (ChooseJsonSchemaPageBase),
  ChooseJsonSourcePage: () => (ChooseJsonSourcePage),
  ChooseMongoDBCollectionPage: () => (ChooseMongoDBCollectionPage),
  ChooseMongoDBDataConnectionPage: () => (ChooseMongoDBDataConnectionPage),
  ChooseMongoDBSchemaPage: () => (ChooseMongoDBSchemaPage),
  ChooseObjectDataSourceDataMembersPage: () => (ChooseObjectDataSourceDataMembersPage),
  ChooseObjectDataSourceTypesPage: () => (ChooseObjectDataSourceTypesPage),
  ChooseSqlConnectionPage: () => (ChooseSqlConnectionPage),
  ConfigureMasterDetailRelationshipsPage: () => (ConfigureMasterDetailRelationshipsPage),
  ConfigureObjectDataSourceParametersPage: () => (ConfigureObjectDataSourceParametersPage),
  ConfigureQueryPage: () => (ConfigureQueryPage),
  ConfigureQueryParametersPage: () => (ConfigureQueryParametersPage),
  DataSourceType: () => (DataSourceType),
  DataSourceWizard: () => (DataSourceWizard),
  DataSourceWizardPageId: () => (DataSourceWizardPageId),
  DataSourceWizardPageIterator: () => (DataSourceWizardPageIterator),
  DataSourceWizardSettings: () => (DataSourceWizardSettings),
  FederatedMasterDetailRelationshipsPage: () => (FederatedMasterDetailRelationshipsPage),
  FederatedQueryConfigurePage: () => (FederatedQueryConfigurePage),
  FederationDataSourceWizardPageId: () => (FederationDataSourceWizardPageId),
  FullscreenDataSourceWizard: () => (FullscreenDataSourceWizard),
  FullscreenDataSourceWizardPageId: () => (FullscreenDataSourceWizardPageId),
  FullscreenDataSourceWizardPageIterator: () => (FullscreenDataSourceWizardPageIterator),
  FullscreenDataSourceWizardSectionId: () => (FullscreenDataSourceWizardSectionId),
  FullscreenWizard: () => (FullscreenWizard),
  FullscreenWizardPage: () => (FullscreenWizardPage),
  FullscreenWizardPageFactory: () => (FullscreenWizardPageFactory),
  JsonDataSourceWizardPageId: () => (JsonDataSourceWizardPageId),
  MasterDetailRelationshipsPageBase: () => (MasterDetailRelationshipsPageBase),
  MongoDBDataSourceWizardPageId: () => (MongoDBDataSourceWizardPageId),
  MultiQueryConfigurePage: () => (MultiQueryConfigurePage),
  MultiQueryConfigureParametersPage: () => (MultiQueryConfigureParametersPage),
  MultiQueryDataSourceWizard: () => (MultiQueryDataSourceWizard),
  MultiQueryDataSourceWizardPageIterator: () => (MultiQueryDataSourceWizardPageIterator),
  ObjectDataSourceWizardPageId: () => (ObjectDataSourceWizardPageId),
  PageFactory: () => (PageFactory),
  PageIterator: () => (PageIterator),
  PopupWizard: () => (PopupWizard),
  SelectDataSourcePage: () => (SelectDataSourcePage),
  SpecifyFederationDataSourceSettingsPage: () => (SpecifyFederationDataSourceSettingsPage),
  SpecifyJsonConnectionPage: () => (SpecifyJsonConnectionPage),
  SpecifyJsonDataSourceSettingsPage: () => (SpecifyJsonDataSourceSettingsPage),
  SpecifyMongoDBDataSourceSettingsPage: () => (SpecifyMongoDBDataSourceSettingsPage),
  SpecifyObjectDataSourceSettingsPage: () => (SpecifyObjectDataSourceSettingsPage),
  SpecifySqlDataSourceSettingsPage: () => (SpecifySqlDataSourceSettingsPage),
  SqlDataSourceWizardPageId: () => (SqlDataSourceWizardPageId),
  StateManager: () => (StateManager),
  TypeItem: () => (TypeItem),
  WizardNavigationPanel: () => (WizardNavigationPanel),
  WizardPageBase: () => (WizardPageBase),
  _DataSourceWizardOptions: () => (_DataSourceWizardOptions),
  _DataSourceWizardOptionsBase: () => (_DataSourceWizardOptionsBase),
  _MultiQueryDataSourceWizardOptions: () => (_MultiQueryDataSourceWizardOptions),
  _SqlDataSourceWrapper: () => (_SqlDataSourceWrapper),
  _WrappedWizardPage: () => (_WrappedWizardPage),
  __loadingStateFunctionName: () => (__loadingStateFunctionName),
  __nextActionFunctionName: () => (__nextActionFunctionName),
  _canEditQueryParameters: () => (_canEditQueryParameters),
  _createDataSourceFullscreenWizard: () => (_createDataSourceFullscreenWizard),
  _createDataSourceWizard: () => (_createDataSourceWizard),
  _createDefaultDataSourceWizardState: () => (_createDefaultDataSourceWizardState),
  _createMultiQueryDataSourceWizard: () => (_createMultiQueryDataSourceWizard),
  _registerChooseAvailableDataSourcePage: () => (_registerChooseAvailableDataSourcePage),
  _registerChooseDataSourceTypePage: () => (_registerChooseDataSourceTypePage),
  _registerChooseJsonConnectionPage: () => (_registerChooseJsonConnectionPage),
  _registerChooseJsonSchemaPage: () => (_registerChooseJsonSchemaPage),
  _registerChooseJsonSourcePage: () => (_registerChooseJsonSourcePage),
  _registerChooseMongoDBCollectionPage: () => (_registerChooseMongoDBCollectionPage),
  _registerChooseMongoDBDataConnectionPage: () => (_registerChooseMongoDBDataConnectionPage),
  _registerChooseMongoDBSchemaPage: () => (_registerChooseMongoDBSchemaPage),
  _registerChooseObjectDataSourceDataMembersPage: () => (_registerChooseObjectDataSourceDataMembersPage),
  _registerChooseObjectDataSourceTypesPage: () => (_registerChooseObjectDataSourceTypesPage),
  _registerChooseSqlConnectionPage: () => (_registerChooseSqlConnectionPage),
  _registerConfigureMasterDetailRelationshipsPage: () => (_registerConfigureMasterDetailRelationshipsPage),
  _registerConfigureObjectDataSourceParametersPage: () => (_registerConfigureObjectDataSourceParametersPage),
  _registerConfigureParametersPage: () => (_registerConfigureParametersPage),
  _registerConfigureQueryPage: () => (_registerConfigureQueryPage),
  _registerDataSourceWizardPages: () => (_registerDataSourceWizardPages),
  _registerFederatedMasterDetailRelationshipsPage: () => (_registerFederatedMasterDetailRelationshipsPage),
  _registerFederatedQueryConfigurePage: () => (_registerFederatedQueryConfigurePage),
  _registerMultiQueryConfigurePage: () => (_registerMultiQueryConfigurePage),
  _registerMultiQueryConfigureParametersPage: () => (_registerMultiQueryConfigureParametersPage),
  _registerMultiQueryDataSourcePages: () => (_registerMultiQueryDataSourcePages),
  _registerSelectDataSourcePage: () => (_registerSelectDataSourcePage),
  _registerSpecifyFederationDataSourceSettingsPage: () => (_registerSpecifyFederationDataSourceSettingsPage),
  _registerSpecifyJsonConnectionPage: () => (_registerSpecifyJsonConnectionPage),
  _registerSpecifyJsonDataSourceSettingsPage: () => (_registerSpecifyJsonDataSourceSettingsPage),
  _registerSpecifyMongoDBDataSourceSettingsPage: () => (_registerSpecifyMongoDBDataSourceSettingsPage),
  _registerSpecifyObjectDataSourceSettingsPage: () => (_registerSpecifyObjectDataSourceSettingsPage),
  _registerSpecifySqlDataSourceSettingsPage: () => (_registerSpecifySqlDataSourceSettingsPage),
  _resetRestoreJsonDataSourceFromState: () => (_resetRestoreJsonDataSourceFromState),
  _resetRestoreMongoDBDataSourceFromState: () => (_resetRestoreMongoDBDataSourceFromState),
  _resetRestoreSqlDataSourceFromState: () => (_resetRestoreSqlDataSourceFromState),
  _restoreFederationDataSourceFromState: () => (_restoreFederationDataSourceFromState),
  _restoreJsonDataSourceFromState: () => (_restoreJsonDataSourceFromState),
  _restoreMongoDBDataSourceFromState: () => (_restoreMongoDBDataSourceFromState),
  _restoreObjectDataSourceFromState: () => (_restoreObjectDataSourceFromState),
  _restoreSqlDataSourceFromState: () => (_restoreSqlDataSourceFromState),
  _setRestoreJsonDataSourceFromState: () => (_setRestoreJsonDataSourceFromState),
  _setRestoreMongoDBDataSourceFromState: () => (_setRestoreMongoDBDataSourceFromState),
  _setRestoreSqlDataSourceFromState: () => (_setRestoreSqlDataSourceFromState),
  parameterTypeToPropertyMap: () => (parameterTypeToPropertyMap)
});

// NAMESPACE OBJECT: ./dist/analytics-wizard-internal.js
var analytics_wizard_internal_namespaceObject = {};
__webpack_require__.r(analytics_wizard_internal_namespaceObject);
__webpack_require__.d(analytics_wizard_internal_namespaceObject, {
  ChooseObjectDataMembers: () => (ChooseObjectDataMembers),
  ChooseObjectMemberParameters: () => (ChooseObjectMemberParameters),
  ChooseObjectParameters: () => (ChooseObjectParameters),
  ChooseObjectTypes: () => (ChooseObjectTypes),
  ChooseObjectTypesTreelistController: () => (ChooseObjectTypesTreelistController),
  CustomQueryTreeListItem: () => (CustomQueryTreeListItem),
  DBSchemaItemsProvider: () => (DBSchemaItemsProvider),
  DBSchemaTreeListController: () => (DBSchemaTreeListController),
  DataMemberTreeNode: () => (DataMemberTreeNode),
  FederatedQueriesTreeNode: () => (FederatedQueriesTreeNode),
  FederationDataSourceItemsExtender: () => (FederationDataSourceItemsExtender),
  FederationQueryBuilderPopupBase: () => (FederationQueryBuilderPopupBase),
  FederationSelectQueryBuilderPopup: () => (FederationSelectQueryBuilderPopup),
  FederationTablesExpressionFieldListProvider: () => (FederationTablesExpressionFieldListProvider),
  FederationTransformQueryBuilderPopup: () => (FederationTransformQueryBuilderPopup),
  FederationTreeNodeProvider: () => (FederationTreeNodeProvider),
  FederationUnionQueryBuilderPopup: () => (FederationUnionQueryBuilderPopup),
  FieldTreeNode: () => (FieldTreeNode),
  JsonDataSourceJsonSourcePageSettingsBase: () => (JsonDataSourceJsonSourcePageSettingsBase),
  JsonDataSourceJsonSourcePageStringSettings: () => (JsonDataSourceJsonSourcePageStringSettings),
  JsonDataSourceJsonSourcePageUriSettings: () => (JsonDataSourceJsonSourcePageUriSettings),
  JsonStringEditor: () => (JsonStringEditor),
  JsonTreeNodeItemsProvider: () => (JsonTreeNodeItemsProvider),
  MultiQueryTreeListItemFactory: () => (MultiQueryTreeListItemFactory),
  ObjectDataSourceParameterProperty: () => (ObjectDataSourceParameterProperty),
  ObjectDataSourceParametersModel: () => (ObjectDataSourceParametersModel),
  ObjectSchemaProvider: () => (ObjectSchemaProvider),
  ObjectTypeDescriptions: () => (ObjectTypeDescriptions),
  ParameterTreeNode: () => (ParameterTreeNode),
  ParametersTreeListController: () => (ParametersTreeListController),
  ParametersTreeListItem: () => (ParametersTreeListItem),
  ParametersTreeListItemBase: () => (ParametersTreeListItemBase),
  ParametersTreeListRootItem: () => (ParametersTreeListRootItem),
  ParametersTreeListRootItemBase: () => (ParametersTreeListRootItemBase),
  QueriesTreeNode: () => (QueriesTreeNode),
  QueryBuilderPopup: () => (QueryBuilderPopup),
  QueryBuilderPopupBase: () => (QueryBuilderPopupBase),
  SelectQuerySqlTextProvider: () => (SelectQuerySqlTextProvider),
  SelectStatementQueryControl: () => (SelectStatementQueryControl),
  SingleCheckedDataMemberTreeNode: () => (SingleCheckedDataMemberTreeNode),
  StoredProceduresQueryControl: () => (StoredProceduresQueryControl),
  TransformResultSchemaProvider: () => (TransformResultSchemaProvider),
  TreeLeafNode: () => (TreeLeafNode),
  TreeNode: () => (TreeNode),
  TreeNodeBase: () => (TreeNodeBase),
  TreeNodeItemsProvider: () => (TreeNodeItemsProvider),
  TreeQueryNode: () => (TreeQueryNode),
  WizardAction: () => (WizardAction),
  WizardPageProcessor: () => (WizardPageProcessor),
  WizardPageSection: () => (WizardPageSection),
  WizardPageSectionFactory: () => (WizardPageSectionFactory),
  WizardPageSectionIterator: () => (WizardPageSectionIterator),
  WizardSectionPosition: () => (WizardSectionPosition),
  WrappedWizardPageSection: () => (WrappedWizardPageSection),
  _createBeforeInitializePageEventArgs: () => (_createBeforeInitializePageEventArgs),
  _createPageEventArgs: () => (_createPageEventArgs),
  _isMoreThanOneDataSourceTypeAvailable: () => (_isMoreThanOneDataSourceTypeAvailable),
  defaultObjectDataSourceItemSpecifics: () => (defaultObjectDataSourceItemSpecifics),
  getLocalizedValidationErrorMessage: () => (getLocalizedValidationErrorMessage),
  getObjectTypeDescriptionsCallback: () => (getObjectTypeDescriptionsCallback),
  getSectionStyle: () => (getSectionStyle),
  subscribeArray: () => (subscribeArray),
  subscribeObject: () => (subscribeObject),
  subscribeProperties: () => (subscribeProperties)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-widgets.js
var queryBuilder_widgets_namespaceObject = {};
__webpack_require__.r(queryBuilder_widgets_namespaceObject);
__webpack_require__.d(queryBuilder_widgets_namespaceObject, {
  expressionFunctions: () => (expressionFunctions)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-widgets-internal.js
var queryBuilder_widgets_internal_namespaceObject = {};
__webpack_require__.r(queryBuilder_widgets_internal_namespaceObject);
__webpack_require__.d(queryBuilder_widgets_internal_namespaceObject, {
  FederatedQueriesHelper: () => (FederatedQueriesHelper),
  GroupFilterEditorSerializer: () => (GroupFilterEditorSerializer),
  KeyColumnSurface: () => (KeyColumnSurface),
  ManageFederatedQueriesEditor: () => (ManageFederatedQueriesEditor),
  MasterDetailEditor: () => (MasterDetailEditor),
  MasterDetailEditorPopupManager: () => (MasterDetailEditorPopupManager),
  MasterDetailRelationSurface: () => (MasterDetailRelationSurface),
  MasterQuerySurface: () => (MasterQuerySurface),
  OperandParameterQBSurface: () => (OperandParameterQBSurface),
  OperandPropertyQBSurface: () => (OperandPropertyQBSurface),
  QBFilterEditorHelper: () => (QBFilterEditorHelper),
  QBFilterEditorHelperDefault: () => (QBFilterEditorHelperDefault),
  QBFilterStringOptions: () => (QBFilterStringOptions),
  QueryBuilderObjectsProvider: () => (QueryBuilderObjectsProvider),
  RightPanelSwitcher: () => (RightPanelSwitcher),
  UndoEditor: () => (UndoEditor),
  _setQBFilterEditorHelperDefault: () => (_setQBFilterEditorHelperDefault),
  createDefaultSQLAceOptions: () => (createDefaultSQLAceOptions),
  createDefaultSQLAdditionalOptions: () => (createDefaultSQLAdditionalOptions),
  createDefaultSQLLanguageHelper: () => (createDefaultSQLLanguageHelper),
  editorTemplates: () => (editorTemplates),
  expressionFunctions: () => (expressionFunctions),
  isAggregatedExpression: () => (isAggregatedExpression)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-metadata.js
var queryBuilder_metadata_namespaceObject = {};
__webpack_require__.r(queryBuilder_metadata_namespaceObject);
__webpack_require__.d(queryBuilder_metadata_namespaceObject, {
  alias: () => (alias),
  location: () => (query_builder_metadata_location),
  name: () => (query_builder_metadata_name),
  selected: () => (selected),
  size: () => (metadata_size),
  sizeLocation: () => (metadata_sizeLocation),
  text: () => (query_builder_metadata_text),
  unknownSerializationsInfo: () => (metadata_unknownSerializationsInfo)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-utils.js
var queryBuilder_utils_namespaceObject = {};
__webpack_require__.r(queryBuilder_utils_namespaceObject);
__webpack_require__.d(queryBuilder_utils_namespaceObject, {
  ActionId: () => (ActionId),
  FederationQueryType: () => (FederationQueryType),
  HandlerUri: () => (HandlerUri),
  JsonSourceType: () => (JsonSourceType),
  RequestWrapper: () => (RequestWrapper),
  SqlQueryType: () => (SqlQueryType),
  controlsFactory: () => (controlsFactory)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-internal.js
var queryBuilder_internal_namespaceObject = {};
__webpack_require__.r(queryBuilder_internal_namespaceObject);
__webpack_require__.d(queryBuilder_internal_namespaceObject, {
  AccordionTabInfo: () => (AccordionTabInfo),
  ColumnDragHandler: () => (ColumnDragHandler),
  ColumnExpressionCollectionHelper: () => (ColumnExpressionCollectionHelper),
  DbObjectDragDropHandler: () => (DbObjectDragDropHandler),
  FederatedTransformQueryBuilderTreeListController: () => (FederatedTransformQueryBuilderTreeListController),
  FederatedUnionQueryBuilderTreeListController: () => (FederatedUnionQueryBuilderTreeListController),
  FederationAllColumnsViewModel: () => (FederationAllColumnsViewModel),
  FederationColumnViewModel: () => (FederationColumnViewModel),
  FederationQuerySurface: () => (FederationQuerySurface),
  FederationQueryViewModel: () => (FederationQueryViewModel),
  FederationTableSurface: () => (FederationTableSurface),
  FederationTableViewModel: () => (FederationTableViewModel),
  JsonSchemaFieldListProviderFactory: () => (JsonSchemaFieldListProviderFactory),
  QueryBuilderTreeListController: () => (QueryBuilderTreeListController),
  SelectedTabInfo: () => (SelectedTabInfo),
  createIsLoadingFlag: () => (createIsLoadingFlag),
  createQueryBuilder: () => (createQueryBuilder),
  createQueryBuilderSurface: () => (createQueryBuilderSurface),
  federationQuerySerializationsInfo: () => (federationQuerySerializationsInfo),
  registerControls: () => (_controlsFactory_registerControls),
  serializeDataConnection: () => (serializeDataConnection),
  updateQueryBuilderSurfaceContentSize: () => (updateQueryBuilderSurfaceContentSize),
  wrapGetFederationdResultSchema: () => (wrapGetFederationdResultSchema),
  wrapGetSelectStatement: () => (wrapGetSelectStatement),
  wrapRebuildResultSchema: () => (wrapRebuildResultSchema)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-elements.js
var queryBuilder_elements_namespaceObject = {};
__webpack_require__.r(queryBuilder_elements_namespaceObject);
__webpack_require__.d(queryBuilder_elements_namespaceObject, {
  AllColumnsSurface: () => (AllColumnsSurface),
  AllColumnsViewModel: () => (AllColumnsViewModel),
  ColumnExpression: () => (ColumnExpression),
  ColumnSurface: () => (ColumnSurface),
  ColumnViewModel: () => (ColumnViewModel),
  JoinConditionSurface: () => (JoinConditionSurface),
  JoinConditionViewModel: () => (JoinConditionViewModel),
  ParameterViewModel: () => (ParameterViewModel),
  QueryElementBaseSurface: () => (QueryElementBaseSurface),
  QueryElementBaseViewModel: () => (QueryElementBaseViewModel),
  QuerySurface: () => (QuerySurface),
  QueryViewModel: () => (QueryViewModel),
  QueryViewModelBase: () => (QueryViewModelBase),
  RelationSurface: () => (RelationSurface),
  RelationViewModel: () => (RelationViewModel),
  TableSurface: () => (TableSurface),
  TableViewModel: () => (TableViewModel)
});

// NAMESPACE OBJECT: ./dist/queryBuilder-elements-metadata.js
var queryBuilder_elements_metadata_namespaceObject = {};
__webpack_require__.r(queryBuilder_elements_metadata_namespaceObject);
__webpack_require__.d(queryBuilder_elements_metadata_namespaceObject, {
  AggregationType: () => (AggregationType),
  ColumnType: () => (ColumnType),
  ConditionType: () => (ConditionType),
  ParametersMode: () => (ParametersMode),
  allColumnsSerializationInfo: () => (allColumnsSerializationInfo),
  columnExpressionSerializationsInfo: () => (columnExpressionSerializationsInfo),
  columnSerializationInfo: () => (columnSerializationInfo),
  joinConditionSerializationInfo: () => (joinConditionSerializationInfo),
  querySerializationsInfo: () => (querySerializationsInfo),
  relationSerializationInfo: () => (relationSerializationInfo),
  tableSerializationInfo: () => (tableSerializationInfo)
});

// NAMESPACE OBJECT: ./dist/querybuilder.js
var querybuilder_namespaceObject = {};
__webpack_require__.r(querybuilder_namespaceObject);
__webpack_require__.d(querybuilder_namespaceObject, {
  DxQueryBuilder: () => (DxQueryBuilder),
  EventGenerator: () => (EventGenerator),
  JSQueryBuilder: () => (JSQueryBuilder),
  JSQueryBuilderBinding: () => (JSQueryBuilderBinding)
});

;// external "DevExpress.Analytics.Widgets"
const external_DevExpress_Analytics_Widgets_namespaceObject = DevExpress.Analytics.Widgets;
;// external "DevExpress.Analytics.Internal"
const external_DevExpress_Analytics_Internal_namespaceObject = DevExpress.Analytics.Internal;
;// external "DevExpress.Analytics.Elements"
const external_DevExpress_Analytics_Elements_namespaceObject = DevExpress.Analytics.Elements;
;// ./dist/diagram/metadata.js




const metadata_name = { propertyName: 'name', modelName: '@Name', displayName: 'Name', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text'), validationRules: external_DevExpress_Analytics_Internal_namespaceObject.nameValidationRules };
const metadata_text = { propertyName: 'text', modelName: '@Text', displayName: 'Text', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text') };
const size = { propertyName: 'size', modelName: '@Size', defaultVal: '100,50', from: external_DevExpress_Analytics_Elements_namespaceObject.Size.fromString, displayName: 'Size', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('objecteditor') };
const metadata_location = { propertyName: 'location', modelName: '@Location', from: external_DevExpress_Analytics_Elements_namespaceObject.Point.fromString, displayName: 'Location', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('objecteditor') };
const sizeLocation = [size, metadata_location];
const unknownSerializationsInfo = [metadata_name].concat(sizeLocation);

;// external "ko"
const external_ko_namespaceObject = ko;
;// ./dist/diagram/elements/diagramElementBaseSurface.js



class DiagramElementBaseSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context, unitProperties) {
        super(control, context, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, DiagramElementBaseSurface._unitProperties, unitProperties));
        this.template = 'dx-diagram-element';
        this.selectiontemplate = 'dx-diagram-element-selection';
        this.contenttemplate = 'dx-diagram-element-content';
        this.margin = external_ko_namespaceObject.observable(0);
        this._disposables.push(this.positionWidthWithoutMargins = external_ko_namespaceObject.pureComputed(() => {
            return this['position'].width() - this.margin() * 2;
        }));
        this._disposables.push(this.positionLineHeightWithoutMargins = external_ko_namespaceObject.pureComputed(() => {
            return this['position'].lineHeight() - this.margin() * 2;
        }));
    }
}
DiagramElementBaseSurface._unitProperties = {
    _height: (o) => {
        return o.size.height;
    },
    _width: (o) => {
        return o.size.width;
    },
    _x: (o) => {
        return o.location.x;
    },
    _y: (o) => {
        return o.location.y;
    }
};

;// ./dist/diagram/elements/connectingPointSurface.js


class ConnectingPointSurface extends DiagramElementBaseSurface {
    constructor(control, context) {
        super(control, context, ConnectingPointSurface._unitProperties);
        this.template = 'dxdd-connecting-point';
        this.selectiontemplate = 'dxdd-connection-point-selection';
        this.contenttemplate = '';
    }
}
ConnectingPointSurface._unitProperties = {
    _x: (o) => {
        return external_ko_namespaceObject.pureComputed(() => { return o.location.x() - o.parentModel().location.x(); });
    },
    _y: (o) => {
        return external_ko_namespaceObject.pureComputed(() => { return o.location.y() - o.parentModel().location.y(); });
    }
};

;// ./dist/diagram/utils.js
var PointSide;
(function (PointSide) {
    PointSide[PointSide["East"] = 0] = "East";
    PointSide[PointSide["South"] = 1] = "South";
    PointSide[PointSide["North"] = 2] = "North";
    PointSide[PointSide["West"] = 3] = "West";
})(PointSide || (PointSide = {}));
const GRID_SIZE = 10;
function determineConnectingPoints(startObject, endObject) {
    const result = { start: null, end: null };
    if (endObject.leftConnectionPoint.location.x() > startObject.rightConnectionPoint.location.x() + GRID_SIZE * 2) {
        result.start = startObject.rightConnectionPoint;
        result.end = endObject.leftConnectionPoint;
    }
    else if (startObject.leftConnectionPoint.location.x() > endObject.rightConnectionPoint.location.x() + GRID_SIZE * 2) {
        result.start = startObject.leftConnectionPoint;
        result.end = endObject.rightConnectionPoint;
    }
    else {
        const startCenter = (startObject.rightConnectionPoint.location.x() + startObject.rightConnectionPoint.location.x()) / 2;
        const endCenter = (endObject.rightConnectionPoint.location.x() + endObject.rightConnectionPoint.location.x()) / 2;
        if (startCenter > endCenter) {
            result.start = startObject.rightConnectionPoint;
            result.end = endObject.rightConnectionPoint;
        }
        else {
            result.start = startObject.leftConnectionPoint;
            result.end = endObject.leftConnectionPoint;
        }
    }
    return result;
}

;// external "DevExpress.Analytics.Utils"
const external_DevExpress_Analytics_Utils_namespaceObject = DevExpress.Analytics.Utils;
;// ./dist/diagram/controlsFactory.js

const diagramControlsFactory = new external_DevExpress_Analytics_Utils_namespaceObject.ControlsFactory();

;// ./dist/diagram/elements/diagramElementBaseViewModel.js



class DiagramElementBaseViewModel extends external_DevExpress_Analytics_Elements_namespaceObject.ElementViewModel {
    getControlFactory() {
        return diagramControlsFactory;
    }
    constructor(control, parent, serializer) {
        super(control, parent, serializer);
    }
}
const diagramElementSerializationInfo = [size, metadata_location, metadata_name, metadata_text, { propertyName: 'type', modelName: '@Type' }];

;// ./dist/diagram/elements/connectingPointModel.js







class ConnectingPointViewModel extends DiagramElementBaseViewModel {
    constructor(control, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'ConnectingPoint' }, control), parent, serializer);
        this.side = external_ko_namespaceObject.pureComputed(() => {
            if (this.percentOffsetY() >= this.percentOffsetX()) {
                if (this.percentOffsetY() > 1 - this.percentOffsetX()) {
                    return PointSide.South;
                }
                else {
                    return PointSide.West;
                }
            }
            else {
                if (this.percentOffsetY() > 1 - this.percentOffsetX()) {
                    return PointSide.East;
                }
                else {
                    return PointSide.North;
                }
            }
        });
        this.size = new external_DevExpress_Analytics_Elements_namespaceObject.Size(7, 7);
        this.location = new external_DevExpress_Analytics_Elements_namespaceObject.Point(0, 0);
        this.location.x = external_ko_namespaceObject.pureComputed(() => {
            const parentModel = this.parentModel();
            return parentModel.location.x() + parentModel.size.width() * this.percentOffsetX();
        });
        this.location.y = external_ko_namespaceObject.pureComputed(() => {
            const parentModel = this.parentModel();
            return parentModel.location.y() + parentModel.size.height() * this.percentOffsetY();
        });
    }
}
const connectingPointSerializationInfo = [
    { propertyName: 'percentOffsetX', modelName: '@PercentOffsetX', defaultVal: 0.5, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel },
    { propertyName: 'percentOffsetY', modelName: '@PercentOffsetY', defaultVal: 0.5, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel }
];

;// ./dist/diagram/elements/diagramElementViewModel.js




class DiagramElementViewModel extends DiagramElementBaseViewModel {
    constructor(control, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'DiagramElement' }, control), parent, serializer);
        this.connectingPoints = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(control && control.ConnectingPoints || [], (item) => { return new ConnectingPointViewModel(item, this, serializer); });
        if (this.text() === undefined) {
            this.text(this.name());
        }
    }
}

;// ./dist/diagram/dragDrop/connectingPointDragHandler.js






class ConnectingPointDragHandler extends external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler {
    constructor(surface, selection, undoEngine, snapHelper, dragHelperContent) {
        super(surface, selection, undoEngine, snapHelper, dragHelperContent);
        this.startConnectingPoint = null;
        this.newConnector = null;
        this.cursor = 'arrow';
        this.containment = '.dxrd-ghost-container';
        this['helper'] = (draggable) => {
            dragHelperContent.update(draggable);
        };
    }
    startDrag(control) {
        if (!(control instanceof ConnectingPointSurface)) {
            throw new Error('ConnectingPointDragHandler can be applied to the ConnectingPoint only.');
        }
        this.startConnectingPoint = control;
        const diagramElement = this.startConnectingPoint.parent.getControlModel();
        this.newConnector = diagramElement.parentModel().createChild({ '@ControlType': 'RoutedConnector' });
        this.newConnector.startPoint().connectingPoint(this.startConnectingPoint.getControlModel());
    }
    drag(event, uiElement) {
        uiElement.dataset.leftPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.leftPosition) + uiElement['scroll'].left);
        uiElement.dataset.topPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.topPosition) + uiElement['scroll'].top);
        const position = this._getAbsoluteSurfacePosition(uiElement);
        this.newConnectorSurface.endPoint().rect({ top: position.top, left: position.left });
    }
    doStopDrag() {
        this.dragHelperContent.reset();
        if (this.selection.dropTarget) {
            const dropTarget = this.selection.dropTarget.getControlModel();
            if (dropTarget instanceof ConnectingPointViewModel) {
                this.newConnector.endPoint().connectingPoint(dropTarget);
            }
            else if (dropTarget instanceof DiagramElementViewModel) {
                const connectings = dropTarget.connectingPoints();
                this.newConnector.endPoint().connectingPoint(connectings[0]);
            }
            this.selection.initialize(this.newConnectorSurface);
        }
    }
    get newConnectorSurface() {
        return this.newConnector && (0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(this.newConnector);
    }
}

;// ./dist/diagram/elements/connectors/connectionPointSurface.js


class ConnectionPointSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context) {
        super(control, context, ConnectionPointSurface._unitProperties);
        this.template = 'dx-diagram-connection-point';
        this.selectiontemplate = 'dx-diagram-connection-point';
        this.relativeX = external_ko_namespaceObject.pureComputed(() => {
            return this.rect().left - this.parent.rect().left;
        });
        this.relativeY = external_ko_namespaceObject.pureComputed(() => {
            return this.rect().top - this.parent.rect().top;
        });
    }
    container() {
        return this.getRoot();
    }
}
ConnectionPointSurface._unitProperties = {
    _x: (o) => {
        return o.location.x;
    },
    _y: (o) => {
        return o.location.y;
    }
};

;// ./dist/diagram/dragDrop/connectionPointDragHandler.js





class ConnectionPointDragHandler extends external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler {
    constructor(surface, selection, undoEngine, snapHelper, dragHelperContent) {
        super(surface, selection, undoEngine, snapHelper, dragHelperContent);
        this.currentConnectionPoint = null;
        this.cursor = 'arrow';
        this.containment = '.dxrd-ghost-container';
        this['helper'] = (draggable) => {
            dragHelperContent.update(draggable);
        };
    }
    startDrag(control) {
        if (!(control instanceof ConnectionPointSurface)) {
            throw new Error('ConnectionPointDragHandler can be applied to the ConnectionPoint only.');
        }
        this.currentConnectionPoint = control;
    }
    drag(event, uiElement) {
        uiElement.dataset.leftPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.leftPosition) + uiElement['scroll'].left);
        uiElement.dataset.topPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.topPosition) + uiElement['scroll'].top);
        const position = this._getAbsoluteSurfacePosition(uiElement);
        this.currentConnectionPoint.rect({ top: position.top, left: position.left });
    }
    doStopDrag() {
        this.dragHelperContent.reset();
        if (this.selection.dropTarget) {
            const dropTarget = this.selection.dropTarget.getControlModel();
            if (dropTarget instanceof ConnectingPointViewModel) {
                const connector = this.currentConnectionPoint.parent.getControlModel();
                if (this.currentConnectionPoint.getControlModel() === connector.startPoint()) {
                    connector.startPoint().connectingPoint(dropTarget);
                }
                else {
                    connector.endPoint().connectingPoint(dropTarget);
                }
            }
            else if (dropTarget instanceof DiagramElementViewModel) {
                const connector = this.currentConnectionPoint.parent.getControlModel();
                const connectings = dropTarget.connectingPoints();
                if (this.currentConnectionPoint.getControlModel() === connector.startPoint()) {
                    connector.startPoint().connectingPoint(connectings[0]);
                }
                else {
                    connector.endPoint().connectingPoint(connectings[0]);
                }
            }
        }
    }
}

;// ./dist/diagram/elements/diagramElementSurface.js

class DiagramElementSurface extends DiagramElementBaseSurface {
    constructor(control, context) {
        super(control, context, null);
        this.contenttemplate = 'dxdd-element-content-with-connecting-points';
    }
    _getChildrenHolderName() { return 'connectingPoints'; }
}

;// ./dist/diagram/elements/diagramModel.js







class DiagramViewModel extends DiagramElementBaseViewModel {
    getInfo() {
        return diagramSerializationsInfo;
    }
    constructor(diagramSource) {
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        super(diagramSource, null, serializer);
        this.controlType = 'Diagram';
        this.controls = external_ko_namespaceObject.observableArray();
        this.name('Diagram');
    }
}
const margins = { propertyName: 'margins', modelName: '@Margins', from: external_DevExpress_Analytics_Elements_namespaceObject.Margins.fromString, displayName: 'Margins' };
const pageWidth = { propertyName: 'pageWidth', modelName: '@PageWidth', defaultVal: 850, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel, displayName: 'Page Width', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('numeric') };
const pageHeight = { propertyName: 'pageHeight', modelName: '@PageHeight', defaultVal: 1250, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel, displayName: 'Page Height', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('numeric') };
const diagramSerializationsInfo = [metadata_name, pageWidth, pageHeight, margins];

;// ./dist/diagram/elements/diagramSurface.js




class DiagramSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(diagram, zoom = external_ko_namespaceObject.observable(1)) {
        super(diagram, {
            measureUnit: external_ko_namespaceObject.observable('Pixels'),
            zoom: zoom,
            dpi: external_ko_namespaceObject.observable(100)
        }, DiagramSurface._unitProperties);
        this.measureUnit = external_ko_namespaceObject.observable('Pixels');
        this.dpi = external_ko_namespaceObject.observable(100);
        this.controls = external_ko_namespaceObject.observableArray();
        this.allowMultiselect = false;
        this.focused = external_ko_namespaceObject.observable(false);
        this.selected = external_ko_namespaceObject.observable(false);
        this.underCursor = external_ko_namespaceObject.observable(new external_DevExpress_Analytics_Internal_namespaceObject.HoverInfo());
        this.templateName = 'dx-diagram-surface';
        this.margins = { bottom: this['_bottom'], left: this['_left'], right: this['_right'], top: this['_top'] };
        this.zoom = zoom;
        this._context = this;
        (0,external_DevExpress_Analytics_Internal_namespaceObject.createObservableArrayMapCollection)(diagram.controls, this.controls, this._createSurface);
    }
    checkParent(surfaceParent) { return false; }
    get parent() {
        return this._parent;
    }
    set parent(newVal) {
        this._parent = newVal;
    }
    getChildrenCollection() {
        return external_ko_namespaceObject.observableArray([]);
    }
}
DiagramSurface._unitProperties = {
    _width: (o) => { return o.pageWidth; },
    _height: (o) => { return o.pageWidth; },
    pageWidth: (o) => { return o.pageWidth; },
    pageHeight: (o) => { return o.pageHeight; },
    _bottom: (o) => { return o.margins.bottom; },
    _left: (o) => { return o.margins.left; },
    _right: (o) => { return o.margins.right; },
    _top: (o) => { return o.margins.top; }
};

;// ./dist/diagram/elements/connectors/connectionPointModel.js




class ConnectionPointViewModel extends DiagramElementBaseViewModel {
    constructor(control, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(control, { '@ControlType': 'ConnectionPoint' }), parent, serializer);
        const _x = this.location.x, _y = this.location.y;
        this.location.x = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.connectingPoint() && this.connectingPoint().location.x() || _x();
            },
            write: (value) => {
                this.connectingPoint(null);
                _x(value);
            }
        });
        this.location.y = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.connectingPoint() && this.connectingPoint().location.y() || _y();
            },
            write: (value) => {
                this.connectingPoint(null);
                _y(value);
            }
        });
    }
}
const connectionPointSerializationInfo = [
    metadata_location,
    { propertyName: 'connectingPoint', modelName: '@ConnectingPoint', link: true }
];

;// ./dist/diagram/elements/connectors/connectorModel.js






class ConnectorViewModel extends DiagramElementBaseViewModel {
    getX() {
        return this.startPoint().location.x() < this.endPoint().location.x() ? this.startPoint().location.x() : this.endPoint().location.x();
    }
    getY() {
        return this.startPoint().location.y() < this.endPoint().location.y() ? this.startPoint().location.y() : this.endPoint().location.y();
    }
    getWidth() {
        return Math.abs(this.startPoint().location.x() - this.endPoint().location.x()) || ConnectorViewModel.MIN_LINE_THICKNESS;
    }
    getHeight() {
        return Math.abs(this.startPoint().location.y() - this.endPoint().location.y()) || ConnectorViewModel.MIN_LINE_THICKNESS;
    }
    constructor(control, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'Connector' }, control), parent, serializer);
        this.startPoint(this.startPoint() || new ConnectionPointViewModel({ '@Location': '0, 0' }, this, serializer));
        this.endPoint(this.endPoint() || new ConnectionPointViewModel({ '@Location': '150, 75' }, this, serializer));
        this.location = new external_DevExpress_Analytics_Elements_namespaceObject.Point(0, 0);
        this._disposables.push(this.location.x = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.getX();
            },
            write: (value) => {
                const oldValue = this.startPoint().location.x() < this.endPoint().location.x() ? this.startPoint().location.x() : this.endPoint().location.x();
                const delta = value - oldValue;
                this.startPoint().location.x(this.startPoint().location.x() + delta);
                this.endPoint().location.x(this.endPoint().location.x() + delta);
            }
        }));
        this._disposables.push(this.location.y = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.getY();
            },
            write: (value) => {
                const oldValue = this.startPoint().location.y() < this.endPoint().location.y() ? this.startPoint().location.y() : this.endPoint().location.y();
                const delta = value - oldValue;
                this.startPoint().location.y(this.startPoint().location.y() + delta);
                this.endPoint().location.y(this.endPoint().location.y() + delta);
            }
        }));
        this.size = new external_DevExpress_Analytics_Elements_namespaceObject.Size(0, 0);
        this._disposables.push(this.size.width = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.getWidth();
            },
            write: (value) => {
                if (this.startPoint().location.x() < this.endPoint().location.x()) {
                    this.endPoint().location.x(this.startPoint().location.x() + value);
                }
                else {
                    this.startPoint().location.x(this.endPoint().location.x() + value);
                }
            }
        }));
        this._disposables.push(this.size.height = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this.getHeight();
            },
            write: (value) => {
                if (this.startPoint().location.y() < this.endPoint().location.y()) {
                    this.endPoint().location.y(this.startPoint().location.y() + value);
                }
                else {
                    this.startPoint().location.y(this.endPoint().location.y() + value);
                }
            }
        }));
    }
}
ConnectorViewModel.MIN_LINE_THICKNESS = 3;

;// ./dist/diagram/elements/connectors/connectorSurface.js



class ConnectorSurface extends DiagramElementBaseSurface {
    constructor(control, context) {
        super(control, context, null);
        this.template = 'dxdd-connector';
        this.selectiontemplate = 'dxdd-connector-selection';
        this.startPoint = external_ko_namespaceObject.pureComputed(() => {
            return new ConnectionPointSurface(control.startPoint(), context);
        });
        this.endPoint = external_ko_namespaceObject.pureComputed(() => {
            return new ConnectionPointSurface(control.endPoint(), context);
        });
    }
}

;// ./dist/diagram/elements/connectors/routedConnectorModel.js





class RoutedConnectorViewModel extends ConnectorViewModel {
    getX() {
        let result = super.getX();
        this.routePoints && this.routePoints().forEach((point) => {
            if (point.x() < result) {
                result = point.x();
            }
        });
        return result;
    }
    getY() {
        let result = super.getY();
        this.routePoints && this.routePoints().forEach((point) => {
            if (point.y() < result) {
                result = point.y();
            }
        });
        return result;
    }
    getWidth() {
        let result = super.getWidth();
        const baseX = this.getX();
        this.routePoints && [this.startPoint().location, this.endPoint().location].concat(this.routePoints()).forEach((point) => {
            if (point.x() - baseX > result) {
                result = point.x() - baseX;
            }
        });
        return result;
    }
    getHeight() {
        let result = super.getHeight();
        const baseY = this.getY();
        this.routePoints && [this.startPoint().location, this.endPoint().location].concat(this.routePoints()).forEach((point) => {
            if (point.y() - baseY > result) {
                result = point.y() - baseY;
            }
        });
        return Math.round(result);
    }
    _fixPoint(point, side) {
        switch (side) {
            case PointSide.North:
                point.y(point.y() - GRID_SIZE);
                break;
            case PointSide.East:
                point.x(point.x() + GRID_SIZE);
                break;
            case PointSide.West:
                point.x(point.x() - GRID_SIZE);
                break;
            case PointSide.South:
                point.y(point.y() + GRID_SIZE);
        }
    }
    _getStartPointSide() {
        if (this.startPoint().connectingPoint()) {
            return this.startPoint().connectingPoint().side();
        }
        if (this.startPoint().location.y() !== this.endPoint().location.y()) {
            if (this.startPoint().location.y() > this.endPoint().location.y()) {
                return PointSide.North;
            }
            else {
                return PointSide.South;
            }
        }
        else {
            if (this.startPoint().location.x() > this.endPoint().location.x()) {
                return PointSide.West;
            }
            else {
                return PointSide.East;
            }
        }
    }
    _getEndPointSide() {
        if (this.endPoint().connectingPoint()) {
            return this.endPoint().connectingPoint().side();
        }
        if (this.startPoint().location.y() !== this.endPoint().location.y()) {
            if (this.startPoint().location.y() > this.endPoint().location.y()) {
                return PointSide.South;
            }
            else {
                return PointSide.North;
            }
        }
        else {
            if (this.startPoint().location.x() > this.endPoint().location.x()) {
                return PointSide.East;
            }
            else {
                return PointSide.West;
            }
        }
    }
    _getRatio(n) {
        const pow2Delimiter = this._getPower(n);
        const delimiter = Math.pow(2, pow2Delimiter);
        const halfDelimeter = Math.pow(2, pow2Delimiter - 1);
        const arr = [];
        for (let i = 1; i < halfDelimeter; i++) {
            if (i % 2 == 0) {
                continue;
            }
            arr.push(delimiter - i);
            arr.push(i);
        }
        arr.reverse();
        const delta = n - halfDelimeter;
        const number = arr[delta] || 1;
        return number / delimiter;
    }
    constructor(control, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'RoutedConnector' }, control), parent, serializer);
        this._isUpdating = false;
        this._getPower = function (_n) {
            for (let i = 0; i < 10; i++) {
                if (_n >= Math.pow(2, i) && _n < Math.pow(2, i + 1)) {
                    return i + 1;
                }
            }
            return 1;
        };
        this.seriesNumber = external_ko_namespaceObject.observable(1);
        this.routePoints = external_ko_namespaceObject.observable([]);
        this.freezeRoute = external_ko_namespaceObject.observable(false);
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            const freezeRoute = !(1 + this.startPoint().location.x() + this.startPoint().location.y()
                + this.endPoint().location.x() + this.endPoint().location.y());
            if (!this._isUpdating) {
                this.freezeRoute(freezeRoute);
            }
        }));
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            if (!this.freezeRoute()) {
                const result = [];
                const startPointSide = this._getStartPointSide();
                const endPointSide = this._getEndPointSide();
                const startPoint = new external_DevExpress_Analytics_Elements_namespaceObject.Point(this.startPoint().location.x(), this.startPoint().location.y()), endPoint = new external_DevExpress_Analytics_Elements_namespaceObject.Point(this.endPoint().location.x(), this.endPoint().location.y());
                if (this.startPoint().connectingPoint()) {
                    this._fixPoint(startPoint, this.startPoint().connectingPoint().side());
                    result.push(startPoint);
                }
                if (this.endPoint().connectingPoint()) {
                    this._fixPoint(endPoint, this.endPoint().connectingPoint().side());
                }
                let baseX = Math.min(startPoint.x(), endPoint.x()), width = Math.abs(startPoint.x() - endPoint.x());
                const baseY = Math.min(startPoint.y(), endPoint.y()), height = Math.abs(startPoint.y() - endPoint.y());
                const number = this.seriesNumber();
                const ratio = this._getRatio(number);
                const indent = (number - 1) * GRID_SIZE;
                if (startPoint.y() - endPoint.y() > 0) {
                    if (startPoint.x() - endPoint.x() > 0) {
                        if (startPointSide === PointSide.North || startPointSide === PointSide.East) {
                            if (endPointSide === PointSide.North || endPointSide === PointSide.East) {
                                if (number !== 1) {
                                    width += indent;
                                    result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height));
                                }
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height * ratio));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height * ratio));
                            }
                        }
                        else {
                            if (endPointSide === PointSide.South || endPointSide === PointSide.West) {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY + height));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY));
                            }
                        }
                    }
                    else {
                        if (startPointSide === PointSide.North || startPointSide === PointSide.West) {
                            if (endPointSide === PointSide.North || endPointSide === PointSide.West) {
                                if (number !== 1) {
                                    baseX -= indent;
                                    result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height));
                                }
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height * ratio));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height * ratio));
                            }
                        }
                        else {
                            if (endPointSide === PointSide.South || endPointSide === PointSide.East) {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY + height));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY));
                            }
                        }
                    }
                }
                else {
                    if (startPoint.x() - endPoint.x() > 0) {
                        if (startPointSide === PointSide.South || startPointSide === PointSide.East) {
                            if (endPointSide === PointSide.South || endPointSide === PointSide.East) {
                                if (number !== 1) {
                                    width += indent;
                                    result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY));
                                }
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height * ratio));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height * ratio));
                            }
                        }
                        else {
                            if (endPointSide === PointSide.North || endPointSide === PointSide.West) {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY + height));
                            }
                        }
                    }
                    else {
                        if (startPointSide === PointSide.South || startPointSide === PointSide.West) {
                            if (endPointSide === PointSide.South || endPointSide === PointSide.West) {
                                if (number !== 1) {
                                    baseX -= indent;
                                    result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY));
                                }
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX, baseY + height * ratio));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY + height * ratio));
                            }
                        }
                        else {
                            if (endPointSide === PointSide.North || endPointSide === PointSide.East) {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width, baseY));
                            }
                            else {
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY));
                                result.push(new external_DevExpress_Analytics_Elements_namespaceObject.Point(baseX + width * ratio, baseY + height));
                            }
                        }
                    }
                }
                if (this.endPoint().connectingPoint()) {
                    result.push(endPoint);
                }
                this.routePoints(result);
            }
        }));
    }
    beginUpdate() { this._isUpdating = true; }
    endUpdate() { this._isUpdating = false; }
}

;// ./dist/diagram/elements/connectors/routedConnectorSurface.js



class RoutedConnectorSurface extends DiagramElementBaseSurface {
    _createRoutePoint(point, base) {
        return {
            x: external_ko_namespaceObject.pureComputed(() => {
                if (this._context.rtl()) {
                    return Math.round(this.getControlModel().size.width() - (point.x() - base.x()));
                }
                else {
                    return Math.round(point.x() - base.x());
                }
            }),
            y: external_ko_namespaceObject.pureComputed(() => { return Math.round(point.y() - base.y()); }),
            modelPoint: point
        };
    }
    _createRouteLineWrapper(point1, point2, isLocked = false) {
        const _self = this, isVerticalLine = Math.abs(point1.x.peek() - point2.x.peek()) < 1, absoluteTop = point1.modelPoint.y.peek(), absoluteLeft = point1.modelPoint.x.peek(), position = {
            top: Math.min(point1.y.peek(), point2.y.peek()) - 2,
            left: Math.min(point1.x.peek(), point2.x.peek()) - 2,
            width: Math.abs(point1.x.peek() - point2.x.peek()) + 6,
            height: Math.abs(point1.y.peek() - point2.y.peek()) + 6
        }, resizeHandler = (params) => {
            _self._control.freezeRoute(true);
            try {
                _self._control.beginUpdate();
                if (isVerticalLine) {
                    let newX = absoluteLeft + params.delta.dx;
                    if (this._context.rtl()) {
                        newX = absoluteLeft - params.delta.dx;
                    }
                    point1.modelPoint.x(newX);
                    point2.modelPoint.x(newX);
                }
                else {
                    const newY = absoluteTop + params.delta.dy;
                    point1.modelPoint.y(newY);
                    point2.modelPoint.y(newY);
                }
            }
            finally {
                _self._control.endUpdate();
            }
        };
        return {
            position: position,
            isVerticalLine: isVerticalLine,
            resizeHandler: resizeHandler,
            resizeStopped: () => {
                _self._control.routePoints.notifySubscribers(_self._control.routePoints());
            },
            isLocked: external_ko_namespaceObject.observable(isLocked)
        };
    }
    _updateRoutePoints() {
        const points = [], control = this.getControlModel(), base = control.location;
        points.push(this._createRoutePoint(control.startPoint().location, base));
        control.routePoints().forEach((point) => {
            points.push(this._createRoutePoint(point, base));
        });
        points.push(this._createRoutePoint(control.endPoint().location, base));
        this.routePoints(points);
    }
    constructor(control, context) {
        super(control, context, null);
        this.template = 'dxdd-routed-connector';
        this.selectiontemplate = 'dxdd-routed-connector-selection';
        this.showArrow = external_ko_namespaceObject.observable(false);
        this.showRightArrow = external_ko_namespaceObject.observable(false);
        this.isVisible = external_ko_namespaceObject.observable(true);
        this.routePoints = external_ko_namespaceObject.observableArray();
        this.routePointsSet = external_ko_namespaceObject.pureComputed(() => {
            const points = [];
            this.routePoints().forEach((point) => {
                points.push(point.x() + ' ' + point.y());
            });
            return points.join(', ');
        });
        this.routeLineWrappers = external_ko_namespaceObject.pureComputed(() => {
            const result = [];
            for (let i = 1; i < this.routePoints().length; i++) {
                result.push(this._createRouteLineWrapper(this.routePoints()[i - 1], this.routePoints()[i], i === 1 || i === this.routePoints().length - 1));
            }
            return result;
        });
        this.connectorID = () => this._connectorID;
        this._disposables.push(control);
        this._connectorID = RoutedConnectorSurface._connectorsCount++;
        this.startPoint = external_ko_namespaceObject.pureComputed(() => {
            return new ConnectionPointSurface(control.startPoint(), context);
        });
        this.endPoint = external_ko_namespaceObject.pureComputed(() => {
            return new ConnectionPointSurface(control.endPoint(), context);
        });
        this._disposables.push(control.routePoints.subscribe((routePoints) => {
            this._updateRoutePoints();
        }));
        this._updateRoutePoints();
    }
}
RoutedConnectorSurface._connectorsCount = 0;

;// ./dist/diagram/controlRegistrator.js


















function registerControls() {
    diagramControlsFactory.registerControl('Unknown', {
        info: unknownSerializationsInfo,
        type: external_DevExpress_Analytics_Elements_namespaceObject.ElementViewModel,
        nonToolboxItem: true,
        surfaceType: external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase
    });
    diagramControlsFactory.registerControl('Connector', {
        info: [
            metadata_name,
            { propertyName: 'location', displayName: 'Location', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('objecteditor') },
            { propertyName: 'startPoint', modelName: '@StartPoint', link: true },
            { propertyName: 'endPoint', modelName: '@EndPoint', link: true }
        ],
        surfaceType: ConnectorSurface,
        type: ConnectorViewModel,
        elementActionsTypes: [],
        nonToolboxItem: false
    });
    diagramControlsFactory.registerControl('RoutedConnector', {
        info: [
            metadata_name,
            { propertyName: 'location', displayName: 'Location', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('objecteditor') },
            { propertyName: 'startPoint', modelName: '@StartPoint', link: true },
            { propertyName: 'endPoint', modelName: '@EndPoint', link: true }
        ],
        surfaceType: RoutedConnectorSurface,
        type: RoutedConnectorViewModel,
        elementActionsTypes: [],
        nonToolboxItem: false
    });
    diagramControlsFactory.registerControl('ConnectionPoint', {
        info: connectionPointSerializationInfo,
        surfaceType: ConnectionPointSurface,
        type: ConnectionPointViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
    diagramControlsFactory.registerControl('Diagram', {
        info: diagramSerializationsInfo,
        surfaceType: DiagramSurface,
        popularProperties: ['name'],
        type: DiagramViewModel,
        elementActionsTypes: [],
        isContainer: true,
        nonToolboxItem: true
    });
    diagramControlsFactory.registerControl('DiagramElement', {
        info: diagramElementSerializationInfo,
        defaultVal: {
            '@SizeF': '150,50',
            'ConnectingPoints': {
                'Item1': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '1',
                    '@PercentOffsetY': '0.5',
                },
                'Item2': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '1',
                },
                'Item3': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '0',
                },
                'Item4': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0',
                    '@PercentOffsetY': '0.5',
                }
            }
        },
        surfaceType: DiagramElementSurface,
        popularProperties: ['text'],
        type: DiagramElementViewModel,
        elementActionsTypes: [],
        nonToolboxItem: false
    });
    diagramControlsFactory.registerControl('Ellipse', {
        info: diagramElementSerializationInfo,
        defaultVal: {
            '@SizeF': '150,50',
            '@Type': 'Ellipse',
            'ConnectingPoints': {
                'Item1': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '1',
                    '@PercentOffsetY': '0.5',
                },
                'Item2': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '1',
                },
                'Item3': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '0',
                },
                'Item4': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0',
                    '@PercentOffsetY': '0.5',
                }
            }
        },
        surfaceType: DiagramElementSurface,
        popularProperties: ['text'],
        type: DiagramElementViewModel,
        elementActionsTypes: [],
        nonToolboxItem: false
    });
    diagramControlsFactory.registerControl('Condition', {
        info: diagramElementSerializationInfo,
        defaultVal: {
            '@SizeF': '150,50',
            '@Type': 'Condition',
            'ConnectingPoints': {
                'Item1': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '1',
                    '@PercentOffsetY': '0.5',
                },
                'Item2': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '1',
                },
                'Item3': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0.5',
                    '@PercentOffsetY': '0',
                },
                'Item4': {
                    '@ControlType': 'ConnectingPoint',
                    '@PercentOffsetX': '0',
                    '@PercentOffsetY': '0.5',
                }
            }
        },
        surfaceType: DiagramElementSurface,
        popularProperties: ['text'],
        type: DiagramElementViewModel,
        elementActionsTypes: [],
        nonToolboxItem: false
    });
    diagramControlsFactory.registerControl('ConnectingPoint', {
        info: connectingPointSerializationInfo,
        surfaceType: ConnectingPointSurface,
        type: ConnectingPointViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
}

;// ./dist/diagram/initializer.js













const groups = {
    'Appearance': { info: [] },
    'Behavior': { info: [] },
    'Design': { info: [metadata_name] },
    'Layout': { info: [metadata_location, size, pageWidth, pageHeight] }
};
function createDiagramDesigner(element, diagramSource, localization, rtl) {
    if (localization) {
        (0,external_DevExpress_Analytics_Utils_namespaceObject.addCultureInfo)({
            messages: localization
        });
    }
    registerControls();
    const diagram = external_ko_namespaceObject.pureComputed(() => { return new DiagramViewModel(diagramSource()); }), surface = external_ko_namespaceObject.pureComputed(() => {
        const surface = new DiagramSurface(diagram());
        return surface;
    });
    const designerModel = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDesigner)(diagram, surface, diagramControlsFactory, groups, undefined, undefined, rtl);
    designerModel.connectionPointDragHandler = new ConnectionPointDragHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.connectingPointDragHandler = new ConnectingPointDragHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.isLoading(false);
    designerModel.selection.focused(surface());
    (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).empty();
    external_ko_namespaceObject.applyBindings(designerModel, element);
    const updateSurfaceContentSize_ = (0,external_DevExpress_Analytics_Internal_namespaceObject.updateSurfaceContentSize)(designerModel.surfaceSize, element);
    const onResize = () => {
        updateSurfaceContentSize_();
    };
    window.addEventListener('resize', onResize);
    (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(element, function () {
        window.removeEventListener('resize', onResize);
    });
    designerModel.tabPanel.width.subscribe(() => {
        updateSurfaceContentSize_();
    });
    updateSurfaceContentSize_();
    return designerModel;
}

;// ./dist/diagram/bindings.js






external_ko_namespaceObject.bindingHandlers['routeLineDraggable'] = {
    init: (element, valueAccessor) => {
        let startDragPosition = null;
        const values = valueAccessor(), options = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ snap: '.dxrd-drag-snap-line', snapTolerance: external_DevExpress_Analytics_Internal_namespaceObject.SnapLinesHelper.snapTolerance }, external_ko_namespaceObject.unwrap(values), {
            start: function (event, uiElement) {
                values.starting();
            },
            stop: function (event, uiElement) {
                values.stopped();
            },
            drag: function (event, uiElement) {
                startDragPosition = startDragPosition || { left: (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.leftPosition), top: (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.topPosition) };
                const dragDeltaLeft = startDragPosition && event.pageX - startDragPosition.left;
                const dragDeltaTop = startDragPosition && event.pageY - startDragPosition.top;
                values.forceResize({ delta: { dx: dragDeltaLeft || 0, dy: dragDeltaTop || 0 } });
            }
        });
        const draggable = new external_DevExpress_Analytics_Internal_namespaceObject.Draggable(element, options);
        (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(element, () => {
            draggable.dispose();
            element = null;
        });
    }
};

;// ./dist/analytics-diagram.js















































;// ./dist/query-builder/dataSource/dbColumn.js

var DBColumnType;
(function (DBColumnType) {
    DBColumnType[DBColumnType["Unknown"] = 0] = "Unknown";
    DBColumnType[DBColumnType["Boolean"] = 1] = "Boolean";
    DBColumnType[DBColumnType["Byte"] = 2] = "Byte";
    DBColumnType[DBColumnType["SByte"] = 3] = "SByte";
    DBColumnType[DBColumnType["Char"] = 4] = "Char";
    DBColumnType[DBColumnType["Decimal"] = 5] = "Decimal";
    DBColumnType[DBColumnType["Double"] = 6] = "Double";
    DBColumnType[DBColumnType["Single"] = 7] = "Single";
    DBColumnType[DBColumnType["Int32"] = 8] = "Int32";
    DBColumnType[DBColumnType["UInt32"] = 9] = "UInt32";
    DBColumnType[DBColumnType["Int16"] = 10] = "Int16";
    DBColumnType[DBColumnType["UInt16"] = 11] = "UInt16";
    DBColumnType[DBColumnType["Int64"] = 12] = "Int64";
    DBColumnType[DBColumnType["UInt64"] = 13] = "UInt64";
    DBColumnType[DBColumnType["String"] = 14] = "String";
    DBColumnType[DBColumnType["DateTime"] = 15] = "DateTime";
    DBColumnType[DBColumnType["Guid"] = 16] = "Guid";
    DBColumnType[DBColumnType["TimeSpan"] = 17] = "TimeSpan";
    DBColumnType[DBColumnType["ByteArray"] = 18] = "ByteArray";
    DBColumnType[DBColumnType["DateOnly"] = 19] = "DateOnly";
    DBColumnType[DBColumnType["TimeOnly"] = 20] = "TimeOnly";
})(DBColumnType || (DBColumnType = {}));
class DBColumn {
    constructor(model) {
        this.name = model['Name'];
        this.type = model['ColumnType'];
        this.size = model['Size'];
    }
    static GetType(dbColumnType) {
        switch (dbColumnType) {
            case DBColumnType.Boolean:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBoolean;
            case DBColumnType.Byte:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemByte;
            case DBColumnType.SByte:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSByte;
            case DBColumnType.Char:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemChar;
            case DBColumnType.Decimal:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDecimal;
            case DBColumnType.Double:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDouble;
            case DBColumnType.Single:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSingle;
            case DBColumnType.Int32:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32;
            case DBColumnType.UInt32:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt32;
            case DBColumnType.Int16:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt16;
            case DBColumnType.UInt16:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt16;
            case DBColumnType.Int64:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt64;
            case DBColumnType.UInt64:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt64;
            case DBColumnType.String:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString;
            case DBColumnType.DateTime:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateTime;
            case DBColumnType.Guid:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemGuid;
            case DBColumnType.TimeSpan:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeSpan;
            case DBColumnType.ByteArray:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBiteArray;
            case DBColumnType.DateOnly:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateOnly;
            case DBColumnType.TimeOnly:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeOnly;
            default:
                return external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemObject;
        }
    }
    static GetSpecific(type) {
        const nullablePattern = /^System\.Nullable[^[]*\[(.+)\]$/;
        const nullableMatch = type.match(nullablePattern);
        if (nullableMatch) {
            type = nullableMatch[1];
        }
        switch (type) {
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBoolean:
                return 'Bool';
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemByte:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSByte:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt16:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt16:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt32:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt64:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt64:
                return 'Integer';
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemChar:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemGuid:
            case 'System.ByteArray':
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString:
                return 'String';
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDouble:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSingle:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDecimal:
                return 'Float';
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateTime:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeSpan:
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateOnly:
                return 'Date';
            case external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeOnly:
                return 'Time';
            default:
                return 'String';
        }
    }
}

;// ./dist/query-builder/dataSource/dbForeignKey.js
class DBForeignKey {
    constructor(model) {
        this.name = model['Name'];
        this.primaryKeyTable = model['PrimaryKeyTable'];
        this.columns = model['Columns'];
        this.primaryKeyColumns = model['PrimaryKeyTableKeyColumns'];
    }
}

;// ./dist/query-builder/dataSource/_dbSchema.js
function deserializeToCollection(model, createItem, _collection) {
    const collection = _collection || [];
    if (model) {
        model.forEach((value) => {
            collection.push(createItem(value));
        });
    }
    return collection;
}

;// ./dist/query-builder/dataSource/dbTable.js



class DBTable {
    constructor(model) {
        this.name = model['Name'];
        this.isView = model['IsView'] === 'true' || model['IsView'] === true;
        this.columns = deserializeToCollection(model['columns'], (columnModel) => new DBColumn(columnModel));
        this.foreignKeys = deserializeToCollection(model['foreignKeys'], (columnModel) => new DBForeignKey(columnModel));
    }
}

;// ./dist/query-builder/dataSource/dbStoredProcedure.js

class DBStoredProcedure {
    constructor(model) {
        this.name = model['Name'];
        this.arguments = deserializeToCollection(model['arguments'], (argModel) => new DBStoredProcedureArgument(argModel));
    }
}
var DBStoredProcedureArgumentDirection;
(function (DBStoredProcedureArgumentDirection) {
    DBStoredProcedureArgumentDirection[DBStoredProcedureArgumentDirection["In"] = 0] = "In";
    DBStoredProcedureArgumentDirection[DBStoredProcedureArgumentDirection["Out"] = 1] = "Out";
    DBStoredProcedureArgumentDirection[DBStoredProcedureArgumentDirection["InOut"] = 2] = "InOut";
})(DBStoredProcedureArgumentDirection || (DBStoredProcedureArgumentDirection = {}));
class DBStoredProcedureArgument {
    constructor(model) {
        this.name = model['Name'];
        this.type = model['Type'];
        this.direction = model['Direction'];
    }
}

;// ./dist/query-builder/dataSource/dbSchema.js



class DBSchema {
    constructor(model) {
        const tables = deserializeToCollection(model['Tables'], (tableModel) => new DBTable(tableModel));
        tables.sort((a, b) => { return a.name.localeCompare(b.name); });
        const views = deserializeToCollection(model['Views'], (tableModel) => new DBTable(tableModel));
        views.sort((a, b) => { return a.name.localeCompare(b.name); });
        this.tables = tables.concat(views);
        this.procedures = deserializeToCollection(model['StoredProcedures'], (procModel) => new DBStoredProcedure(procModel));
    }
    assignTablesAndViews(tables, views) {
        this.tables = tables.concat(views);
    }
}

;// ./dist/query-builder/dataSource/_dbSchemaProvider.js





function getDBSchemaCallback({ requestWrapper, connection, tables, getTables, getViews }) {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    requestWrapper.getDbSchema({ connection, tables, getTables, getViews })
        .done(data => {
        deferred.resolve(new DBSchema(JSON.parse(data.dbSchemaJSON)));
    })
        .fail(data => {
        (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Schema loading failed. {0}', 'DxDesignerStringId.Error_SchemaLoadingFailed'), (0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data)), 'error', 5000);
        deferred.reject();
    });
    return deferred.promise();
}
function getDBStoredProceduresCallback(requestWrapper, connection) {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    requestWrapper.getDbStoredProcedures(connection)
        .done(data => {
        deferred.resolve(new DBSchema(JSON.parse(data.dbSchemaJSON)).procedures);
    })
        .fail(data => {
        (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Stored procedures loading failed. {0}', 'DxDesignerStringId.Error_SchemaLoadingFailed'), (0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data)), 'error', 5000);
        deferred.reject();
    });
    return deferred.promise();
}

;// ./dist/query-builder/initializer.js

const ActionId = {
    Save: 'dxqb-save',
    DataPreview: 'dxqb-data-preview',
    SelectStatementPreview: 'dxqb-select-statement-preview'
};
const HandlerUri = (0,external_DevExpress_Analytics_Internal_namespaceObject.createGlobalModuleVariableFunc)('DXQB.axd');

;// ./dist/query-builder/utils/_dataConnection.js


function serializeDataConnection(connection) {
    const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
    const data = { 'DataConnection': serializer.serialize(connection) };
    if (!!connection.options)
        (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(data, { 'ConnectionOptions': serializer.serialize(connection.options) });
    return JSON.stringify(data);
}

;// ./dist/query-builder/utils/requestwrapper.js





class RequestWrapper {
    sendRequest(action, arg) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.sendRequest)(HandlerUri(), action, arg);
    }
    _sendRequest(settings) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.sendRequest)(settings);
    }
    getDbSchema({ connection, tables, getViews, getTables }) {
        const requestModel = {
            connectionJSON: serializeDataConnection(connection),
            tables: null,
            views: null,
            getTables,
            getViews
        };
        if (tables && tables.length > 0) {
            requestModel.tables = (tables || []).filter(x => !x.isView).map(x => x.name);
            requestModel.views = (tables || []).filter(x => x.isView).map(x => x.name);
        }
        return this.sendRequest('getDBSchema', (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)(requestModel));
    }
    getDbStoredProcedures(connection) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionJSON: serializeDataConnection(connection)
        });
        return this.sendRequest('getDBStoredProcedures', requestData);
    }
    getSelectStatement(connection, queryJSON) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionJSON: serializeDataConnection(connection),
            sqlQueryJSON: queryJSON
        });
        return this.sendRequest('getSelectStatement', requestData);
    }
    getDataPreview(connection, queryJSON) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionJSON: serializeDataConnection(connection),
            sqlQueryJSON: queryJSON
        });
        return this.sendRequest('getDataPreview', requestData);
    }
    rebuildResultSchema(dataSource, queryName, relationsEditing = false, parameters, dataSerializationExtension) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            sqlDataSourceJSON: JSON.stringify({ 'SqlDataSource': new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(dataSource) }),
            queryName: queryName,
            relationsEditing: relationsEditing,
            parameters: parameters,
            dataSerializationExtension: dataSerializationExtension
        });
        return this.sendRequest('rebuildResultSchema', requestData);
    }
    getFederationResultSchema(dataSource) {
        const serializedModel = dataSource.getSerializableModel().getSerializableFederationDataSourceInfo();
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            federationDataSourceJSON: JSON.stringify({ 'FederationDataSource': new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(dataSource) }),
            dataSources: serializedModel.dataSources
        });
        return this.sendRequest('getFederationResultSchema', requestData);
    }
    validateJsonUri(jsonDataSource) {
        const uriJsonSourceJSON = JSON.stringify(jsonDataSource.source.serialize(true));
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            uriJsonSourceJSON: uriJsonSourceJSON
        });
        const ajaxSettings = {
            uri: HandlerUri(),
            action: 'validateJsonEndPoint',
            arg: requestData,
            ignoreError: () => true
        };
        return this._sendRequest(ajaxSettings);
    }
    saveJsonSource(connectionName, jsonDataSource) {
        const jsonSource = jsonDataSource.source;
        const jsonSourceJSON = JSON.stringify(jsonSource.serialize(true));
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionName: connectionName,
            customJson: jsonSource.json(),
            uriJsonSourceJSON: jsonSourceJSON
        });
        return this.sendRequest('saveJsonSource', requestData);
    }
    getJsonSchema(jsonDataSource, parameters) {
        const jsonSource = jsonDataSource.source;
        const jsonSourceJSON = JSON.stringify(jsonSource.serialize(true));
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionName: jsonDataSource.connectionName(),
            customJson: jsonSource.json(),
            uriJsonSourceJSON: jsonSourceJSON,
            parameters: parameters
        });
        return this.sendRequest('getJsonSchema', requestData);
    }
    getMongoDBQuerySchema(mongoDBDataSource, query) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionJSON: serializeDataConnection(mongoDBDataSource.connection),
            databaseName: query.databaseName(),
            collectionName: query.collectionName()
        });
        return this.sendRequest('getMongoDBCollectionSchema', requestData);
    }
    getMongoDBCollections(connection) {
        const requestData = (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)({
            connectionJSON: serializeDataConnection(connection),
        });
        return this.sendRequest('getMongoDBCollections', requestData);
    }
    getObjectTypeDescriptions(context) {
        return this.sendRequest('getObjectSchema', context);
    }
}

;// ./dist/query-builder/dataSource/dbSchemaProvider.js










class DBSchemaProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _getDBSchema(tables, getViews, getTables) {
        return getDBSchemaCallback({
            requestWrapper: this._requestWrapper,
            connection: this.connection,
            tables,
            getTables,
            getViews
        });
    }
    _getDBStoredProcedures(connection) {
        return getDBStoredProceduresCallback(this._requestWrapper, connection);
    }
    constructor(connection, _requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this._tables = {};
        this._tableRequests = external_ko_namespaceObject.observableArray([]).extend({ deferred: true });
        this.connection = connection;
        this._disposables.push(this.connection.name.subscribe(() => {
            this._tables = {};
            this._dbSchema = null;
            this._dbStoredProceduresSchema = null;
        }));
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            const tableRequests = this._tableRequests();
            if (!tableRequests.length)
                return;
            this._tableRequests([]);
            const tables = tableRequests.map(x => x.table);
            this._getDBSchema(tables).done(dbSchema => {
                tableRequests.forEach(tableRequest => {
                    const schemaTable = dbSchema.tables.filter(x => x.name === tableRequest.table.name)[0];
                    if (!schemaTable) {
                        tableRequest.deferred.reject();
                        return;
                    }
                    tableRequest.table.columns = schemaTable.columns;
                    tableRequest.deferred.resolve(tableRequest.table);
                });
            }).fail(() => tableRequests.forEach(request => request.deferred.reject()));
        }));
        this.getItems = (pathRequest) => {
            const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (!pathRequest.fullPath) {
                this.getDbSchema().done((dbSchema) => {
                    deferred.resolve((dbSchema.tables || []).map((item) => {
                        const dataMemberInfo = {
                            name: item.name,
                            displayName: item.name,
                            isList: false,
                            specifics: item.isView ? 'view' : 'table',
                            dragData: { noDragable: false }
                        };
                        return dataMemberInfo;
                    }));
                });
            }
            else {
                deferred.resolve([]);
            }
            return deferred.promise();
        };
    }
    getDbViews() {
        if (!this._dbViewsSchema || this._dbViewsSchema.state() === 'rejected')
            this._dbViewsSchema = this._getDBSchema(undefined, true, false);
        return this._dbViewsSchema;
    }
    getDbTables() {
        if (!this._dbTablesSchema || this._dbTablesSchema.state() === 'rejected')
            this._dbTablesSchema = this._getDBSchema(undefined, false, true);
        return this._dbTablesSchema;
    }
    getDbSchema() {
        if (!this._dbSchema || this._dbSchema.state() === 'rejected') {
            const _dbSchemaDef = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            this._dbSchema = _dbSchemaDef;
            external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when([this.getDbTables(), this.getDbViews()]).done((results) => {
                const dbSchema = new DBSchema({});
                dbSchema.assignTablesAndViews(results[0].tables, results[1].tables);
                _dbSchemaDef.resolve(dbSchema);
            }).fail(() => _dbSchemaDef.reject());
        }
        return this._dbSchema.promise();
    }
    getDbStoredProcedures() {
        if (!this._dbStoredProceduresSchema || this._dbStoredProceduresSchema.state() === 'rejected')
            this._dbStoredProceduresSchema = this._getDBStoredProcedures(this.connection);
        return this._dbStoredProceduresSchema;
    }
    getDbTable(tableName, fullPath = '') {
        let schemaRequest = this.getDbSchema;
        if (fullPath.startsWith('tables'))
            schemaRequest = this.getDbTables;
        else if (fullPath.startsWith('views'))
            schemaRequest = this.getDbViews;
        if (!this._tables[tableName]) {
            const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            this._tables[tableName] = deferred.promise();
            schemaRequest.call(this).done((dbSchema) => {
                const table = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(dbSchema.tables, table => table.name === tableName);
                if (!table) {
                    deferred.reject();
                    (0,external_DevExpress_Analytics_Internal_namespaceObject.isCustomizedWithUpdateLocalizationMethod)('The schema does not contain the specified table: ') ?
                        (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('The schema does not contain the specified table: ') + "'" + tableName + "'." :
                        (0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('The schema does not contain the specified table: "{0}".', 'DataAccessStringId.TableNotInSchemaValidationException'), tableName);
                }
                else if (table.columns.length > 0) {
                    deferred.resolve(table);
                }
                else {
                    this._tableRequests.push({ table: table, deferred: deferred });
                }
            }).fail(() => deferred.reject());
        }
        return this._tables[tableName];
    }
}

;// ./dist/query-builder/dataSource/resultTable.js

const resultTableSerializationInfo = [
    { propertyName: 'tableName', modelName: '@Name' },
    {
        propertyName: 'columns', modelName: 'Fields', array: true, info: [
            { propertyName: 'name', modelName: '@Name' },
            { propertyName: 'propertyType', modelName: '@Type' }
        ]
    }
];
class ResultTable {
    getInfo() {
        return resultTableSerializationInfo;
    }
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
    }
}

;// ./dist/query-builder/dataSource/resultSet.js



const resultSetSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'tables', modelName: 'Views', array: true }
];
class ResultSet {
    getInfo() {
        return resultSetSerializationInfo;
    }
    static from(model, serializer) {
        return model && new ResultSet(model['DataSet'], serializer) || null;
    }
    static toJson(value, serializer, refs) {
        return { 'DataSet': serializer.serialize(value, resultSetSerializationInfo, refs) };
    }
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this.tables = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model && model['Views'] || [], (item) => {
            return new ResultTable(item, serializer);
        });
    }
}

;// ./dist/query-builder/dataSource/json/jsonSchemaNode.js





var JsonNodeType;
(function (JsonNodeType) {
    JsonNodeType[JsonNodeType["Object"] = 0] = "Object";
    JsonNodeType[JsonNodeType["Array"] = 1] = "Array";
    JsonNodeType[JsonNodeType["Property"] = 2] = "Property";
})(JsonNodeType || (JsonNodeType = {}));
class JsonNode {
    static from(model, serializer) {
        return new JsonNode(model, serializer);
    }
    static toJsonNodes(value, serializer, refs) {
        return (value || []).map(item => JsonNode.toJsonNode(item, serializer, refs));
    }
    static toJsonNode(value, serializer, refs, recoursive = true) {
        const obj = serializer.serialize(value, jsonSchemaNodeSerializationInfo, refs);
        const nodes = recoursive ? JsonNode.toJsonNodes(value.nodes, serializer, refs) : [];
        if (nodes.length > 0)
            obj['Node'] = nodes;
        return obj;
    }
    getInfo() {
        return jsonSchemaNodeSerializationInfo;
    }
    constructor(model, serializer) {
        this.nodes = [];
        if (!model)
            return;
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        const innerNodes = model['Node'];
        const currentNodes = innerNodes instanceof Array ?
            deserializeToCollection(innerNodes, (nodeModel) => new JsonNode(nodeModel)).sort((a, b) => { return a.name().localeCompare(b.name()); })
            : !innerNodes ? [] : [new JsonNode(innerNodes)];
        this.nodes = currentNodes;
    }
}
class JsonSchemaNode extends JsonNode {
    static from(model, serializer) {
        return new JsonSchemaNode(model || {}, serializer);
    }
    static toJson(value, serializer, refs) {
        if (!value)
            return {};
        const obj = { Node: JsonNode.toJsonNode(value, serializer, refs, false) };
        obj.Node['Node'] = JsonNode.toJsonNodes(value.nodes, serializer, refs);
        return obj;
    }
    getInfo() {
        return jsonSchemaNodeSerializationInfo;
    }
    constructor(model, serializer) {
        super(model['Node'], serializer);
        this.nodeType = JsonNodeType[JsonNodeType.Object];
        this.valueType = 'Unknown';
        this.displayName = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('root');
        this.selected = external_ko_namespaceObject.observable(false);
    }
}
class JsonSchemaRootNode extends JsonNode {
    static from(model, serializer) {
        return new JsonSchemaRootNode(model, serializer);
    }
    static toJson(value, serializer, refs) {
        return JsonSchemaNode.toJson(value.nodes[0], serializer, refs);
    }
    getInfo() {
        return jsonSchemaRootNodeSerializationInfo;
    }
    constructor(model, serializer) {
        super(model);
        this._rootElementList = null;
    }
    getRootElementPartList(allowObjectRootElements = true) {
        if (this._rootElementList)
            return this._rootElementList;
        if (this.nodes.length === 0)
            return [];
        this._rootElementList = [];
        const node = this.nodes[0];
        const currentPath = {
            fullPath: external_ko_namespaceObject.unwrap(node.name),
            path: '',
            pathParts: [external_ko_namespaceObject.unwrap(node.name)]
        };
        this._fillRootElementList(node, currentPath, allowObjectRootElements);
        this._rootElementList.sort((a, b) => { return a.fullPath.localeCompare(b.fullPath); });
        return this._rootElementList;
    }
    _fillRootElementList(node = this.nodes[0], currentPath = { fullPath: 'root', path: '', pathParts: ['root'] }, allowObjectRootElements) {
        if (!node)
            return this._rootElementList;
        const nodeType = external_ko_namespaceObject.unwrap(node.nodeType);
        if (nodeType == JsonNodeType[JsonNodeType.Property])
            return;
        if (nodeType === JsonNodeType[JsonNodeType.Array]) {
            this._rootElementList.push(currentPath);
            return;
        }
        if ((allowObjectRootElements && nodeType === JsonNodeType[JsonNodeType.Object])) {
            this._rootElementList.push(currentPath);
        }
        (node.nodes || []).forEach(x => {
            const nextPath = this._getNextPath(currentPath, x.name());
            this._fillRootElementList(x, nextPath, allowObjectRootElements);
        });
        return this._rootElementList;
    }
    _getNextPath(currentPath, nodeName) {
        const _nodeName = !currentPath.fullPath ? nodeName : [currentPath.fullPath, nodeName].join('.');
        return {
            pathParts: currentPath.pathParts.concat(nodeName),
            fullPath: _nodeName,
            path: nodeName
        };
    }
}
const jsonSchemaNodeSerializationInfo = [
    { propertyName: 'nodes', modelName: 'Node', from: JsonNode.from, toJsonObject: JsonNode.toJsonNodes },
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'selected', modelName: '@Selected', from: external_DevExpress_Analytics_Utils_namespaceObject.parseBool },
    { propertyName: 'nodeType', modelName: '@NodeType' },
    { propertyName: 'type', modelName: '@Type' }
];
const jsonSchemaRootNodeSerializationInfo = [
    { propertyName: 'nodes', modelName: 'Node', from: JsonSchemaRootNode.from, toJsonObject: JsonSchemaRootNode.toJsonNodes }
];

;// ./dist/query-builder/dataSource/json/jsonAuthenticationInfo.js

class JsonAuthenticationInfo {
    static from(model, serializer) {
        return new JsonAuthenticationInfo(model, serializer);
    }
    static toJson(value, serializer, refs) {
        return serializer.serialize(value, value.getInfo(), refs);
    }
    getInfo() {
        return [
            { propertyName: 'password', modelName: '@password', defaultVal: '' },
            { propertyName: 'userName', modelName: '@user', defaultVal: '' }
        ];
    }
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model || {});
    }
}

;// ./dist/query-builder/dataSource/json/jsonParameter.js





var JsonParameterType;
(function (JsonParameterType) {
    JsonParameterType[JsonParameterType["PathParameter"] = 0] = "PathParameter";
    JsonParameterType[JsonParameterType["QueryParameter"] = 1] = "QueryParameter";
    JsonParameterType[JsonParameterType["Header"] = 2] = "Header";
})(JsonParameterType || (JsonParameterType = {}));
class JsonParameter extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    static from(model, serializer) {
        return new JsonParameter(model || {}, serializer);
    }
    static toJson(value, serializer, refs) {
        return serializer.serialize(value, value.getInfo(), refs);
    }
    _initEditingProperties() {
        const currentValue = this.value() || '';
        if (currentValue.indexOf(this.expression_Prefix) === 0) {
            this.isExpression(true);
            this._expression.value(currentValue.substring(this.expression_Prefix.length));
        }
        else {
            this._editingValue(currentValue);
        }
    }
    switchEditors() {
        const newIsExpression = !this.isExpression();
        this.isExpression(newIsExpression);
        if (newIsExpression) {
            this._expression.value(this._editingValue() ? "'" + this._editingValue() + "'" : '');
        }
        else {
            this._editingValue('');
        }
    }
    getInfo() {
        return [
            { propertyName: 'name', modelName: '@Name', displayName: 'Name', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text') },
            { propertyName: 'value', modelName: '@Value', displayName: 'Value', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text') },
            { propertyName: 'itemType', modelName: '@ItemType', defaultVal: JsonParameterType[JsonParameterType.PathParameter], alwaysSerialize: true }
        ];
    }
    constructor(model, serializer) {
        super();
        this.expression_Prefix = 'expression:';
        this.namePlaceholder = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Name', 'AnalyticsCoreStringId.CollectionEditor_Name_Placeholder');
        this.valuePlaceholder = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Value', 'AnalyticsCoreStringId.CollectionEditor_Value_Placeholder');
        this._editingValue = external_ko_namespaceObject.observable('');
        this._expression = { value: external_ko_namespaceObject.observable('') };
        this.isExpression = external_ko_namespaceObject.observable(false);
        this.itemsProvider = null;
        this._parameterTypes = parameterTypes;
        this.nameValidationRules = undefined;
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this._initEditingProperties();
        this._disposables.push(this.value = external_ko_namespaceObject.computed(() => {
            const expressionValue = (this.expression_Prefix + this._expression.value());
            const editingValue = this._editingValue();
            return this.isExpression.peek() ? expressionValue : editingValue;
        }));
    }
}
const parameterTypes = [
    { value: JsonParameterType[JsonParameterType.PathParameter], displayValue: 'Path Parameter', localizationId: 'AnalyticsCoreStringId.JsonDSWizard_ChooseJsonSourcePage_PathParameter' },
    { value: JsonParameterType[JsonParameterType.QueryParameter], displayValue: 'Query Parameter', localizationId: 'AnalyticsCoreStringId.JsonDSWizard_ChooseJsonSourcePage_QueryParameter' },
    { value: JsonParameterType[JsonParameterType.Header], displayValue: 'Header', localizationId: 'AnalyticsCoreStringId.JsonDSWizard_ChooseJsonSourcePage_Header' }
];

;// ./dist/query-builder/dataSource/json/jsonSource.js






class JsonSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    static from(model, serializer) {
        return new JsonSource(model || {}, serializer);
    }
    static toJson(value, serializer, refs) {
        return serializer.serialize(value, jsonSourceSerializationInfo, refs);
    }
    getInfo() {
        return jsonSourceSerializationInfo;
    }
    constructor(model = {}, serializer) {
        super();
        this.sourceType = external_ko_namespaceObject.observable();
        this.uri = external_ko_namespaceObject.observable();
        this.json = external_ko_namespaceObject.observable();
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this._disposables.push(this.uri.subscribe((newUri) => {
            newUri && this.sourceType(JsonSource._URIJSONSOURCE_TYPE);
        }));
        this._disposables.push(this.json.subscribe((newJsonString) => {
            newJsonString && this.sourceType(JsonSource._CUSTOMJSONSOURCE_TYPE);
        }));
        function _getJsonParametersModelDeserialized(parameterModels, itemType) {
            if (parameterModels && Array.isArray(parameterModels[JsonParameterType[itemType]]) && parameterModels[JsonParameterType[itemType]])
                parameterModels = parameterModels[JsonParameterType[itemType]];
            return (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(parameterModels, (item) => {
                if (!item['@ItemType'])
                    item['@ItemType'] = JsonParameterType[itemType];
                return new JsonParameter(item, serializer);
            });
        }
        this.queryParameters = _getJsonParametersModelDeserialized(model['QueryParameters'], JsonParameterType.QueryParameter);
        this.headers = _getJsonParametersModelDeserialized(model['Headers'], JsonParameterType.Header);
        this.pathParameters = _getJsonParametersModelDeserialized(model['PathParameters'], JsonParameterType.PathParameter);
    }
    serialize(includeRootTag = false) {
        return includeRootTag ? { 'Source': this.serialize() } : (new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).serialize(this);
    }
    resetSource() {
        this.sourceType('');
        this.json('');
        this.uri('');
    }
}
JsonSource._URIJSONSOURCE_TYPE = 'DevExpress.DataAccess.Json.UriJsonSource';
JsonSource._CUSTOMJSONSOURCE_TYPE = 'DevExpress.DataAccess.Json.CustomJsonSource';
const jsonSourceSerializationInfo = [
    { propertyName: 'sourceType', modelName: '@SourceType', defaultVal: '' },
    { propertyName: 'json', modelName: '@Json', defaultVal: '' },
    { propertyName: 'uri', modelName: '@Uri', defaultVal: '' },
    { propertyName: 'authenticationInfo', modelName: 'AuthenticationInfo', from: JsonAuthenticationInfo.from, toJsonObject: JsonAuthenticationInfo.toJson },
    { propertyName: 'headers', modelName: 'Headers', array: true },
    { propertyName: 'queryParameters', modelName: 'QueryParameters', array: true },
    { propertyName: 'pathParameters', modelName: 'PathParameters', array: true }
];

;// ./dist/query-builder/dataSource/json/_jsonSchemaProvider.js





function _processJsonSchemaResponse(schema) {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    schema.done(data => {
        try {
            const jsonSchema = JSON.parse(data);
            const jsonSchemaModel = new JsonSchemaRootNode(jsonSchema);
            deferred.resolve(jsonSchemaModel);
        }
        finally {
            if (deferred.state() === 'pending')
                deferred.reject();
        }
    }).fail(data => {
        (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Schema loading failed. {0}', 'DxDesignerStringId.Error_SchemaLoadingFailed'), (0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data)));
        deferred.reject();
    });
    return deferred.promise();
}
const _getJsonSchemaCallback = (requestWrapper, jsonDataSource, parameters = []) => {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    requestWrapper.getJsonSchema(jsonDataSource, parameters)
        .done(data => deferred.resolve(data.jsonSchemaJSON))
        .fail(reason => deferred.reject(reason));
    return _processJsonSchemaResponse(deferred.promise());
};
let getJsonSchemaCallback = _getJsonSchemaCallback;
function _setGetJsonSchemaCallback(func) { getJsonSchemaCallback = func; }
function _resetGetJsonSchemaCallback() { getJsonSchemaCallback = _getJsonSchemaCallback; }

;// ./dist/query-builder/dataSource/json/jsonSchemaProvider.js




class JsonSchemaProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(jsonDataSource, _requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this._jsonDataSource = jsonDataSource;
        this._disposables.push(this._jsonDataSource.source.sourceType.subscribe(() => {
            this._jsonSchemaPromise = null;
        }));
        this.getItems = (pathRequest) => {
            const getItemsDeferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            const loadSchemaPromise = !this._jsonSchema ? this.getJsonSchema() : new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve(this._jsonSchema).promise();
            loadSchemaPromise
                .done((jsonSchema) => {
                this._jsonSchema = jsonSchema;
                const schemaByPath = this.getSchemaByPath(pathRequest, jsonSchema);
                getItemsDeferred.resolve(schemaByPath);
            })
                .fail(getItemsDeferred.reject);
            return getItemsDeferred.promise();
        };
    }
    reset() {
        this._jsonSchemaPromise = null;
    }
    mapToDataMemberContract(nodes) {
        return (nodes || []).map((node) => {
            const dataMemberInfo = {
                name: node.name(),
                displayName: node.displayName || node.name(),
                isSelected: node.selected(),
                isList: node.nodes && node.nodes.length > 0,
                specifics: 'table',
                dragData: { noDragable: false }
            };
            return dataMemberInfo;
        });
    }
    getSchemaByPath(pathRequest, jsonSchema) {
        if (!pathRequest.fullPath) {
            return this.mapToDataMemberContract(jsonSchema.nodes);
        }
        else {
            let currentNodes = jsonSchema.nodes;
            for (let i = 0; i < pathRequest.pathParts.length; i++) {
                const pathPart = (currentNodes || []).filter((node) => node.name() == pathRequest.pathParts[i])[0];
                if (!pathPart)
                    return [];
                currentNodes = pathPart.nodes;
            }
            return this.mapToDataMemberContract(currentNodes);
        }
    }
    getJsonSchema(parameters = []) {
        if (!this._jsonSchemaPromise || this._jsonSchemaPromise.state() === 'rejected')
            this._jsonSchemaPromise = getJsonSchemaCallback(this._requestWrapper, this._jsonDataSource, parameters);
        return this._jsonSchemaPromise;
    }
}

;// ./dist/query-builder/dataSource/json/jsonDataSource.js







class JsonDataSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    getInfo() {
        return jsonDataSourceSerializationInfo;
    }
    clone(_serializer) {
        const serializer = _serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        const serialized = serializer.serialize(this);
        return new JsonDataSource(serialized);
    }
    static from(model, serializer) {
        return new JsonDataSource(model, serializer);
    }
    static toJson(value, serializer, refs) {
        return serializer.serialize(value, jsonDataSourceSerializationInfo, refs);
    }
    constructor(model, serializer, requestWrapper = new RequestWrapper()) {
        super();
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this.jsonSchemaProvider = new JsonSchemaProvider(this, requestWrapper);
        this.source && this._disposables.push(this.source);
        this._disposables.push(this.connectionName.subscribe(() => {
            this.source.resetSource();
        }));
    }
    getSchema(parameters = []) {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        this.jsonSchemaProvider.getJsonSchema(parameters)
            .done((schema) => {
            this.schema = schema;
            deferred.resolve(schema);
        })
            .fail(() => {
            this.schema = null;
            deferred.reject();
        });
        return deferred.promise();
    }
}
const jsonDataSourceSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'connectionName', modelName: '@ConnectionName' },
    { propertyName: 'rootElement', modelName: '@RootElement', defaultVal: 'root' },
    { propertyName: 'schema', modelName: 'Schema', from: JsonSchemaRootNode.from, toJsonObject: JsonSchemaRootNode.toJson },
    { propertyName: 'source', modelName: 'Source', from: JsonSource.from, toJsonObject: JsonSource.toJson }
];

;// ./dist/query-builder/dataSource/mongoDB/_mongoDBSchemaProvider.js


const _getMongoDBSchemaCallback = (requestWrapper, dataSource, query) => {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    requestWrapper.getMongoDBQuerySchema(dataSource, query)
        .done(data => deferred.resolve(data.collectionSchemaJson))
        .fail(reason => deferred.reject(reason));
    return _processJsonSchemaResponse(deferred.promise());
};
let getMongoDBSchemaCallback = _getMongoDBSchemaCallback;
function _setGetMongoDBSchemaCallback(func) { getMongoDBSchemaCallback = func; }
function _resetGetMongoDBSchemaCallback() { getMongoDBSchemaCallback = _getMongoDBSchemaCallback; }

;// ./dist/query-builder/dataSource/mongoDB/mongoDBQuery.js




const mongoDBQuerySerializationInfo = [
    { propertyName: 'id', modelName: '@Id' },
    { propertyName: 'databaseName', modelName: '@DatabaseName' },
    { propertyName: 'collectionName', modelName: '@CollectionName' },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'filterString', modelName: '@FilterString' },
    { propertyName: 'schema', modelName: 'Schema', from: JsonSchemaRootNode.from, toJsonObject: JsonSchemaRootNode.toJson },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
class MongoDBQuery {
    getInfo() {
        return mongoDBQuerySerializationInfo;
    }
    static fromCollection(state) {
        const query = new MongoDBQuery();
        query.databaseName(state.databaseName);
        query.collectionName(state.collectionName);
        query.alias(state.alias);
        query.filterString(state.filterString);
        query.id(state.id);
        return query;
    }
    toCollection() {
        return {
            databaseName: this.databaseName(),
            collectionName: this.collectionName(),
            alias: this.alias(),
            filterString: this.filterString(),
            id: this.id(),
        };
    }
    get hash() {
        return this.databaseName() + '.' + this.collectionName();
    }
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({
            '@ItemType': 'Query',
            '@Id': (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)(),
        }, model));
    }
}

;// ./dist/query-builder/dataSource/mongoDB/mongoDBSchemaProvider.js

class MongoDBSchemaProvider {
    constructor(_dataSource, _requestWrapper) {
        this._dataSource = _dataSource;
        this._requestWrapper = _requestWrapper;
        this._schemaPromises = {};
    }
    getQuerySchema(query) {
        let promise = this._schemaPromises[query.hash];
        if (!promise || promise.state() === 'rejected') {
            promise = getMongoDBSchemaCallback(this._requestWrapper, this._dataSource, query);
            this._schemaPromises[query.hash] = promise;
        }
        return promise;
    }
}

;// ./dist/query-builder/dataSource/sql/dataConnectionBase.js



const dataConnectionSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'parameteres', modelName: 'Parameters' },
    { propertyName: 'fromAppConfig', modelName: '@FromAppConfig', defaultVal: false, from: external_DevExpress_Analytics_Utils_namespaceObject.parseBool }
];
class DataConnectionBase {
    getInfo() {
        return dataConnectionSerializationInfo;
    }
    constructor(model, serializer) {
        this.name = external_ko_namespaceObject.observable();
        this.parameteres = external_ko_namespaceObject.observable();
        this.fromAppConfig = external_ko_namespaceObject.observable(true);
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
    }
}

;// ./dist/query-builder/dataSource/mongoDB/mongoDBDataConnection.js


class MongoDBDataConnection extends DataConnectionBase {
    static from(json) {
        return new MongoDBDataConnection(json ? JSON.parse(json) : undefined);
    }
    serialize() {
        return JSON.stringify(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(this));
    }
}

;// ./dist/query-builder/dataSource/mongoDB/mongoDBDataSource.js








class MongoDBDataSource {
    getInfo() {
        return mongoDBDataSourceSerializationInfo;
    }
    saveQueries() {
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        return this.queries().map(x => JSON.stringify(serializer.serialize(x)));
    }
    constructor(model, serializer, _requestWrapper = new RequestWrapper()) {
        this._requestWrapper = _requestWrapper;
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this.queries = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model?.Queries, (item) => {
            return new MongoDBQuery(item, serializer);
        });
        this.connection = new MongoDBDataConnection(model?.Connection, serializer);
        this.schemaProvider = new MongoDBSchemaProvider(this, _requestWrapper);
    }
    getAvailableCollections() {
        return this._requestWrapper.getMongoDBCollections(this.connection);
    }
    getQuerySchema(query) {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        if (query.schema?.nodes?.length) {
            deferred.resolve(query.schema);
        }
        else {
            this.schemaProvider.getQuerySchema(query)
                .done((schema) => {
                query.schema = schema;
                deferred.resolve(schema);
            })
                .fail((reason) => {
                query.schema = null;
                deferred.reject(reason);
            });
        }
        return deferred.promise();
    }
    loadQueries() {
        const promises = this.queries().map((query) => {
            return this.getQuerySchema(query);
        });
        return external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when(promises).promise();
    }
}
const mongoDBDataSourceSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'connection', modelName: 'Connection' },
    { propertyName: 'queries', modelName: 'Queries', array: true },
];

;// ./dist/query-builder/dataSource/sql/connectionOptions.js



const connectionOptionsSerializationInfo = [
    { propertyName: 'closeConnection', modelName: '@CloseConnection', from: external_DevExpress_Analytics_Utils_namespaceObject.parseBool },
    { propertyName: 'commandTimeout', modelName: '@DbCommandTimeout', from: s => { let val = parseInt(s); if (isNaN(val))
            val = null; return external_ko_namespaceObject.observable(val); }, defaultVal: null },
];
class ConnectionOptions {
    getInfo() {
        return connectionOptionsSerializationInfo;
    }
    constructor(model, serializer) {
        this.closeConnection = external_ko_namespaceObject.observable(true);
        this.commandTimeout = external_ko_namespaceObject.observable(null);
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
    }
}

;// ./dist/query-builder/dataSource/utils.js
const SqlQueryType = {
    customSqlQuery: 'CustomSqlQuery',
    tableQuery: 'SelectQuery',
    storedProcQuery: 'StoredProcQuery'
};
const JsonSourceType = {
    fileJsonSource: 'FileJsonSource',
    customJsonSource: 'CustomJsonSource',
    uriJsonSource: 'UriJsonSource'
};
var FederationQueryType;
(function (FederationQueryType) {
    FederationQueryType[FederationQueryType["SelectNode"] = 0] = "SelectNode";
    FederationQueryType[FederationQueryType["UnionNode"] = 1] = "UnionNode";
    FederationQueryType[FederationQueryType["SourceNode"] = 2] = "SourceNode";
    FederationQueryType[FederationQueryType["TransformationNode"] = 3] = "TransformationNode";
})(FederationQueryType || (FederationQueryType = {}));

;// ./dist/query-builder/dataSource/dataSourceParameterMeta.js





function validateName(nameCandidate) {
    return nameCandidate && !nameCandidate.match(/[~`!"№;%\^:\?*\(\)&\-\+={}\[\]\|\\\/,\.<>'\s]/);
}
const dsParameterNameValidationRules = [{
        type: 'custom',
        validationCallback: (options) => { return validateName(options.value); },
        get message() {
            return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Name is required and should be a valid identifier.', 'AnalyticsCoreStringId.NameIsRequired_Error');
        }
    }];
const parameterValueSerializationsInfo = { propertyName: 'value', displayName: 'Value', localizationId: 'DevExpress.DataAccess.Parameter.Value', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text') };
const dsParameterName = { propertyName: 'name', displayName: 'Name', localizationId: 'DevExpress.DataAccess.Parameter.Name', validationRules: dsParameterNameValidationRules, editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text') };
const dsParameterType = {
    propertyName: 'type', displayName: 'Type', localizationId: 'DevExpress.DataAccess.Parameter.Type', modelName: '@Type', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('combobox'), valuesArray: [
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString, displayValue: 'String', localizationId: 'AnalyticsCoreStringId.Parameter_Type_String' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateTime, displayValue: 'Date and Time', localizationId: 'AnalyticsCoreStringId.Parameter_Type_DateTime' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateOnly, displayValue: 'Date', localizationId: 'AnalyticsCoreStringId.Parameter_Type_DateOnly' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeOnly, displayValue: 'Time', localizationId: 'AnalyticsCoreStringId.Parameter_Type_TimeOnly' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt16, displayValue: 'Number (16 bit integer)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Int16' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32, displayValue: 'Number (32 bit integer)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Int32' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt64, displayValue: 'Number (64 bit integer)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Int64' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSingle, displayValue: 'Number (floating-point)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Float' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDouble, displayValue: 'Number (double-precision floating-point)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Double' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDecimal, displayValue: 'Number (decimal)', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Decimal' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBoolean, displayValue: 'Boolean', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Boolean' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemGuid, displayValue: 'Guid', localizationId: 'AnalyticsCoreStringId.Parameter_Type_Guid' },
        { value: external_DevExpress_Analytics_Internal_namespaceObject.ExpressionType, displayValue: 'Expression', localizationId: 'DataAccessUIStringId.ParametersColumn_Expression' }
    ]
};
const dsExpressionResultType = {
    propertyName: 'resultType',
    displayName: 'Result Type',
    localizationId: 'DataAccessWebStringId.QueryBuilder_ResultType',
    modelName: '@ResultType',
    editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('combobox'),
    valuesArray: dsParameterType.valuesArray.filter(x => !(0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(x.value))
};
const baseDSParamterSerializationsInfo = [
    { propertyName: '_name', modelName: '@Name' },
    { propertyName: '_value', modelName: '#text' },
    parameterValueSerializationsInfo,
    { propertyName: 'itemType', modelName: '@ItemType' }
];
const dsParameterSerializationInfo = [dsParameterName, dsParameterType, dsExpressionResultType].concat(baseDSParamterSerializationsInfo);
const storedProcAdditionValuesArray = [
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemByte, displayValue: 'Non-negative number (8 bit integer)', localizationId: 'DataAccessStringId.Type_Byte' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSByte, displayValue: 'Number (8 bit integer)', localizationId: 'DataAccessStringId.Type_SByte' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt32, displayValue: 'Non-negative number (32 bit integer)', localizationId: 'DataAccessStringId.Type_UInt' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt16, displayValue: 'Non-negative number (16 bit integer)', localizationId: 'DataAccessStringId.Type_UShort' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt64, displayValue: 'Non-negative number (64 bit integer)', localizationId: 'DataAccessStringId.Type_ULong' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemChar, displayValue: 'Char', localizationId: 'DataAccessStringId.Type_Char' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemObject, displayValue: 'Object', localizationId: 'DataAccessStringId.Type_Object' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBiteArray, displayValue: 'Byte array', localizationId: 'DataAccessStringId.Type_ByteArray' },
    { value: external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeSpan, displayValue: 'Time interval', localizationId: 'DataAccessStringId.Type_TimeSpan' },
];
function storedProcParameterSerializationsInfo(type) {
    const copyParamType = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, dsParameterType);
    const newValuesArray = [];
    newValuesArray.push(dsParameterType.valuesArray.filter(item => item.value === type)[0] || storedProcAdditionValuesArray.filter(item => item.value === type)[0]);
    newValuesArray.push(dsParameterType.valuesArray.filter(item => (0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(item.value))[0]);
    copyParamType.valuesArray = newValuesArray;
    const copyResultType = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, dsExpressionResultType);
    copyResultType.valuesArray = newValuesArray.slice(0, 0);
    copyResultType.disabled = true;
    return [
        (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ disabled: true }, dsParameterName),
        copyParamType,
        copyResultType
    ].concat(baseDSParamterSerializationsInfo);
}

;// external "DevExpress.Analytics.Widgets.Internal"
const external_DevExpress_Analytics_Widgets_Internal_namespaceObject = DevExpress.Analytics.Widgets.Internal;
;// ./dist/query-builder/widgets/expressionFunctions.js

const expressionFunctions = ((addins) => (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.combineFunctionDisplay)(addins))({
    'String': {
        'CreateTable': [{ paramCount: 1, text: 'CreateTable(, )', displayName: 'CreateTable(Column1, ..., ColumnN)', descriptionStringId: 'ExpressionEditorStringId.Function_CreateTable' }],
        'FormatString': [{ paramCount: 1, text: 'FormatString(, )', displayName: 'FormatString(Format, Value1, ... , ValueN)', descriptionStringId: 'ExpressionEditorStringId.Function_FormatString' }]
    }
});

;// ./dist/query-builder/dataSource/dataSourceParameter.js












function integerValueConverter(val, defaultVal, type) {
    return (0,external_DevExpress_Analytics_Internal_namespaceObject.integerValueConverter)(val, defaultVal, type);
}
function floatValueConverter(val, defaultVal, type) {
    return (0,external_DevExpress_Analytics_Internal_namespaceObject.floatValueConverter)(val, defaultVal, type);
}
function expressionValueConverter(val) {
    if (val instanceof Date) {
        const prependZero = (x) => (x < 10 ? '0' : '') + x;
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)('#{0}/{1}/{2} {3}:{4}#', prependZero(val.getMonth() + 1), prependZero(val.getDate()), val.getFullYear(), prependZero(val.getHours()), prependZero(val.getMinutes()));
    }
    return (val || '').toString();
}
class DataSourceParameterTypeValue {
    constructor(name, defaultValue, _valueConverter, realTypeName) {
        this.name = name;
        this.defaultValue = defaultValue;
        this.realTypeName = realTypeName;
        if (_valueConverter)
            this.valueConverter = (val) => _valueConverter(val, defaultValue, this.name);
    }
    get specifics() {
        return DBColumn.GetSpecific(this.realTypeName || this.name);
    }
}
const tryParseDate = val => {
    let date;
    try {
        date = (0,external_DevExpress_Analytics_Internal_namespaceObject.parseDate)(val);
    }
    catch (e) {
        date = dateDefaultValue();
    }
    return date;
};
const dateDefaultValue = () => {
    const date = new Date();
    date.setHours(0, 0, 0, 0);
    return date;
};
class DataSourceParameter extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    static _getTypeValue(typeName, resultType = null) {
        const result = DataSourceParameter._typeValues.filter((type) => { return type.name === typeName; });
        if (result.length > 0) {
            if (resultType && (0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(typeName))
                result[0].realTypeName = resultType;
            return result[0];
        }
        return { name: typeName, defaultValue: null, specifics: 'String', disableEditor: true };
    }
    _getTypeValue(typeName) {
        return DataSourceParameter._getTypeValue(typeName, this.resultType());
    }
    _tryConvertValue(value, typeValue) {
        if (!DataSourceParameter._isValueValid(value))
            return typeValue.defaultValue;
        const converter = typeValue.valueConverter || (val => { return val; }), newValue = converter(value);
        return DataSourceParameter._isValueValid(newValue) ? newValue : typeValue.defaultValue;
    }
    static _isValueValid(value) {
        return value !== void 0 && value !== null && !isNaN(typeof value === 'string' ? '' : value);
    }
    getEditorType(type) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getEditorType)(type);
    }
    _updateValueInfo(newType) {
        const typeValue = this._getTypeValue(newType);
        const newValue = this._tryConvertValue(this._value(), typeValue);
        const expressionOptions = this._expressionValue.peek();
        this._expressionValue(null);
        this._value(null);
        this._valueInfo((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, parameterValueSerializationsInfo, {
            editor: this.getEditorType(typeValue.name),
            disabled: typeValue.disableEditor === true,
            editorOptions: {
                onFocusOut: (params) => {
                    this.isValid(params.component.option('isValid'));
                }
            }
        }));
        this._expressionValue(expressionOptions);
        this.type(newType);
        this._value(newValue);
    }
    constructor(model, serializer, _serializationsInfo = dsParameterSerializationInfo) {
        super();
        this._serializationsInfo = _serializationsInfo;
        this._valueInfo = external_ko_namespaceObject.observable(parameterValueSerializationsInfo);
        this._parametersFunctions = expressionFunctions;
        this.isValid = external_ko_namespaceObject.observable(true);
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Parameter' }));
        this._disposables.push(this.name = external_ko_namespaceObject.pureComputed({
            read: () => { return this._name(); },
            write: (value) => { if (validateName(value))
                this._name(value); }
        }));
        this._expressionValue = external_ko_namespaceObject.observable({
            value: this._value,
            functions: this._parametersFunctions
        });
        this._disposables.push(this.type.subscribe((val) => {
            if ((0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(val)) {
                this.resultType(this._previousResultType);
            }
            else {
                this.resultType(null);
                this._previousResultType = val;
            }
            this._updateValueInfo(val);
        }));
        this._previousResultType = ((0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(this.type.peek()))
            ? this.resultType()
            : this.type();
        this.value = external_ko_namespaceObject.pureComputed({
            read: () => {
                return (0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(this.type()) ? this._expressionValue() : this._value();
            },
            write: (val) => {
                this._value(val);
            }
        });
        this._updateValueInfo(this.type.peek());
    }
    get specifics() {
        const realTypeName = ((0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(this.type.peek()))
            ? this.resultType()
            : this.type();
        const result = DataSourceParameter._typeValues.filter((type) => { return type.name === realTypeName; });
        if (result.length > 0)
            return result[0].specifics;
        return 'string';
    }
    getInfo() {
        if (this.type) {
            const info = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, [], this._serializationsInfo);
            info.splice(info.indexOf(info.filter((prop) => { return prop.propertyName === 'value'; })[0]), 1, this._valueInfo());
            return info;
        }
        return this._serializationsInfo;
    }
    isPropertyVisible(propName) {
        if (propName === 'resultType')
            return (0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(this.type());
        return true;
    }
}
DataSourceParameter._typeValues = [
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateTime, dateDefaultValue(), tryParseDate),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDateOnly, dateDefaultValue(), tryParseDate),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemTimeOnly, dateDefaultValue(), tryParseDate),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString, ''),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSByte, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt16, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt64, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemByte, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt16, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt32, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemUInt64, '0', integerValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDecimal, '0', floatValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemDouble, '0', floatValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemSingle, '0', floatValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBoolean, false, val => val !== void 0 ? String(val).toLowerCase() === 'true' : val),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemGuid, '00000000-0000-0000-0000-000000000000'),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.ExpressionType, '', expressionValueConverter),
    new DataSourceParameterTypeValue(external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemChar, ''),
];

;// ./dist/query-builder/dataSource/sql/customSqlQueryMeta.js
const customQuerySerializationsInfo = [
    { propertyName: 'type', modelName: '@Type' },
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'sqlString', modelName: 'Sql', defaultVal: '' },
    { propertyName: 'parameters', modelName: 'Parameters', array: true },
    { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/dataSource/sql/customSqlQuery.js







class CustomSqlQuery {
    constructor(model, parent, serializer) {
        this.parent = parent;
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this.type = external_ko_namespaceObject.pureComputed(() => SqlQueryType.customSqlQuery);
        this.parameters = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Parameters'], (item) => {
            return new DataSourceParameter(item, serializer);
        });
    }
    getInfo() {
        return customQuerySerializationsInfo;
    }
    generateName() {
        return 'CustomSqlQuery';
    }
}

;// ./dist/query-builder/dataSource/sql/masterDetailRelationMeta.js
const masterDetailRelationSerializationsInfo = [
    { propertyName: 'masterQuery', modelName: '@Master' },
    { propertyName: 'detailQuery', modelName: '@Detail' },
    { propertyName: '_customName', modelName: '@Name' },
    {
        propertyName: 'keyColumns', modelName: 'KeyColumns', array: true, info: [
            { propertyName: 'masterColumn', modelName: '@Master' },
            { propertyName: 'detailColumn', modelName: '@Detail' },
            { propertyName: 'itemType', modelName: '@ItemType' }
        ]
    },
    { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/dataSource/sql/masterDetailRelation.js





class MasterDetailRelation extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    dispose() {
        super.dispose();
        this.disposeObservableArray(this.keyColumns);
    }
    constructor(model, serializer) {
        super();
        this.name = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this._customName() || this.masterQuery() + this.detailQuery();
            },
            write: (value) => {
                this._customName(value);
            },
            deferEvaluation: true
        });
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Relation' }));
        this._disposables.push(this.name);
    }
    createKeyColumn() {
        const newKeyColumn = {
            masterColumn: external_ko_namespaceObject.observable(),
            detailColumn: external_ko_namespaceObject.observable(),
            itemType: 'KeyColumn'
        };
        this.keyColumns.push(newKeyColumn);
    }
    getInfo() {
        return masterDetailRelationSerializationsInfo;
    }
}

;// ./dist/query-builder/dataSource/sql/sqlDataConnection.js

class SqlDataConnection extends DataConnectionBase {
    static from(model, serializer) {
        return new SqlDataConnection(model, serializer);
    }
    static toJson(value, serializer, refs) {
        return serializer.serialize(value, dataConnectionSerializationInfo, refs);
    }
}

;// ./dist/query-builder/dataSource/sql/tableQueryMeta.js
const tableQuerySerializationsInfo = [
    { propertyName: 'type', modelName: '@Type' },
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'parameters', modelName: 'Parameters', array: true },
    {
        propertyName: '_tablesObject', modelName: 'Tables', info: [
            {
                propertyName: 'tables', modelName: 'SelectedTables', array: true, info: [
                    { propertyName: 'name', modelName: '@Name' },
                    { propertyName: 'alias', modelName: '@Alias' }
                ]
            }
        ]
    },
    { propertyName: 'filterString', modelName: 'Filter', defaultVal: '' },
    { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/dataSource/sql/tableQuery.js







class TableQuery {
    constructor(model, parent, serializer) {
        this.parent = parent;
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this.type = external_ko_namespaceObject.pureComputed(() => { return SqlQueryType.tableQuery; });
        this.parameters = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Parameters'], item => new DataSourceParameter(item, serializer));
    }
    tables() {
        return this['_tablesObject']['tables']();
    }
    getInfo() {
        return tableQuerySerializationsInfo;
    }
    generateName() {
        return this.tables().length > 0 ? (this.tables()[0].alias() || this.tables()[0].name()) : 'SelectQuery';
    }
}

;// ./dist/query-builder/dataSource/sql/storedProcQueryMeta.js
const storedProcQuerySerializationsInfo = [
    { propertyName: 'type', modelName: '@Type' },
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'procName', modelName: 'ProcName' },
    { propertyName: 'parameters', modelName: 'Parameters', array: true },
    { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/dataSource/sql/storedProcQuery.js









class StoredProcQuery {
    constructor(model, parent, serializer) {
        this.parent = parent;
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this.type = external_ko_namespaceObject.pureComputed(() => SqlQueryType.storedProcQuery);
        this.parameters = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Parameters'], item => {
            let parameterValueType = item['@Type'];
            if ((0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(parameterValueType))
                parameterValueType = item['@ResultType'] || parameterValueType;
            return new DataSourceParameter(item, serializer, storedProcParameterSerializationsInfo(parameterValueType));
        });
    }
    getInfo() {
        return storedProcQuerySerializationsInfo;
    }
    generateName() {
        return this.procName() || 'Query';
    }
}

;// ./dist/query-builder/dataSource/sql/sqlDataSource.js














const sqlDataSourceSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'connection', modelName: 'Connection', from: SqlDataConnection.from, toJsonObject: SqlDataConnection.toJson },
    { propertyName: 'queries', modelName: 'Queries', array: true },
    { propertyName: 'relations', modelName: 'Relations', array: true },
    { propertyName: 'resultSet', modelName: 'ResultSchema', from: ResultSet.from, toJsonObject: ResultSet.toJson },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
class SqlDataSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    getInfo() {
        return sqlDataSourceSerializationInfo;
    }
    createQuery(item, serializer) {
        if (item['@Type'] === SqlQueryType.customSqlQuery) {
            return new CustomSqlQuery(item, this, serializer);
        }
        else if (item['@Type'] === SqlQueryType.tableQuery) {
            return new TableQuery(item, this, serializer);
        }
        else if (item['@Type'] === SqlQueryType.storedProcQuery) {
            return new StoredProcQuery(item, this, serializer);
        }
        else {
            throw new Error('Unknown sql query type.');
        }
    }
    constructor(model, serializer, requestWrapper = new RequestWrapper()) {
        super();
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'SqlDataSource' }));
        const deprecateName = this['_model']['Name'];
        if (deprecateName) {
            if (!this.name()) {
                this.name(deprecateName);
            }
            delete this['_model']['Name'];
        }
        this.queries = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Queries'], (item) => this.createQuery(item, serializer));
        this.relations = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Relations'], (item) => {
            return new MasterDetailRelation(item, serializer);
        });
        if (this.connection && model['ConnectionOptions'])
            this.connection.options = new ConnectionOptions(model['ConnectionOptions'], serializer);
        this.dbSchemaProvider = new DBSchemaProvider(this.connection, requestWrapper);
        this._disposables.push(this.dbSchemaProvider);
        this._disposables.push(this.connection.name.subscribe(() => {
            this.queries([]);
            this.relations([]);
            this.resultSet = null;
        }));
    }
}

;// ./dist/query-builder/dataSource/object/objectDataSource.js



class ObjectDataSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor() {
        super(...arguments);
        this.name = external_ko_namespaceObject.observable();
    }
    setState(state) {
        this.ctor = state.ctor;
        this.dataMember = state.dataMember;
        this.selectedType = state.selectedType;
        this.id = (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)().replace(/-/g, '');
        this.name(state.dataSourceName);
    }
}

;// ./dist/query-builder/dataSource/object/objectSchema.js



class ObjectDataMemberBase {
    constructor(model) {
        model && (() => {
            this.name = model.name;
            this.displayName = model.displayName;
            this.dataMember = model.dataMember;
        })();
    }
}
class ObjectParameter extends ObjectDataMemberBase {
    constructor(model) {
        super(model);
        model && (() => {
            this.value = model.value;
            this.type = model.type;
            this.resultType = model.resultType;
        })();
    }
}
class ObjectDataSourceMethodBase extends ObjectDataMemberBase {
    constructor(model) {
        super(model);
        this.parameters = deserializeToCollection(model.parameters, (parameters) => new ObjectParameter(parameters));
    }
}
class ObjectDataMember extends ObjectDataSourceMethodBase {
    constructor(model) {
        super(model);
        this.disabled = external_ko_namespaceObject.observable(false);
        this.memberType = model && model.memberType;
        this.isStatic = model.isStatic;
    }
    static empty() {
        return new ObjectDataMember({
            displayName: ObjectDataMember.entireObject(),
            name: ObjectDataMember.entireObject(),
            parameters: []
        });
    }
    isEntireObject() {
        return this.name == ObjectDataMember.entireObject() && this.displayName == ObjectDataMember.entireObject();
    }
    isAvailable() {
        return !this.disabled();
    }
}
ObjectDataMember.entireObject = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Entire Object', 'AnalyticsCoreStringId.ObjectDSWizard_ConfigureParameters_EntireObject');
class ObjectCtor extends ObjectDataSourceMethodBase {
}
class ObjectType extends ObjectDataMemberBase {
    constructor(model) {
        super(model);
        this.ctors = deserializeToCollection(model.ctors, (ctor) => new ObjectCtor(ctor));
        this.members = deserializeToCollection(model.members, (member) => new ObjectDataMember(member));
    }
    updateMembers(selectedCtor) {
        this.members.forEach(member => {
            if (selectedCtor) {
                member.disabled(false);
            }
            else {
                member.disabled(member.isEntireObject() ? true : !member.isStatic);
            }
        });
    }
}

;// ./dist/query-builder/widgets/editorTemplates.js

const editorTemplates = new external_DevExpress_Analytics_Widgets_namespaceObject.EditorTemplates();

;// ./dist/query-builder/metadata.js



const query_builder_metadata_name = { propertyName: 'name', modelName: '@Name', displayName: 'Name', localizationId: 'DevExpress.DataAccess.Sql.SqlQuery.Name', disabled: true, editor: editorTemplates.getEditor('text') };
const alias = { propertyName: 'alias', modelName: '@Alias', displayName: 'Alias', localizationId: 'DataAccessUIStringId.QueryBuilderColumns_Alias', defaultVal: '', editor: editorTemplates.getEditor('text') };
const query_builder_metadata_text = { propertyName: 'text', modelName: '@Text', displayName: 'Text', editor: editorTemplates.getEditor('text') };
const selected = { propertyName: 'selected', displayName: 'Output', editor: editorTemplates.getEditor('bool'), localizationId: 'DataAccessUIStringId.QueryBuilderColumns_Output' };
const metadata_size = { propertyName: 'size', modelName: '@Size', defaultVal: '100,125', from: external_DevExpress_Analytics_Elements_namespaceObject.Size.fromString };
const query_builder_metadata_location = { propertyName: 'location', modelName: '@Location', from: external_DevExpress_Analytics_Elements_namespaceObject.Point.fromString };
const metadata_sizeLocation = [metadata_size, query_builder_metadata_location];
const metadata_unknownSerializationsInfo = [query_builder_metadata_name].concat(metadata_sizeLocation);

;// ./dist/query-builder/dataSource/federation/federatedQueries/sourceQuery.js





const sourceQuerySerializationsInfo = [
    { propertyName: 'queryType', modelName: '@QueryType' },
    query_builder_metadata_name,
    alias,
    { propertyName: 'sourceName', modelName: '@SourceName' },
];
class SourceQuery extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(model, serializer, sourceName, _sourcePath) {
        super();
        this._sourcePath = _sourcePath;
        model['@SourceName'] = model['@SourceName'] || sourceName;
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, model);
        this._disposables.push(this.queryType = external_ko_namespaceObject.pureComputed(() => { return FederationQueryType[FederationQueryType.SourceNode]; }));
    }
    getInfo() {
        return sourceQuerySerializationsInfo;
    }
    getPath() {
        if (this._sourcePath)
            return this._sourcePath;
        if (this.name()) {
            if (this.name() === this.sourceName())
                return [this.name()];
            const dataSourceName = this.sourceName().slice(0, this.sourceName().indexOf(this.name()) - 1);
            return [dataSourceName, this.name()];
        }
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueryExpression.js



const expressionSerializationInfo = [
    { propertyName: 'table', modelName: '@NodeAlias' },
    query_builder_metadata_name,
    alias,
    { propertyName: 'expressionType', modelName: '@ExpressionType' },
    { propertyName: 'columnExpression', modelName: '@ColumnExpression' }
];
var FederatedQueryExpressionType;
(function (FederatedQueryExpressionType) {
    FederatedQueryExpressionType[FederatedQueryExpressionType["SelectColumnExpression"] = 0] = "SelectColumnExpression";
    FederatedQueryExpressionType[FederatedQueryExpressionType["SelectExpression"] = 1] = "SelectExpression";
    FederatedQueryExpressionType[FederatedQueryExpressionType["SelectAllColumnsExpression"] = 2] = "SelectAllColumnsExpression";
    FederatedQueryExpressionType[FederatedQueryExpressionType["SelectAllNodeColumnsExpression"] = 3] = "SelectAllNodeColumnsExpression";
    FederatedQueryExpressionType[FederatedQueryExpressionType["SelectRowCountExpression"] = 4] = "SelectRowCountExpression";
})(FederatedQueryExpressionType || (FederatedQueryExpressionType = {}));
class FederatedQueryExpression {
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Expression' }));
    }
    getInfo() {
        return expressionSerializationInfo;
    }
}

;// ./dist/query-builder/dataSource/federation/federationDataSourceMeta.js
const federationDataSourceSerializationInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'queries', modelName: 'Queries', array: true },
    { propertyName: 'relations', modelName: 'Relations', array: true },
    { propertyName: 'sources', modelName: 'Sources', array: true },
    {
        modelName: 'SerializableSourceMap',
        propertyName: 'serializableSourceMap',
        array: true,
        info: [
            { modelName: '@DataSource', propertyName: 'dataSource', link: true },
            { modelName: '@Name', propertyName: 'name' }
        ]
    }
];
const sourceSerializationInfo = [
    { propertyName: 'sourceName', modelName: '@SourceName' },
    { propertyName: 'dataMember', modelName: '@DataMember' },
];

;// ./dist/query-builder/dataSource/federation/federationSource.js




class FederationSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(model, serializer, pathParts, sourceName) {
        super();
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        const [dataSource, ...dataMember] = pathParts || [];
        model['@DataMember'] = dataMember?.join('.') || model['@DataMember'];
        model['@SourceName'] = sourceName || model['@SourceName'];
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Source' }));
    }
    getInfo() {
        return sourceSerializationInfo;
    }
    getDataSourceName() {
        return this.hasDataMember() ? this.sourceName().slice(0, this.sourceName().lastIndexOf(this.dataMember()) - 1)
            : this.sourceName();
    }
    getPath() {
        return [this.getDataSourceName()].concat(this.hasDataMember() ? [this.dataMember()] : []);
    }
    hasDataMember() {
        return this.dataMember() && this.sourceName().lastIndexOf(this.dataMember()) !== 0;
    }
}

;// external "DevExpress.Analytics.Criteria"
const external_DevExpress_Analytics_Criteria_namespaceObject = DevExpress.Analytics.Criteria;
;// external "DevExpress.Analytics.Criteria.Utils"
const external_DevExpress_Analytics_Criteria_Utils_namespaceObject = DevExpress.Analytics.Criteria.Utils;
;// ./dist/query-builder/utils/controlsFactory.js

const controlsFactory = new external_DevExpress_Analytics_Utils_namespaceObject.ControlsFactory();

;// ./dist/query-builder/elements/queryElementModel.js


class QueryElementBaseViewModel extends external_DevExpress_Analytics_Elements_namespaceObject.ElementViewModel {
    getControlFactory() {
        return controlsFactory;
    }
    constructor(control, parent, serializer) {
        super(control, parent, serializer);
    }
}

;// ./dist/query-builder/elements/joinConditionModel.js





class JoinConditionViewModel extends RoutedConnectorViewModel {
    getControlFactory() {
        return controlsFactory;
    }
    beforeDeserialize() {
        this.startPoint = external_ko_namespaceObject.observable();
        this.endPoint = external_ko_namespaceObject.observable();
    }
    constructor(control, relation, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(control, { '@ControlType': 'JoinCondition', '@ItemType': 'KeyColumn' }), relation, serializer);
        this.parentColumn = external_ko_namespaceObject.pureComputed(() => relation.parentTable().getColumn(this.parentColumnName()));
        this.nestedColumn = external_ko_namespaceObject.pureComputed(() => relation.nestedTable().getColumn(this.nestedColumnName()));
        this.joinType = relation.joinType;
        this.left = external_ko_namespaceObject.pureComputed(() => relation.parentTableName() + '.' + this.parentColumnName());
        this.right = external_ko_namespaceObject.pureComputed(() => relation.nestedTableName() + '.' + this.nestedColumnName());
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            if (this.parentColumn() && this.nestedColumn()) {
                const result = determineConnectingPoints(this.parentColumn(), this.nestedColumn());
                this.startPoint().connectingPoint(result.start);
                this.endPoint().connectingPoint(result.end);
            }
        }));
    }
}

;// ./dist/query-builder/elements/relationModelMeta.js
const relationSerializationInfo = [
    { propertyName: 'joinType', modelName: '@Type' },
    { propertyName: 'parentTableName', modelName: '@Parent' },
    { propertyName: 'nestedTableName', modelName: '@Nested' },
    { propertyName: 'conditions', modelName: 'KeyColumns', defaultVal: [], array: true },
    { propertyName: 'itemType', modelName: '@itemType' }
];

;// ./dist/query-builder/elements/relationModel.js






class RelationViewModel extends QueryElementBaseViewModel {
    _getConditionNumber() {
        let result = this.conditions().length + 1;
        const existingNumbers = this.conditions().map(_c => _c.seriesNumber());
        for (let index = 0; index < this.conditions().length; index++) {
            if (existingNumbers.indexOf(index + 1) !== -1)
                continue;
            result = index + 1;
            break;
        }
        return result;
    }
    constructor(model, query, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ControlType': 'Relation', '@ItemType': 'Relation' }), query, serializer);
        this.parentTable = external_ko_namespaceObject.observable(query.getTable(this.parentTableName.peek()));
        this.nestedTable = external_ko_namespaceObject.observable(query.getTable(this.nestedTableName.peek()));
        this.parentTableName = external_ko_namespaceObject.pureComputed(() => this.parentTable().actualName());
        this.nestedTableName = external_ko_namespaceObject.pureComputed(() => this.nestedTable().actualName());
        this.conditions = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['KeyColumns'], (item) => {
            return new JoinConditionViewModel(item, this, serializer);
        });
    }
    getInfo() {
        return relationSerializationInfo;
    }
    addChild(control) {
        const condition = control;
        if (this.conditions && this.conditions.indexOf(condition) === -1) {
            condition.seriesNumber(this._getConditionNumber());
            condition.parentModel(this);
            this.conditions.push(condition);
        }
    }
    removeChild(control) {
        const index = this.conditions().indexOf(control);
        if (index > -1)
            this.conditions.splice(index, 1);
        if (this.conditions().length === 0)
            this.parentModel().removeChild(this);
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueries/subNode.js











const subNodeSerializationInfo = [
    { propertyName: 'condition', modelName: '@Condition', defaultVal: '' },
    {
        propertyName: 'joinType',
        modelName: '@JoinType',
        defaultVal: 'Inner',
    },
    { propertyName: 'query', modelName: 'Query' },
];
class SubNode {
    constructor(model, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'JoinElement' }));
        if (model['Query'])
            this.query = external_ko_namespaceObject.observable(new SourceQuery(model['Query']));
    }
    static serializeRelation(relation) {
        const parentTableName = relation.parentTableName();
        const nestedTableName = relation.nestedTableName();
        return relation.conditions().map((condition) => {
            const firstCondition = '[' + nestedTableName + '.' + condition.nestedColumnName() + ']';
            const secondCondition = '[' + parentTableName + '.' + condition.parentColumnName() + ']';
            const operator = external_DevExpress_Analytics_Criteria_Utils_namespaceObject.operatorTokens[condition.operator()];
            const conditionString = firstCondition + ' ' + operator + ' ' + secondCondition;
            return conditionString;
        });
    }
    static deserializeRelationModel(subNodeQuery, relations) {
        const conditionStrings = relations.reduce((conditionStrings, relation) => {
            return conditionStrings.concat(SubNode.serializeRelation(relation));
        }, []);
        const subNode = new SubNode({ '@JoinType': relations[0]?.joinType(), '@Condition': conditionStrings.join(' And ') });
        subNode.query(subNodeQuery);
        return subNode;
    }
    _parsePath(condition) {
        if (condition[0] === '[' && condition[condition.length - 1] === ']')
            return external_DevExpress_Analytics_Criteria_Utils_namespaceObject.CriteriaOperatorStateMachine.parse(condition)['propertyName'];
        return condition;
    }
    _createCondition(parent, nested, operator) {
        return {
            '@ControlType': 'JoinCondition',
            '@ItemType': 'KeyColumn',
            '@Operator': operator,
            '@Nested': nested,
            '@Parent': parent
        };
    }
    _conditionBinary(operator) {
        const leftPath = this._parsePath(operator.leftPart['propertyName']);
        const rightPath = this._parsePath(operator.rightPart['propertyName']);
        const _operator = external_DevExpress_Analytics_Criteria_namespaceObject.BinaryOperatorType[operator.operatorType];
        return this._createCondition(rightPath.split('.')[1], leftPath.split('.')[1], _operator);
    }
    createRelationModel(query) {
        const result = external_DevExpress_Analytics_Criteria_Utils_namespaceObject.CriteriaOperatorStateMachine.parse(this.condition());
        const relationMap = {};
        const processBinary = (operator) => {
            const parentTable = this._parsePath(operator.rightPart['propertyName']).split('.')[0];
            const nestedTable = this._parsePath(operator.leftPart['propertyName']).split('.')[0];
            const keyColumns = this._conditionBinary(operator);
            if (relationMap[parentTable]) {
                relationMap[parentTable].addChild(new JoinConditionViewModel(keyColumns, relationMap[parentTable]));
            }
            else {
                relationMap[parentTable] = new RelationViewModel({
                    '@Parent': parentTable,
                    '@Nested': nestedTable,
                    '@Type': this.joinType(),
                    'KeyColumns': {
                        'Item1': keyColumns
                    }
                }, query, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer());
            }
        };
        if (result instanceof external_DevExpress_Analytics_Criteria_namespaceObject.GroupOperator) {
            result.operands.forEach(operand => {
                if (operand instanceof external_DevExpress_Analytics_Criteria_namespaceObject.BinaryOperator)
                    processBinary(operand);
            });
        }
        else if (result instanceof external_DevExpress_Analytics_Criteria_namespaceObject.BinaryOperator) {
            processBinary(result);
        }
        return Object.values(relationMap);
    }
    getInfo() {
        return subNodeSerializationInfo;
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueries/selectQuery.js










const selectQuerySerializationsInfo = [
    { propertyName: 'queryType', modelName: '@QueryType' },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'expressions', modelName: 'Expressions', array: true },
    { propertyName: 'subNodes', modelName: 'SubNodes', array: true },
    { propertyName: 'root', modelName: 'Root' },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
class SelectQuery extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(model, serializer, _path) {
        super();
        this._path = _path;
        this.init(model, serializer, _path);
    }
    get sources() {
        return external_ko_namespaceObject.observableArray([this.root()].concat(this.subNodes().map(node => node.query())).map(sourceQuery => new FederationSource({}, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer(), sourceQuery.getPath(), sourceQuery.sourceName())));
    }
    getInfo() {
        return selectQuerySerializationsInfo;
    }
    generateName() {
        return this.root() ? this.root().alias() : 'SelectQuery';
    }
    init(model, serializer, rootPath) {
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this._disposables.push(this.queryType = external_ko_namespaceObject.pureComputed(() => { return FederationQueryType[FederationQueryType.SelectNode]; }));
        this.expressions = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Expressions'], (item) => {
            return new FederatedQueryExpression(item, serializer);
        });
        this.subNodes = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['SubNodes'], (item) => {
            return new SubNode(item, serializer);
        });
        if (model['Root'])
            this.root = external_ko_namespaceObject.observable(new SourceQuery(model['Root'], serializer, this.alias(), rootPath));
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueries/transformQuery.js








const transformQuerySerializationsInfo = [
    { propertyName: 'queryType', modelName: '@QueryType' },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'transformationRules', modelName: 'TransformationRules', array: true },
    { propertyName: 'root', modelName: 'Root' },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
const transformationRuleSerializationsInfo = [
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'unfold', modelName: '@Unfold' },
    { propertyName: 'flatten', modelName: '@Flatten' },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
class TransformQuery extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(model, serializer) {
        super();
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this._disposables.push(this.queryType = external_ko_namespaceObject.pureComputed(() => { return FederationQueryType[FederationQueryType.TransformationNode]; }));
        this.transformationRules = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['TransformationRules'], (item) => {
            return new FederationTransformationRule(item, serializer);
        });
        if (model['Root'])
            this.root = external_ko_namespaceObject.observable(new SourceQuery(model['Root'], serializer));
    }
    get sources() {
        return external_ko_namespaceObject.observableArray([new FederationSource({}, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer(), this.root().getPath(), this.root().sourceName())]);
    }
    getInfo() {
        return transformQuerySerializationsInfo;
    }
    generateName() {
        return this.root() && this.root().sourceName().split('_').pop();
    }
}
class FederationTransformationRule {
    constructor(model, serializer) {
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'TransformationRule' }));
    }
    getInfo() {
        return transformationRuleSerializationsInfo;
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueriesContainer.js








class FederatedQueriesContainer extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(model, dataSources, _serializer) {
        super();
        this.dataSources = dataSources;
        this._serializer = _serializer;
        this.sources = external_ko_namespaceObject.observableArray();
        this._serializer = _serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
    }
    _dataSourceName(dataSource) {
        return dataSource?.data?.name() || dataSource?.name;
    }
    getQueryNameFromPath(path) {
        const [dataSourceRef, ...members] = path;
        const dataSource = this.dataSources().filter(x => x.ref == dataSourceRef || x.id == dataSourceRef)[0];
        const dataSourceName = this._dataSourceName(dataSource) || dataSourceRef;
        const dataMember = members?.join('.');
        return dataMember ? [dataSourceName, dataMember].join('_') : dataSourceName;
    }
    getPathFromQueryName(sourceName) {
        for (const source of this.sources()) {
            if (source.sourceName() === sourceName) {
                const pathParts = source.getPath();
                let dataSourceId = pathParts.shift();
                const dataSource = this.dataSources().filter(x => this._dataSourceName(x) == dataSourceId)[0];
                dataSourceId = dataSource && (dataSource.ref || dataSource.id) || dataSourceId;
                return [dataSourceId, ...pathParts];
            }
        }
    }
    createQuery(item, dataSource) {
        if (item['@QueryType'] === FederationQueryType[FederationQueryType.SelectNode]) {
            return new SelectQuery(item, this._serializer, (dataSource || this).getPathFromQueryName(item['Root'] && item['Root']['@SourceName']));
        }
        return null;
    }
    addSource(source, queryPath) {
        if (source instanceof FederationSource) {
            const existedSource = this.sources().filter(x => x.sourceName() == source.sourceName())[0];
            !existedSource && this.sources.push(source);
            return;
        }
        const existedSource = this.sources().filter(x => x.sourceName() == source)[0];
        !existedSource && this.sources.push(new FederationSource({}, this._serializer, queryPath, source));
    }
    removeSource(sourceName) {
        const source = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.sources(), source => source.sourceName() === sourceName);
        if (!this.queries().some(query => query.expressions().some(exp => exp.table() === sourceName)))
            this.sources.remove(source);
    }
    addSelectQuery(queryPath, columnName) {
        const queryName = this.getQueryNameFromPath(queryPath);
        let query = this.queries().filter(query => query.alias() === queryName)[0];
        if (!query) {
            query = new SelectQuery({
                '@Alias': queryName,
                'Root': {},
            }, this._serializer, queryPath);
            this.queries.push(query);
            this.addSource(queryName, queryPath);
        }
        if (!columnName || query.expressions().filter(expression => expression.name() == columnName)[0]) {
            return;
        }
        const expression = new FederatedQueryExpression({
            '@NodeAlias': queryName,
            '@Name': columnName,
            '@ExpressionType': FederatedQueryExpressionType[FederatedQueryExpressionType.SelectColumnExpression]
        }, this._serializer);
        query.expressions.push(expression);
    }
    removeQuery(queryName) {
        const query = this.queries().filter(query => query.alias() === queryName)[0];
        this.queries.remove(query);
        const source = this.sources().filter(x => x.sourceName() == queryName)[0];
        source && this.sources.remove(source);
    }
    removeExpression(columnName, queryPath) {
        const queryName = this.getQueryNameFromPath(queryPath);
        const query = this.queries().filter(query => query.alias() === queryName)[0];
        const expression = query.expressions().filter(x => x.name() == columnName)[0];
        query.expressions.remove(expression);
        if (query.expressions().length == 0) {
            this.removeQuery(queryName);
        }
    }
    dispose() {
        super.dispose();
        this.disposeObservableArray(this.sources);
        this.disposeObservableArray(this.queries);
    }
}

;// ./dist/query-builder/dataSource/federation/federatedQueries/unionQuery.js






const unionQuerySerializationsInfo = [
    { propertyName: 'queryType', modelName: '@QueryType' },
    { propertyName: 'unionType', modelName: '@UnionType' },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'queries', modelName: 'UnionElements', array: true }
];
var UnionTypes;
(function (UnionTypes) {
    UnionTypes[UnionTypes["Union"] = 0] = "Union";
    UnionTypes[UnionTypes["UnionAll"] = 1] = "UnionAll";
})(UnionTypes || (UnionTypes = {}));
class UnionQuery extends FederatedQueriesContainer {
    constructor(model, dataSources, serializer) {
        super(model, dataSources, serializer);
        (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Query' }));
        this._disposables.push(this.queryType = external_ko_namespaceObject.pureComputed(() => { return FederationQueryType[FederationQueryType.UnionNode]; }));
        this.queries = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['UnionElements'], (item) => this.createQuery(item));
    }
    getInfo() {
        return unionQuerySerializationsInfo;
    }
    generateName() {
        return this.queries()[0] && this.queries()[0].alias().split('_').pop();
    }
}

;// ./dist/query-builder/dataSource/federation/federationMasterDetailRelation.js



const keyColumnInfoArray = [
    { propertyName: 'masterColumn', modelName: '@Parent' },
    { propertyName: 'detailColumn', modelName: '@Nested' },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
const masterDetailRelationSerializationInfoArray = [
    { propertyName: 'masterQuery', modelName: '@MasterQueryName' },
    { propertyName: 'detailQuery', modelName: '@DetailQueryName' },
    { propertyName: '_customName', modelName: '@RelationName' },
    { propertyName: 'keyColumns', modelName: 'KeyColumns', array: true, info: keyColumnInfoArray },
    { propertyName: 'itemType', modelName: '@ItemType' }
];
class FederationMasterDetailRelation extends MasterDetailRelation {
    static create(_relation) {
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        const relation = new FederationMasterDetailRelation(serializer.serialize(_relation), serializer);
        relation.keyColumns().forEach(x => x['getInfo'] = () => keyColumnInfoArray);
        return relation;
    }
    constructor(model, serializer) {
        super(model, serializer);
        this.getInfo = () => masterDetailRelationSerializationInfoArray;
        if (model['@MasterQueryName'] && model['@DetailQueryName']) {
            (serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'Relation' }));
        }
    }
}

;// ./dist/query-builder/dataSource/federation/federationSerializableModel.js



class SerializableDataFederationDataSource extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    get _currentDataSources() {
        const dataSources = [];
        this.dataSource && this.dataSource.serializableSourceMap().reduce((result, x) => {
            const dataSource = x.dataSource();
            if (dataSource && result.indexOf(dataSource) === -1)
                result.push(dataSource);
            return result;
        }, dataSources);
        return dataSources;
    }
    _collectDependentDataSources(item, resultArray) {
        if (item.serializableSourceMap) {
            item.serializableSourceMap().forEach(x => this._collectDependentDataSources(x.dataSource(), resultArray));
        }
        const itemName = external_ko_namespaceObject.unwrap(item.name);
        if (!itemName || resultArray.every(x => external_ko_namespaceObject.unwrap(x.name) !== itemName)) {
            resultArray.push(item);
        }
    }
    constructor(dataSource, serializer) {
        super();
        this.dataSources = external_ko_namespaceObject.observableArray([]);
        this.serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        if (dataSource) {
            this.dataSource = dataSource;
            this.dataSources(this._currentDataSources);
        }
    }
    dispose() {
        this.dataSource = null;
        this.dataSources = null;
    }
    getInfo() {
        return [
            { propertyName: 'dataSources', modelName: 'dataSources', array: true },
            { propertyName: 'dataSource', modelName: 'dataSource' }
        ];
    }
    collectDependentDataSources() {
        const dependentDataSources = [];
        this._currentDataSources.forEach(item => this._collectDependentDataSources(item, dependentDataSources));
        return dependentDataSources;
    }
    serialize() {
        const clonedModel = new SerializableDataFederationDataSource(null);
        clonedModel.dataSource = this.dataSource;
        clonedModel.dataSources(this.collectDependentDataSources());
        const serializedModel = this.serializer.serialize(clonedModel);
        clonedModel.dispose();
        return serializedModel;
    }
    getSerializableFederationDataSourceInfo() {
        const serializedModel = this.serialize();
        return {
            dataSource: JSON.stringify(serializedModel.dataSource),
            dataSources: Object.keys(serializedModel.dataSources || []).map(key => JSON.stringify(serializedModel.dataSources[key]))
        };
    }
}

;// ./dist/query-builder/dataSource/federation/_dbSchemaFederationDataSourceProvider.js




class DBSchemaFederationDataSourceProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    getDbSchema() {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        if (this._rootItems.length)
            deferred.resolve(this._rootItems);
        else {
            this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest('')).done(rootItems => {
                deferred.resolve(rootItems);
            });
        }
        return deferred.promise();
    }
    getDbTable(tableName, path) {
        const $deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(path)).done(items => {
            const columns = items.map(x => {
                return {
                    Name: x.name
                };
            });
            $deferred.resolve(new DBTable({ Name: tableName, columns: columns }));
        });
        return $deferred.promise();
    }
    dispose() {
        super.dispose();
        this.getItems = null;
    }
    constructor(itemsProvider) {
        super();
        this._rootItems = [];
        this.getItems = itemsProvider.getItems;
    }
}

;// ./dist/query-builder/dataSource/federation/federationDataSource.js












class FederationDataSource extends FederatedQueriesContainer {
    getInfo() {
        return federationDataSourceSerializationInfo;
    }
    getSerializableModel() {
        return this._serializableModel;
    }
    createQuery(item) {
        const query = super.createQuery(item);
        if (!query) {
            if (item['@QueryType'] === FederationQueryType[FederationQueryType.UnionNode]) {
                return new UnionQuery(item, this.dataSources, this._serializer);
            }
            else if (item['@QueryType'] === FederationQueryType[FederationQueryType.TransformationNode]) {
                return new TransformQuery(item, this._serializer);
            }
        }
        return query;
    }
    updateSerializableModel() {
        this.serializableSourceMap = external_ko_namespaceObject.observableArray();
        this.sources().forEach(source => {
            const dataSourceName = source.getDataSourceName();
            dataSourceName && this.serializableSourceMap().push({
                name: external_ko_namespaceObject.observable(source.sourceName()),
                dataSource: external_ko_namespaceObject.observable(this.dataSources().filter(x => this._dataSourceName(x) === dataSourceName)[0].data)
            });
        });
        this._serializableModel = new SerializableDataFederationDataSource(this, this._serializer);
    }
    constructor(model, dataSources, fielListProvider, serializer) {
        super(model, dataSources, serializer);
        this.dataSources = dataSources;
        this.fielListProvider = fielListProvider;
        this.serializableSourceMap = external_ko_namespaceObject.observableArray();
        this._serializer.deserialize(this, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(model, { '@ItemType': 'FederationDataSource' }));
        this.sources = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Sources'], (item) => {
            return new FederationSource(item, serializer);
        });
        this.queries = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Queries'], (item) => this.createQuery(item));
        this.relations = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(model['Relations'], (item) => {
            return new FederationMasterDetailRelation(item, this._serializer);
        });
        if (fielListProvider)
            this._disposables.push(this.dbSchemaProvider = new DBSchemaFederationDataSourceProvider(this.fielListProvider));
        this.updateSerializableModel();
    }
    dispose() {
        super.dispose();
        this.disposeObservableArray(this.relations);
        this._serializableModel.dispose();
    }
    get dependentDataSources() {
        const serializableModel = this.getSerializableModel();
        return serializableModel.getSerializableFederationDataSourceInfo().dataSources;
    }
}

;// ./dist/analytics-data.js

















































































;// ./dist/query-builder/dataSource/_utils.js


function generateQueryUniqueName(queries, query, nameProperty = 'name') {
    const name = (query[nameProperty]() || query.generateName()).replace(/\./g, '_');
    return (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(queries, item => item[nameProperty]() === name) ?
        (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueNameForNamedObjectsArray)(queries, name + '_') : name;
}

;// ./dist/analytics-data-internal.js











;// ./dist/analytics-data-utils.js



;// ./dist/analytics-data-metadata.js















;// ./dist/query-builder/wizard/pageId.js
const DataSourceWizardPageId = {
    ChoosePredefinedDataSourcePage: 'choosePredefinedDataSourcePage',
    ChooseDataSourceTypePage: 'chooseDataSourceTypePage',
    ConfigureMasterDetailRelationshipsPage: 'configureMasterDetailRelationshipsPage'
};
const SqlDataSourceWizardPageId = {
    ChooseConnectionPage: 'chooseSqlConnectionPage',
    ConfigureQueryPage: 'configureSqlQueryPage',
    ConfigureParametersPage: 'configureSqlParametersPage',
    MultiQueryConfigurePage: 'multiSqlQueryConfigurePage',
    MultiQueryConfigureParametersPage: 'multiSqlQueryConfigureParametersPage',
    FederatedQueryConfigurePage: 'federatedQueryConfigurePage'
};
const FederationDataSourceWizardPageId = {
    FederatedQueryConfigurePage: 'federatedQueryConfigurePage',
    FederatedMasterDetailRelationshipsPage: 'federatedMasterDetailRelationshipsPage'
};
const JsonDataSourceWizardPageId = {
    ChooseJsonSourcePage: 'chooseJsonSourcePage',
    ChooseJsonSchemaPage: 'chooseJsonSchemaPage',
    ChooseConnectionPage: 'chooseJsonConnectionPage',
    SpecifyJsonConnectionPage: 'specifyJsonConnectionPage'
};
const MongoDBDataSourceWizardPageId = {
    ChooseMongoDBDataConnectionPage: 'chooseMongoDBDataConnectionPage',
    ChooseMongoDBCollectionPage: 'chooseMongoDBCollectionPage',
    ChooseMongoDBSchemaPage: 'chooseMongoDBSchemaPage'
};
const ObjectDataSourceWizardPageId = {
    ChooseTypesPage: 'chooseObjectDataSourceTypesPage',
    ChooseDataMembersPage: 'chooseObjectDataSourceDataMembersPage',
    ConfigureParametersPage: 'configureObjectDataSourceParametersPage'
};
const FullscreenDataSourceWizardPageId = {
    ChooseDataSourceTypePage: DataSourceWizardPageId.ChooseDataSourceTypePage,
    SpecifySqlDataSourceSettingsPage: 'specifySqlDataSourceSettingsPage',
    SpecifyJsonDataSourceSettingsPage: 'specifyJsonDataSourceSettingsPage',
    SpecifyMongoDBDataSourceSettingsPage: 'specifyMongoDBDataSourceSettingsPage',
    SpecifyObjectDataSourceSettingsPage: 'specifyObjectDataSourceSettingsPage',
    SpecifyFederationDataSourceSettingsPage: 'specifyFederationDataSourceSettingsPage',
    SelectDataSourcePage: 'selectDataSourcePage'
};
const FullscreenDataSourceWizardSectionId = {
    SpecifyJsonConnectionPage: JsonDataSourceWizardPageId.SpecifyJsonConnectionPage,
    ChooseJsonSchemaPage: JsonDataSourceWizardPageId.ChooseJsonSchemaPage,
    ChooseJsonSourcePage: JsonDataSourceWizardPageId.ChooseJsonSourcePage,
    ChooseMongoDBDataConnectionPage: MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage,
    ChooseMongoDBCollectionsPage: MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage,
    ChooseMongoDBSchemaPage: MongoDBDataSourceWizardPageId.ChooseMongoDBSchemaPage,
    ChooseSqlConnectionPage: SqlDataSourceWizardPageId.ChooseConnectionPage,
    ConfigureFederatedQueriesPage: FederationDataSourceWizardPageId.FederatedQueryConfigurePage,
    ConfigureFederatedMasterDetailRelationshipsPage: FederationDataSourceWizardPageId.FederatedMasterDetailRelationshipsPage,
    ConfigureQueryPage: SqlDataSourceWizardPageId.MultiQueryConfigurePage,
    ConfigureQueryParametersPage: SqlDataSourceWizardPageId.MultiQueryConfigureParametersPage,
    ConfigureMasterDetailRelationshipsPage: DataSourceWizardPageId.ConfigureMasterDetailRelationshipsPage
};

;// ./dist/query-builder/wizard/settings.js

const AutoQueryPreload = (0,external_DevExpress_Analytics_Internal_namespaceObject.createGlobalModuleVariableFunc)(true);

;// ./dist/query-builder/wizard/internal/_constants.js
const __loadingStateFunctionName = '__loadingState';
const __nextActionFunctionName = '__nextAction';

;// ./dist/query-builder/wizard/pages/wizardPageBase.js


class WizardPageBase extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor() {
        super(...arguments);
        this.changeAlways = false;
        this._onChange = () => void 0;
    }
    dispose() {
        super.dispose();
        this._onChange = () => void 0;
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    onChange(callback) {
        this._onChange = callback;
    }
    initialize(state, stateChanged) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    canNext() {
        return true;
    }
    canFinish() {
        return false;
    }
}

;// ./dist/query-builder/wizard/pages/chooseDataSourceTypePage.js







var DataSourceType;
(function (DataSourceType) {
    DataSourceType[DataSourceType["NoData"] = 0] = "NoData";
    DataSourceType[DataSourceType["Sql"] = 1] = "Sql";
    DataSourceType[DataSourceType["Json"] = 2] = "Json";
    DataSourceType[DataSourceType["Object"] = 3] = "Object";
    DataSourceType[DataSourceType["Federation"] = 4] = "Federation";
    DataSourceType[DataSourceType["MongoDB"] = 5] = "MongoDB";
})(DataSourceType || (DataSourceType = {}));
class TypeItem {
    constructor(textDefault, textID, imageClassName, imageTemplateName, type) {
        this.text = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(textDefault, textID);
        this.imageClassName = imageClassName;
        this.imageTemplateName = imageTemplateName;
        this.type = type;
    }
}
class ChooseDataSourceTypePage extends WizardPageBase {
    constructor(_dataSourceTypeOptions) {
        super();
        this._dataSourceTypeOptions = _dataSourceTypeOptions;
        this._itemClick = (item) => {
            this.selectedItem(item);
        };
        this._IsSelected = (item) => {
            return this.selectedItem().type === item.type;
        };
        this.selectedItem = external_ko_namespaceObject.observable();
        this.typeItems = this._createTypeItems();
        this._disposables.push(this.selectedItem.subscribe(() => this._onChange()));
        this._extendCssClass = () => void (0);
    }
    canNext() {
        return this.selectedItem() != null;
    }
    canFinish() {
        return false;
    }
    _goToNextPage() {
        this[__nextActionFunctionName] && this[__nextActionFunctionName]();
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({ dataSourceType: this.selectedItem().type }).promise();
    }
    _createTypeItems() {
        const typeItems = [];
        if (this._dataSourceTypeOptions.sqlDataSourceAvailable) {
            typeItems.push(new TypeItem('Database', 'DataAccessUIStringId.DSTypeSql', 'sqldatasource', 'dxrd-svg-wizard-SqlDataSource', DataSourceType.Sql));
        }
        if (this._dataSourceTypeOptions.jsonDataSourceAvailable) {
            typeItems.push(new TypeItem('JSON', 'DataAccessUIStringId.DSTypeJson', 'jsondatasource', 'dxrd-svg-wizard-JsonDataSource', DataSourceType.Json));
        }
        if (this._dataSourceTypeOptions.objectDataSourceAvailable) {
            typeItems.push(new TypeItem('Object', 'DataAccessUIStringId.DSTypeObject', 'objectdatasource', 'dxrd-svg-wizard-ObjectDataSource', DataSourceType.Object));
        }
        if (this._dataSourceTypeOptions.mongoDBDataSourceAvailable) {
            typeItems.push(new TypeItem('MongoDB', 'DataAccessUIStringId.DSTypeMongoDB', 'mongodatasource', 'dxrd-svg-wizard-MongodbDataSource', DataSourceType.MongoDB));
        }
        if (this._dataSourceTypeOptions.federationDataSourceAvailable) {
            typeItems.push(new TypeItem('Data Federation', 'DataAccessUIStringId.DSTypeFederation', 'federationdatasource', 'dxrd-svg-wizard-FederationDataSource', DataSourceType.Federation));
        }
        return typeItems;
    }
    initialize(state) {
        if (!this.typeItems || this.typeItems.length === 0) {
            this.selectedItem(null);
        }
        else {
            const type = state.dataSourceType !== undefined ? state.dataSourceType : this.typeItems[0].type;
            this.selectedItem((0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.typeItems, (item) => item.type === type));
        }
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
}
function _registerChooseDataSourceTypePage(factory, dataSourceTypeOptions) {
    factory.registerMetadata(DataSourceWizardPageId.ChooseDataSourceTypePage, {
        setState: (data, state) => {
            state.dataSourceType = data.dataSourceType;
        },
        getState: (state) => {
            return state;
        },
        resetState: (state, defaultState) => {
            state.dataSourceType = defaultState.dataSourceType;
        },
        create: () => {
            return new ChooseDataSourceTypePage(dataSourceTypeOptions);
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Select the data source type.', 'DataAccessUIStringId.WizardPageChooseDSType'),
        template: 'dxrd-page-choose-datasource-type'
    });
}

;// ./dist/query-builder/wizard/pages/sqlDataSourceWizard/_sqlDataSourceWrapper.js



class _SqlDataSourceWrapper extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    get sqlQuery() {
        return this.sqlDataSource.queries()[this._queryIndex];
    }
    set sqlQuery(val) {
        if (val)
            val.parent = this.sqlDataSource;
        this.sqlDataSource.queries()[this._queryIndex] = val;
    }
    saveCustomQueries() {
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        return this.customQueries.length > 0 && this.customQueries.map(x => JSON.stringify(serializer.serialize(x)));
    }
    save() {
        return JSON.stringify(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(this.sqlDataSource));
    }
    constructor(sqlDataSourceJSON, queryName, requestWrapper) {
        super();
        this.sqlDataSourceJSON = sqlDataSourceJSON;
        this.customQueries = [];
        this.sqlDataSource = new SqlDataSource(sqlDataSourceJSON ? JSON.parse(sqlDataSourceJSON) : {}, undefined, requestWrapper);
        this._disposables.push(this.sqlDataSource);
        if (queryName) {
            this.sqlDataSource.queries().some((value, index) => {
                if (value.name() === queryName) {
                    this._queryIndex = index;
                    return true;
                }
                return false;
            });
        }
        else {
            this._queryIndex = this.sqlDataSource.queries().length;
        }
    }
}

;// ./dist/query-builder/wizard/dataSourceWizardState.js











const _defaultRestoreSqlDataSourceFromState = (state, requestWrapper, dataSourceId) => {
    const wrapper = new _SqlDataSourceWrapper(state.sqlDataSourceJSON, state.queryName, requestWrapper);
    state.name && wrapper.sqlDataSource.connection.name(state.name);
    const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
    if (state.customQueries && state.customQueries.length > 0) {
        wrapper.customQueries = state.customQueries.map((query) => wrapper.sqlDataSource.createQuery(JSON.parse(query), serializer));
        wrapper.sqlDataSource.queries.push(...wrapper.customQueries);
    }
    if (state.relations && state.relations.length > 0) {
        wrapper.sqlDataSource.relations.push(...state.relations.map((relation) => new MasterDetailRelation(JSON.parse(relation), serializer)));
    }
    wrapper.sqlDataSource.id = dataSourceId || (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)().replace(/-/g, '');
    return wrapper;
};
let _restoreSqlDataSourceFromState = _defaultRestoreSqlDataSourceFromState;
const _setRestoreSqlDataSourceFromState = (func) => { _restoreSqlDataSourceFromState = func; };
const _resetRestoreSqlDataSourceFromState = () => { _restoreSqlDataSourceFromState = _defaultRestoreSqlDataSourceFromState; };
const _defaultRestoreFederationDataSourceFromState = (state, usedDataSources, dataSourceId) => {
    const dataSource = new FederationDataSource(JSON.parse(state.federationDataSourceJSON), usedDataSources);
    if (dataSourceId) {
        dataSource.id = dataSourceId;
    }
    if (state.relations && state.relations.length > 0) {
        dataSource.relations.push(...state.relations.map((relation) => new FederationMasterDetailRelation(JSON.parse(relation), new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer())));
    }
    return dataSource;
};
const _restoreFederationDataSourceFromState = _defaultRestoreFederationDataSourceFromState;
const _defaultRestoreJsonDataSourceFromState = (state, requestWrapper, dataSourceId) => {
    const jsonDataSource = new JsonDataSource({
        'Source': state.jsonSource && JSON.parse(state.jsonSource) || {},
        'Schema': state.jsonScheme && JSON.parse(state.jsonScheme) || {},
        '@RootElement': state.rootElement || ''
    }, undefined, requestWrapper);
    state.connectionName && jsonDataSource.connectionName(state.connectionName);
    state.dataSourceName && jsonDataSource.name(state.dataSourceName);
    jsonDataSource.id = dataSourceId || (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)().replace(/-/g, '');
    return jsonDataSource;
};
let _restoreJsonDataSourceFromState = _defaultRestoreJsonDataSourceFromState;
function _setRestoreJsonDataSourceFromState(func) { _restoreJsonDataSourceFromState = func; }
function _resetRestoreJsonDataSourceFromState() { _restoreJsonDataSourceFromState = _defaultRestoreJsonDataSourceFromState; }
const _defaultRestoreMongoDBDataSourceFromState = (state, requestWrapper, dataSourceId) => {
    const dataSource = new MongoDBDataSource({
        'Connection': state.connectionJson && JSON.parse(state.connectionJson),
    }, undefined, requestWrapper);
    if (state.queries && state.queries.length > 0) {
        dataSource.queries.push(...state.queries.map((query) => new MongoDBQuery(JSON.parse(query), new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer())));
    }
    dataSource.name(state.dataSourceName || '');
    dataSource.id = dataSourceId || (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)().replace(/-/g, '');
    return dataSource;
};
let _restoreMongoDBDataSourceFromState = _defaultRestoreMongoDBDataSourceFromState;
function _setRestoreMongoDBDataSourceFromState(func) { _restoreMongoDBDataSourceFromState = func; }
function _resetRestoreMongoDBDataSourceFromState() { _restoreMongoDBDataSourceFromState = _defaultRestoreMongoDBDataSourceFromState; }
function _restoreObjectDataSourceFromState(state, requestWrapper, dataSourceId) {
    const objectDataSource = new ObjectDataSource();
    objectDataSource.setState(state);
    if (dataSourceId)
        objectDataSource.id = dataSourceId;
    return objectDataSource;
}
function _createDefaultDataSourceWizardState(sqlDataSourceWizardState = {}, jsonDataSourceWizardState = { jsonSource: '' }, objectDataSourceWizardState = {}, federationDataSourceWizardState = {}, mongoDBDataSourceWizardState = {}) {
    return {
        dataSourceType: DataSourceType.Sql,
        jsonDataSourceWizard: jsonDataSourceWizardState,
        sqlDataSourceWizard: sqlDataSourceWizardState,
        objectDataSourceWizard: objectDataSourceWizardState,
        federationDataSourceWizard: federationDataSourceWizardState,
        mongoDBDataSourceWizard: mongoDBDataSourceWizardState,
        dataSourceId: (0,external_DevExpress_Analytics_Internal_namespaceObject.guid)().replace(/-/g, '')
    };
}

;// ./dist/query-builder/wizard/pages/__wrappedWizardPage.js



class _WrappedWizardPage extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    dispose() {
        this.onChange = null;
        this[__loadingStateFunctionName] = null;
        this.page.dispose();
        this._initDef && this._initDef.reject();
        this._initDef = null;
    }
    resetCommitedState() {
        this._lastCommitedState = null;
    }
    constructor(pageId, page, template, description) {
        super();
        this.pageId = pageId;
        this.page = page;
        this.template = template;
        this.description = description;
        this._isInitialized = false;
        this._initDef = null;
        this.isChanged = true;
        if (page.onChange) {
            this.onChange = (callback) => page.onChange(callback);
        }
    }
    commit() {
        return this.page.commit().done((result) => {
            this.isChanged = JSON.stringify(this._lastCommitedState) !== JSON.stringify(result);
            this._lastCommitedState = result;
        });
    }
    initialize(state, force = false, stateChanged = false) {
        this._initDef && this._initDef.reject();
        this._initDef = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        if (!this._isInitialized || force) {
            this._isInitialized = true;
            this.page.initialize(state, stateChanged).fail(() => {
                this._isInitialized = false;
                this._initDef && this._initDef.reject();
            }).done((result) => {
                this._initDef && this._initDef.resolve(result);
            });
        }
        else {
            this._initDef.resolve();
        }
        return this._initDef.promise();
    }
}

;// ./dist/query-builder/wizard/pages/chooseSqlConnectionPage.js







class ChooseSqlConnectionPage extends WizardPageBase {
    constructor(connectionStrings, _getSqlConnectionsCallback) {
        super();
        this._getSqlConnectionsCallback = _getSqlConnectionsCallback;
        this._connectionStrings = external_ko_namespaceObject.observableArray([]);
        this._selectedConnectionString = external_ko_namespaceObject.observableArray([]);
        this._connectionStrings = connectionStrings;
        this._disposables.push(this._selectedConnectionString.subscribe(() => this._onChange()));
    }
    initialize(state) {
        const selectConnection = (connections) => {
            if (connections.length === 1) {
                this._selectedConnectionString([connections[0]]);
            }
            else {
                const selectedString = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(connections, 'name', state.name) || connections[0];
                this._selectedConnectionString(selectedString ? [selectedString] : []);
            }
        };
        if (this._getSqlConnectionsCallback) {
            const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
            this._getSqlConnectionsCallback().done(connections => {
                this._connectionStrings(connections);
                selectConnection(connections);
                deferred.resolve(connections);
            }).fail(() => {
                deferred.reject();
            });
            return deferred.promise();
        }
        const connectionStrings = this._connectionStrings();
        selectConnection(connectionStrings);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    canNext() {
        return this._selectedConnectionString().length !== 0;
    }
    commit() {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        if (this._selectedConnectionString()[0]) {
            deferred.resolve({
                name: this._selectedConnectionString()[0].name
            });
        }
        else {
            deferred.resolve();
        }
        return deferred.promise();
    }
}
function _registerChooseSqlConnectionPage(factory, connectionStrings, getSqlConnectionsCallback) {
    factory.registerMetadata(SqlDataSourceWizardPageId.ChooseConnectionPage, {
        create: () => {
            return new ChooseSqlConnectionPage(connectionStrings, getSqlConnectionsCallback);
        },
        setState: (data, state) => {
            state.name = data.name;
        },
        getState: (state) => {
            return state.sqlDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.name = defaultState.name;
        },
        template: 'dxrd-page-connectionstring',
        description: (0,external_DevExpress_Analytics_Internal_namespaceObject.localizeWithUpdateLocalizationMethod)('Choose a data connection') || (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose a data connection.', 'AnalyticsCoreStringId.SqlDSWizard_PageChooseConnection')
    });
}

;// ./dist/query-builder/wizard/internal/objectDataSource/_chooseObjectParameters.js









class ObjectDataSourceParameterProperty extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    switchPropertyType() {
        if (this.isExpression()) {
            this.type(this._parameter.resultType);
            this._parameter.value = this.value();
        }
        else {
            this.type(external_DevExpress_Analytics_Internal_namespaceObject.ExpressionType);
            this._parameter.value = this.expression.value();
        }
    }
    isExpression() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(this.type());
    }
    _generateInfo(info, propertyName = 'value') {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, {
            displayName: this._parameter.displayName,
            propertyName: propertyName,
            modelName: propertyName,
            editorOptions: {}
        }, info);
    }
    _initGetInfo(typeValue) {
        const info = [
            this._generateInfo({
                editor: (0,external_DevExpress_Analytics_Internal_namespaceObject.getEditorType)(this._parameter.resultType),
                from: (val) => external_ko_namespaceObject.observable((typeValue.valueConverter || ((val) => val))(val)),
                defaultVal: !this.isExpression() && this._parameter.value || typeValue.defaultValue
            }),
            this._generateInfo({
                editor: { header: 'dx-objectdatasource-expression-popup' },
                from: (val) => {
                    return { value: external_ko_namespaceObject.observable(val), itemsProvider: this._itemsProvider };
                },
                defaultVal: this.isExpression() && this._parameter.value || ''
            }, 'expression')
        ];
        this._disposables.push({ dispose: () => this.getInfo = undefined });
        this.getInfo = () => info;
    }
    _patchOriginalParameterValue(value) {
        const type = this._parameter.resultType;
        if (value && (0,external_DevExpress_Analytics_Utils_namespaceObject.isDateType)(this._parameter.resultType) && value instanceof Date)
            this._parameter.value = (0,external_DevExpress_Analytics_Utils_namespaceObject.serializeParameterDate)(value, type);
        else
            this._parameter.value = value;
    }
    _afterInitialize() {
        if (!this.isExpression()) {
            this._patchOriginalParameterValue(this.value());
        }
    }
    _subscribeProperties() {
        this._disposables.push(this.type.subscribe((newVal) => {
            this._parameter.type = newVal;
        }), this.expression.value.subscribe((newVal) => {
            this.isExpression() && (this._patchOriginalParameterValue(newVal));
        }), this.value.subscribe((newVal) => {
            !this.isExpression() && (this._patchOriginalParameterValue(newVal));
        }));
    }
    isPropertyVisible(propertyName) {
        return this.isExpression() ? propertyName === 'expression' : propertyName === 'value';
    }
    constructor(_parameter, _itemsProvider) {
        super();
        this._parameter = _parameter;
        this._itemsProvider = _itemsProvider;
        this.type = external_ko_namespaceObject.observable('');
        this.type(this._parameter.type);
        const typeValue = DataSourceParameter._getTypeValue(this._parameter.resultType);
        this._initGetInfo(typeValue);
        new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().deserialize(this, {});
        this._subscribeProperties();
        this._afterInitialize();
    }
}
class ObjectDataSourceParameterEditorAddon {
    _doWithModel(action) {
        const model = this._editor._get('_model');
        if (model instanceof ObjectDataSourceParameterProperty) {
            return action(model);
        }
    }
    constructor(_editor) {
        this._editor = _editor;
        this.imageTemplateName = 'dx-objectdatasource-expression';
    }
    switchEditors() {
        this._doWithModel((model) => model.switchPropertyType());
    }
    isExpression() {
        return this._doWithModel((model) => model.isExpression());
    }
}
class ObjectDataSourceParameterGrid extends external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties {
    constructor(x) {
        super(external_ko_namespaceObject.observable(x));
        this.createEditorAddOn = (_editor) => {
            const editor = (0,external_DevExpress_Analytics_Widgets_namespaceObject.unwrapEditor)(_editor);
            const addon = new ObjectDataSourceParameterEditorAddon(editor);
            return {
                data: addon,
                templateName: 'dx-wizard-menu-box-editorswitch'
            };
        };
    }
}
class ObjectDataSourceParametersModel extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(parametersMethod, itemsProvider) {
        super();
        this.displayName = parametersMethod.displayName;
        this._grids = parametersMethod.parameters.map(x => {
            const param = new ObjectDataSourceParameterProperty(x, itemsProvider);
            const grid = new ObjectDataSourceParameterGrid(param);
            this._disposables.push(param);
            this._disposables.push(grid);
            return grid;
        });
    }
}
class ChooseObjectMemberParameters extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _updateParameters(propertyName, method) {
        this[propertyName]() && this[propertyName]().dispose();
        if (method && method.parameters.length > 0)
            this[propertyName](new ObjectDataSourceParametersModel(method, this._itemsProvider));
        else
            this[propertyName](null);
    }
    constructor(_itemsProvider) {
        super();
        this._itemsProvider = _itemsProvider;
        this._ctorParametersObject = external_ko_namespaceObject.observable();
        this._dataMemberParametersObject = external_ko_namespaceObject.observable();
        this.hasParameters = () => {
            return this._dataMemberParametersObject() || this._ctorParametersObject();
        };
    }
    updateCtorParameters(method) {
        this._updateParameters('_ctorParametersObject', method);
    }
    updateMethodParameters(method) {
        this._updateParameters('_dataMemberParametersObject', method);
    }
}
class ChooseObjectParameters extends ChooseObjectMemberParameters {
    constructor(selectedCtor, selectedDataMembers, itemsProvider) {
        super(itemsProvider);
        this._disposables.push(selectedCtor.subscribe((ctor) => {
            this.updateCtorParameters(ctor);
        }));
        this._disposables.push(selectedDataMembers.subscribe(a => {
            this.updateMethodParameters(a[0]);
        }));
    }
}

;// ./dist/query-builder/wizard/pages/objectDataSourceWizard/configureObjectDataSourceParametersPage.js






class ConfigureObjectDataSourceParametersPage extends WizardPageBase {
    constructor(itemsProvider) {
        super();
        this._objectDataSource = new ObjectDataSource();
        this._disposables.push(this._chooseObjectParameters = new ChooseObjectMemberParameters(itemsProvider));
    }
    canNext() {
        return false;
    }
    canFinish() {
        return true;
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            selectedType: this._objectDataSource.selectedType,
            ctor: this._objectDataSource.ctor,
            dataMember: this._objectDataSource.dataMember,
            dataSourceName: this._objectDataSource.name(),
        }).promise();
    }
    initialize(state) {
        this._objectDataSource.setState(state);
        this._chooseObjectParameters.updateCtorParameters(this._objectDataSource.ctor);
        this._chooseObjectParameters.updateMethodParameters(this._objectDataSource.dataMember);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
}
function _registerConfigureObjectDataSourceParametersPage(factory, getItemsProviderCallback) {
    factory.registerMetadata(ObjectDataSourceWizardPageId.ConfigureParametersPage, {
        setState: (data, state) => {
            state.ctor = data.ctor;
            state.dataMember = data.dataMember;
            state.selectedType = data.selectedType;
        },
        getState: (state) => {
            return state.objectDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.dataMember = defaultState.dataMember;
        },
        create: () => {
            return new ConfigureObjectDataSourceParametersPage(getItemsProviderCallback && getItemsProviderCallback());
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure constructor parameters and/or method parameters.', 'AnalyticsCoreStringId.ObjectDSWizard_ConfigureParameters_Description'),
        template: 'dx-objectdatasource-configureparameters-page'
    });
}

;// ./dist/query-builder/wizard/internal/objectDataSource/_objectSchemaProvider.js









class ObjectTypeDescriptions {
    constructor(model) {
        const types = deserializeToCollection(model, (key) => new ObjectType(key));
        this.types = types.sort((a, b) => { return a.displayName.localeCompare(b.displayName); });
    }
}
function getObjectTypeDescriptionsCallback(requestWrapper, context) {
    const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
    requestWrapper.getObjectTypeDescriptions(context)
        .done(data => {
        try {
            const objectTypeDescriptionsModel = new ObjectTypeDescriptions(data.objectDataSourceInfoJson);
            deferred.resolve(objectTypeDescriptionsModel);
        }
        finally {
            if (deferred.state() === 'pending')
                deferred.reject();
        }
    })
        .fail(data => {
        (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Schema loading failed. {0}', 'DxDesignerStringId.Error_SchemaLoadingFailed'), (0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data)));
        deferred.reject();
    });
    return deferred.promise();
}
class ObjectSchemaProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(_requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this.getItems = (pathRequest) => {
            const getItemsDeferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            const loadSchemaPromise = this.getObjectTypeDescriptions();
            loadSchemaPromise
                .done((objectSchema) => {
                getItemsDeferred.resolve(this.getSchemaByPath(pathRequest, objectSchema));
            })
                .fail(getItemsDeferred.reject);
            return getItemsDeferred.promise();
        };
    }
    dispose() {
        this._objectTypeDescriptionsPromise = null;
    }
    getSchemaByPath(pathRequest, objectSchema) {
        if (!pathRequest.fullPath) {
            return objectSchema.types.map(type => (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, type, { isList: true, specifics: 'List' }));
        }
        else {
            let currentCtros = [];
            for (let i = 0; i < pathRequest.pathParts.length; i++) {
                const pathPart = (objectSchema.types || []).filter((type) => type.name == pathRequest.pathParts[i])[0];
                if (!pathPart)
                    return [];
                currentCtros = pathPart.ctors;
            }
            return currentCtros.map(ctor => (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, ctor, { isList: false, specifics: 'default' }));
        }
    }
    getObjectTypeDescriptions(context = '') {
        if (!this._objectTypeDescriptionsPromise || this._objectTypeDescriptionsPromise.state() === 'rejected')
            this._objectTypeDescriptionsPromise = getObjectTypeDescriptionsCallback(this._requestWrapper, context);
        return this._objectTypeDescriptionsPromise;
    }
}

;// ./dist/query-builder/wizard/internal/objectDataSource/_chooseObjectTypes.js



class ChooseObjectTypesTreelistController extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListController {
    canSelect(value) {
        return true;
    }
}
class ChooseObjectTypes extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(types, provider) {
        super();
        this.types = types;
        this.selectedType = external_ko_namespaceObject.observable();
        this.selectedCtor = external_ko_namespaceObject.observable();
        this.selectedPath = external_ko_namespaceObject.observable('');
        this._scrollViewHeight = 'calc(100% - 36px)';
        this.availableTypesTreelistModel = {
            expandRootItems: true,
            itemsProvider: provider,
            selectedPath: this.selectedPath,
            treeListController: new ChooseObjectTypesTreelistController()
        };
        this._disposables.push(this.selectedPath.subscribe(newValue => {
            types().forEach((type) => {
                if (type.name == newValue) {
                    this.selectedType(type);
                    if (this.selectedCtor() == null)
                        this.selectedCtor.valueHasMutated();
                    else
                        this.selectedCtor(null);
                }
                else {
                    type.ctors.forEach(ctor => {
                        if (type.name.concat('.').concat(ctor.name) == newValue) {
                            this.selectedType(type);
                            this.selectedCtor(ctor);
                        }
                    });
                }
            });
        }));
    }
}

;// ./dist/query-builder/wizard/pages/objectDataSourceWizard/chooseObjectDataSourceTypesPage.js










class ChooseObjectDataSourceTypesPage extends WizardPageBase {
    constructor(_requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this._objectDataSource = new ObjectDataSource();
        this._types = external_ko_namespaceObject.observableArray([]);
        this._disposables.push(this._provider = new ObjectSchemaProvider(this._requestWrapper), this._chooseObjectType = new ChooseObjectTypes(this._types, this._provider));
    }
    canNext() {
        return !!this._selectedTypeName;
    }
    canFinish() {
        return false;
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            selectedType: this._selectedTypeName,
            selectedObjectType: this._chooseObjectType.selectedType(),
            ctor: this._chooseObjectType.selectedCtor()
        }).promise();
    }
    initialize(state) {
        this._objectDataSource.setState(state);
        this._provider.getObjectTypeDescriptions(state.context).done((schema) => {
            schema.types.forEach(type => type.members.splice(0, 0, ObjectDataMember.empty()));
            this._types(schema.types || []);
            if (schema.types.length > 0) {
                this._chooseObjectType.selectedPath(schema.types[0].name);
            }
        });
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    get _selectedTypeName() {
        return !!this._chooseObjectType.selectedType() ? this._chooseObjectType.selectedType().name : null;
    }
}
function _registerChooseObjectDataSourceTypesPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(ObjectDataSourceWizardPageId.ChooseTypesPage, {
        setState: (data, state) => {
            state.selectedType = data.selectedType;
            state.selectedObjectType = data.selectedObjectType;
            state.ctor = data.ctor;
        },
        getState: (state) => {
            return state.objectDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.selectedType = defaultState.selectedType;
            state.selectedObjectType = defaultState.selectedObjectType;
            state.ctor = defaultState.ctor;
        },
        create: () => {
            return new ChooseObjectDataSourceTypesPage(dataSourceWizardOptions.requestWrapper);
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose the type and its constructor.', 'AnalyticsCoreStringId.ObjectDSWizard_ChooseType_Description'),
        template: 'dxrd-page-objectdatasource-types'
    });
}

;// ./dist/query-builder/wizard/internal/objectDataSource/_chooseObjectDataMembers.js


class ChooseObjectDataMembers extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(type, ctor) {
        super();
        this.dataMembers = external_ko_namespaceObject.observableArray([]);
        this.selectedDataMembers = external_ko_namespaceObject.observableArray([]);
        let oldType = type() && type().name;
        this._disposables.push(ctor.subscribe(newVal => {
            type().updateMembers(newVal);
            if (oldType != type().name) {
                this.dataMembers(type().members);
                this.selectedDataMembers([]);
                oldType = type().name;
            }
            this.coerceSelection();
        }));
    }
    coerceSelection() {
        const selectedMember = this.selectedDataMembers()[0];
        const bestItem = selectedMember && selectedMember.isAvailable() ? selectedMember : this.dataMembers().filter(a => a.isAvailable())[0];
        this.selectedDataMembers([bestItem]);
    }
}

;// ./dist/query-builder/wizard/pages/objectDataSourceWizard/chooseObjectDataSourceDataMembersPage.js








class ChooseObjectDataSourceDataMembersPage extends WizardPageBase {
    constructor(_requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this._objectDataSource = new ObjectDataSource();
        this._type = external_ko_namespaceObject.observable();
        this._ctor = external_ko_namespaceObject.observable();
        this._disposables.push(this._chooseObjectDataMember = new ChooseObjectDataMembers(this._type, this._ctor));
    }
    initialize(state) {
        this._objectDataSource.setState(state);
        this._type(state.selectedObjectType);
        this._ctor(state.ctor);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    canNext() {
        return !!this._needParametersPage;
    }
    canFinish() {
        return !this._needParametersPage;
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            selectedType: this._type().name,
            ctor: this._ctor(),
            dataMember: !this._selectedDataMember || this._selectedDataMember.isEntireObject() ? undefined : this._selectedDataMember,
        }).promise();
    }
    get _selectedDataMember() {
        const selectedDataMembers = this._chooseObjectDataMember.selectedDataMembers();
        return !!selectedDataMembers ? selectedDataMembers[0] : null;
    }
    get _needParametersPage() {
        return (!!this._selectedDataMember && this._selectedDataMember.parameters.length > 0) ||
            (!!this._ctor() && this._ctor().parameters.length > 0);
    }
}
function _registerChooseObjectDataSourceDataMembersPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(ObjectDataSourceWizardPageId.ChooseDataMembersPage, {
        setState: (data, state) => {
            state.selectedType = data.selectedType;
            state.ctor = data.ctor;
            state.dataMember = data.dataMember;
        },
        getState: (state) => {
            return state.objectDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.dataMember = defaultState.dataMember;
        },
        create: () => {
            return new ChooseObjectDataSourceDataMembersPage(dataSourceWizardOptions.requestWrapper);
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose the entire object or a data member to bind.', 'AnalyticsCoreStringId.ObjectDSWizard_ChooseDataMember_Description'),
        template: 'dxrd-page-objectdatasource-datamembers'
    });
}

;// external "DevExpress.Analytics.Serializer.Native"
const external_DevExpress_Analytics_Serializer_Native_namespaceObject = DevExpress.Analytics.Serializer.Native;
;// ./dist/query-builder/wizard/internal/_utils.js
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var WizardSectionPosition;
(function (WizardSectionPosition) {
    WizardSectionPosition[WizardSectionPosition["Left"] = 1] = "Left";
    WizardSectionPosition[WizardSectionPosition["TopLeft"] = 2] = "TopLeft";
    WizardSectionPosition[WizardSectionPosition["BottomLeft"] = 3] = "BottomLeft";
    WizardSectionPosition[WizardSectionPosition["Right"] = 4] = "Right";
    WizardSectionPosition[WizardSectionPosition["TopRight"] = 5] = "TopRight";
    WizardSectionPosition[WizardSectionPosition["BottomRight"] = 6] = "BottomRight";
    WizardSectionPosition[WizardSectionPosition["Top"] = 7] = "Top";
    WizardSectionPosition[WizardSectionPosition["Bottom"] = 8] = "Bottom";
})(WizardSectionPosition || (WizardSectionPosition = {}));
class CustomQueryTreeListItem extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListItemViewModel {
    _getTemplateName() {
        return this._getCustomizedTemplateName(true);
    }
    _getCustomizedTemplateName(isEditable) {
        return isEditable ? 'dxd-custom-query-treelisitem' : super._getTemplateName();
    }
    updateViewModel(args) {
        super.updateViewModel(args);
        const viewModel = this.getViewModel();
        viewModel.queryName = this.queryName;
    }
    createViewModel() {
        return (0,external_DevExpress_Analytics_Serializer_Native_namespaceObject.createViewModelGenerator)(super.createViewModel())
            .generateProperty('queryName', this.queryName)
            .generateProperty('queryNameHasChanged', (event) => this.queryName = event.value)
            .getViewModel();
    }
    onPropertyChanged(args) {
        super.onPropertyChanged(args);
        if (args.propertyName === 'data') {
            this.queryName = this.data?.name || '';
        }
        if (args.propertyName === 'queryName') {
            const propertyChangedEventArgs = args;
            const data = this.data;
            if (data && data.name !== propertyChangedEventArgs.newValue) {
                data.name = propertyChangedEventArgs.newValue;
                data['_afterCheckToggled'] && data['_afterCheckToggled'](data);
            }
        }
    }
}
__decorate([
    (0,external_DevExpress_Analytics_Serializer_Native_namespaceObject.mutable)('')
], CustomQueryTreeListItem.prototype, "queryName", void 0);
class MultiQueryTreeListItemFactory extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.KoTreeListItemFactory {
    createItem(options, path, onItemsVisibilityChanged, rtl, resolver) {
        if (path[0] === 'queries') {
            return new CustomQueryTreeListItem((0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.wrapTreeListOptionsWithKo)(options), path, onItemsVisibilityChanged, rtl, resolver);
        }
        return super.createItem(options, path, onItemsVisibilityChanged, rtl, resolver);
    }
}
function getSectionStyle(position, defaultMargin = 30, isVisible = true) {
    const _clearStyle = 'inherit';
    const _defaultHalfMargin = '-' + (defaultMargin / 2) + 'px';
    const _fullSize = 100;
    const _defaultSize = _fullSize / 2;
    const _inPercent = (size) => {
        return size + '%';
    };
    if (!position) {
        return {
            top: _defaultHalfMargin,
            bottom: _defaultHalfMargin,
            left: _defaultHalfMargin,
            right: _defaultHalfMargin,
            width: _clearStyle,
            height: _clearStyle,
            display: isVisible ? 'block' : 'none'
        };
    }
    return {
        top: (position === WizardSectionPosition.Left || position === WizardSectionPosition.Right || position === WizardSectionPosition.Top || position === WizardSectionPosition.TopLeft || position === WizardSectionPosition.TopRight) ? _defaultHalfMargin : _clearStyle,
        bottom: (position === WizardSectionPosition.Left || position === WizardSectionPosition.Right || position === WizardSectionPosition.Bottom || position === WizardSectionPosition.BottomLeft || position === WizardSectionPosition.BottomRight) ? _defaultHalfMargin : _clearStyle,
        left: (position === WizardSectionPosition.Top || position === WizardSectionPosition.Bottom || position === WizardSectionPosition.Left || position === WizardSectionPosition.TopLeft || position === WizardSectionPosition.BottomLeft) ? _defaultHalfMargin : _clearStyle,
        right: (position === WizardSectionPosition.Top || position === WizardSectionPosition.Bottom || position === WizardSectionPosition.Right || position === WizardSectionPosition.TopRight || position === WizardSectionPosition.BottomRight) ? _defaultHalfMargin : _clearStyle,
        width: (position === WizardSectionPosition.Top || position === WizardSectionPosition.Bottom) ? _clearStyle : _inPercent(_defaultSize),
        height: (position === WizardSectionPosition.Left || position === WizardSectionPosition.Right) ? _clearStyle : _inPercent(_defaultSize),
        display: isVisible ? 'block' : 'none'
    };
}
function subscribeArray(array, subscribeItem, onChange) {
    array().forEach((item) => subscribeItem(item, onChange));
    return array.subscribe((changeSet) => {
        changeSet.forEach((change) => {
            if (change.status === 'added') {
                subscribeItem(change.value, onChange);
            }
            else if (change.status === 'deleted') {
                change.value['dispose'] && change.value['dispose']();
            }
        });
        onChange();
    }, null, 'arrayChange');
}
function subscribeProperties(properties, onChange) {
    const subscriptions = [];
    properties.forEach((property) => {
        if (property && property.subscribe) {
            subscriptions.push(property.subscribe((val) => onChange(val)));
        }
    });
    return subscriptions;
}
function subscribeObject(object, subscribeProperties, onChange) {
    subscribeProperties(object(), onChange);
    return object.subscribe((newVal) => {
        subscribeProperties(newVal, onChange);
        onChange();
    });
}
function _createBeforeInitializePageEventArgs(page, self) {
    return {
        page: page.page,
        pageId: page.pageId,
        wizard: self,
        state: self.stateManager.getPageState(page.pageId)
    };
}
function _createPageEventArgs(page, self) {
    return {
        page: page.page,
        pageId: page.pageId,
        wizard: self
    };
}
function _isMoreThanOneDataSourceTypeAvailable(dataSourceOptions) {
    return [dataSourceOptions.jsonDataSourceAvailable, dataSourceOptions.sqlDataSourceAvailable, dataSourceOptions.mongoDBDataSourceAvailable,
        dataSourceOptions.objectDataSourceAvailable, dataSourceOptions.federationDataSourceAvailable].filter(x => !!x).length > 1;
}

;// ./dist/query-builder/wizard/chooseAvailablePage.js






class ChooseAvailableItemPage extends WizardPageBase {
    constructor(items, canCreateNew = true, _getJsonConnectionsCallback) {
        super();
        this.items = items;
        this._getJsonConnectionsCallback = _getJsonConnectionsCallback;
        this.selectedItems = external_ko_namespaceObject.observableArray([]);
        this.operations = [
            { text: this.existingOperationText, createNew: false },
            { text: this.createNewOperationText, createNew: true }
        ];
        this.selectedOperation = external_ko_namespaceObject.observable(this.operations[0]);
        this._createNew = external_ko_namespaceObject.pureComputed(() => this.selectedOperation().createNew);
        this.canCreateNew = external_ko_namespaceObject.observable(canCreateNew);
        this._disposables.push(...subscribeProperties([this.selectedOperation, this.selectedItems], () => this._onChange()));
    }
    canNext() {
        return this.selectedItems().length !== 0 || this.selectedOperation().createNew;
    }
    initialize(state) {
        if (this._getJsonConnectionsCallback) {
            const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
            this._getJsonConnectionsCallback().done(connections => {
                this.items(connections);
                const item = this._getSelectedItem(state);
                this.selectedItems(item ? [item] : []);
                deferred.resolve(connections);
            }).fail(() => {
                deferred.reject();
            });
            return deferred.promise();
        }
        const item = this._getSelectedItem(state);
        this.selectedItems(item ? [item] : []);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve(this).promise();
    }
    _displayExpr(item) {
        return item.description || item.name;
    }
    _getSelectedItem(state) {
        return this.items()[0];
    }
    onDblClick() {
        this[__nextActionFunctionName] && this[__nextActionFunctionName]();
    }
    get createNewOperationText() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)("No, I'd like to create a new data source", 'AnalyticsCoreStringId.Wizard_CreateNewDataSource');
    }
    get existingOperationText() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Yes, let me choose an existing data source from the list', 'AnalyticsCoreStringId.Wizard_ChooseDataSourceFromList');
    }
}

;// ./dist/query-builder/wizard/chooseAvailableDataSourcePage.js




class ChooseAvailableDataSourcePage extends ChooseAvailableItemPage {
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            predefinedDataSourceName: this.selectedOperation().createNew ? null : this.selectedItems()[0].name
        }).promise();
    }
    _getSelectedItem(state) {
        const availableDataSources = this.items() || [];
        if (state.predefinedDataSourceName) {
            return availableDataSources.filter(x => x.name === state.predefinedDataSourceName)[0];
        }
        else if (availableDataSources.length === 0) {
            return null;
        }
        else {
            return availableDataSources[0];
        }
    }
    canNext() {
        return this.selectedOperation().createNew;
    }
    canFinish() {
        return !this.selectedOperation().createNew;
    }
}
function _registerChooseAvailableDataSourcePage(factory, wizardOptions) {
    factory.registerMetadata(DataSourceWizardPageId.ChoosePredefinedDataSourcePage, {
        setState: (data, state) => {
            state.predefinedDataSourceName = data.predefinedDataSourceName;
        },
        getState: (state) => {
            return state;
        },
        resetState: (state, defaultState) => {
            state.predefinedDataSourceName = null;
        },
        create: () => {
            return new ChooseAvailableDataSourcePage(wizardOptions.predefinedDataSources, wizardOptions.canCreateDataSource);
        },
        alwaysShowTitle: true,
        template: 'dxrd-page-selectitems',
        description: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Do you want to use an existing data source?', 'AnalyticsCoreStringId.Wizard_UseExisting_DataSource') + ' ' +
            (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('The Wizard assigns the selected or a newly created data source to the report.', 'AnalyticsCoreStringId.Wizard_DataSourceAssignment_Description')
    });
}

;// ./dist/query-builder/wizard/pages/jsonDataSourceWizard/chooseJsonConnectionPage.js





class ChooseJsonConnectionPage extends ChooseAvailableItemPage {
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            connectionName: !this.selectedOperation().createNew ? this.selectedItems()[0].name : null
        }).promise();
    }
    _getSelectedItem(data) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.items(), 'name', data.connectionName) || super._getSelectedItem();
    }
    get createNewOperationText() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)("No, I'd like to create a new data connection", 'AnalyticsCoreStringId.JsonDSWizard_CreateNewConnection');
    }
    get existingOperationText() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Yes, let me choose an existing data connection from the list', 'AnalyticsCoreStringId.JsonDSWizard_UseExistingConnection');
    }
}
function _registerChooseJsonConnectionPage(factory, wizardOptions) {
    factory.registerMetadata(JsonDataSourceWizardPageId.ChooseConnectionPage, {
        create: () => {
            return new ChooseJsonConnectionPage(wizardOptions.connectionStrings && wizardOptions.connectionStrings.json, wizardOptions.allowCreateNewJsonConnection, wizardOptions.getJsonConnectionStrings);
        },
        description: wizardOptions.allowCreateNewJsonConnection ?
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Do you want to use an existing data connection?', 'AnalyticsCoreStringId.JsonDSWizard_ChooseConnection_Description') :
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose a data connection.', 'AnalyticsCoreStringId.SqlDSWizard_PageChooseConnection'),
        getState: (state) => state.jsonDataSourceWizard,
        setState: (data, state) => state.connectionName = data.connectionName,
        resetState: (state, defaultState) => {
            state.connectionName = defaultState.connectionName;
        },
        template: 'dxrd-page-selectitems'
    });
}

;// ./dist/query-builder/wizard/internal/jsonSourceSettings.js
const parameterTypeToPropertyMap = {
    'PathParameter': 'pathParameters',
    'QueryParameter': 'queryParameters',
    'Header': 'headers',
};

;// ./dist/query-builder/wizard/internal/_jsonDataSourceWidgets.js






function getLocalizedValidationErrorMessage(emptyValueErrorMessage, localizedPropertyName, subProperty) {
    const requiredMessageSuffix = emptyValueErrorMessage || (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('The value cannot be empty', 'AnalyticsCoreStringId.ParametersPanel_DateTimeValueValidationError');
    if (!localizedPropertyName)
        return requiredMessageSuffix;
    let propertyNamesPrefix = !subProperty ? localizedPropertyName : (0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)('{0}. {1}', localizedPropertyName, subProperty);
    if (!(0,external_DevExpress_Analytics_Utils_namespaceObject._stringEndsWith)(propertyNamesPrefix, ':'))
        propertyNamesPrefix += ':';
    return (0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)('{0} {1}', propertyNamesPrefix, requiredMessageSuffix);
}
class JsonStringEditor extends external_DevExpress_Analytics_Widgets_namespaceObject.Editor {
    constructor(modelPropertyInfo, level, parentDisabled, textToSearch) {
        super(modelPropertyInfo, level, parentDisabled, textToSearch);
        this.aceEditorHasErrors = external_ko_namespaceObject.observable(false);
        this.aceAvailable = (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.aceAvailable)();
        this.editorContainer = external_ko_namespaceObject.observable();
        this.languageHelper = {
            getLanguageMode: () => 'ace/mode/json',
            createCompleters: () => { return []; }
        };
        this.aceOptions = {
            showLineNumbers: false,
            highlightActiveLine: false,
            showPrintMargin: false,
            enableBasicAutocompletion: true,
            enableLiveAutocompletion: true
        };
        this.isValid = external_ko_namespaceObject.computed(() => {
            return this._model() && this._model().isValid();
        });
        this.additionalOptions = {
            onChangeAnnotation: (session) => {
                const annotations = session && session.getAnnotations() || [];
                this._model() && this._model().aceEditorHasErrors && this._model().aceEditorHasErrors(annotations.filter(annotation => annotation.type === 'error' || annotation.type === 'warning').length > 0);
            },
            onBlur: () => {
                const editorContainer = this.editorContainer();
                if (editorContainer) {
                    this.value(editorContainer.getValue());
                }
            }
        };
        this.jsonStringValidationRules = [{
                type: 'custom',
                reevaluate: true,
                validationCallback: (options) => { return this.isValid(); },
                get message() {
                    return getLocalizedValidationErrorMessage((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('The value cannot be empty and should have a valid format.', 'AnalyticsCoreStringId.ValueIsRequiredOrInvalidFormat_Error'), (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('JSON String:', 'DataAccessUIStringId.WizardPageChooseJsonSource_Custom'));
                }
            }];
        this.showDropBorders = external_ko_namespaceObject.observable(false);
    }
    uploadFile(e) {
        if (e && e.event) {
            e.event.stopPropagation();
            e.event.preventDefault();
        }
        (0,external_DevExpress_Analytics_Internal_namespaceObject.uploadFile)({
            accept: '.json,.txt'
        }).done((result) => {
            const fileContent = (0,external_DevExpress_Analytics_Internal_namespaceObject.base64DecodeUnicode)(result.content);
            this.value(fileContent);
        });
    }
    getUploadTitle() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Upload JSON File', 'AnalyticsCoreStringId.UploadJsonFile_Title');
    }
}

;// ./dist/query-builder/wizard/internal/_jsonSourceSettings.js














class JsonDataSourceJsonSourcePageSettingsBase extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    dispose() {
        this._validationSummary && this._validationSummary.dispose();
        this._validationGroup && this._validationGroup.dispose();
        this._validationSummary = null;
        this._validationGroup = null;
        super.dispose();
    }
    _onValidationGroupInitialized(args) {
        this._validationGroup = args.component;
    }
    _onValidationGroupDisposing(args) {
        this._validationGroup = null;
    }
    _onValidationSummaryInitialized(args) {
        this._validationSummary = args.component;
    }
    _onValidationSummaryDisposing(args) {
        this._validationSummary = null;
    }
    _repaintSummary() {
        this._validationSummary && this._validationSummary.repaint();
    }
    _validate() {
        this._validationSummary && this._validationGroup && this._validationGroup.validate();
    }
    constructor() {
        super();
        this._validationGroup = null;
        this._validationSummary = null;
        this.validationGroup = {
            onInitialized: (args) => this._onValidationGroupInitialized(args),
            onDisposing: (args) => this._onValidationGroupDisposing(args),
            validate: () => this._validate()
        };
        this.validationSummary = {
            onInitialized: (args) => this._onValidationSummaryInitialized(args),
            onDisposing: (args) => this._onValidationSummaryDisposing(args)
        };
        this._disposables.push(this.grid = new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(external_ko_namespaceObject.observable(this)));
    }
}
class JsonDataSourceJsonSourcePageStringSettings extends JsonDataSourceJsonSourcePageSettingsBase {
    onChange(_onChange) {
        let timeoutId = null;
        const localOnChange = () => {
            _onChange();
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => this._validate(), 1);
        };
        this._disposables.push(this.stringSource.subscribe((newVal) => localOnChange()));
    }
    _isJsonSourceValid(jsonString) {
        if (!jsonString)
            return false;
        let isJsonSourceValid = true;
        try {
            JSON.parse(jsonString);
        }
        catch (ex) {
            isJsonSourceValid = false;
        }
        return isJsonSourceValid;
    }
    isEmpty() {
        return !this.stringSource();
    }
    reset() {
        this.stringSource('');
    }
    setValue(dataSource) {
        this.stringSource(dataSource.source.json());
    }
    getInfo() {
        return [
            {
                propertyName: 'stringSource', defaultVal: '', displayName: 'JSON String', editor: {
                    header: 'dx-jsonwizard-jsonstring-editor', editorType: JsonStringEditor, custom: 'dx-property-json-string-editor'
                }
            }
        ];
    }
    applySettings(jsonDataSource) {
        jsonDataSource.source.uri(undefined);
        jsonDataSource.source.json(this.stringSource());
        jsonDataSource.source.authenticationInfo = new JsonAuthenticationInfo({});
        jsonDataSource.source.headers([]);
        jsonDataSource.source.queryParameters([]);
        jsonDataSource.source.pathParameters([]);
    }
    constructor() {
        super();
        this._validatorsReady = external_ko_namespaceObject.observable(false);
        this.isValid = external_ko_namespaceObject.pureComputed(() => {
            const isJsonValid = this._isJsonSourceValid(this.stringSource());
            const aceHasErrors = this.aceEditorHasErrors();
            return isJsonValid && !aceHasErrors;
        });
        this.validationGroup = null;
        this.validationSummary = null;
        this.stringSource = external_ko_namespaceObject.observable('');
        this.aceEditorHasErrors = external_ko_namespaceObject.observable(false);
        this.cssClass = { 'dxrd-wizard-json-string-source-grid': true };
        this._disposables.push(this.grid = new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(external_ko_namespaceObject.observable(this)));
    }
}
class JsonDataSourceJsonSourcePageUriSettings extends JsonDataSourceJsonSourcePageSettingsBase {
    _getPatchedParameter(parameter) {
        parameter.nameValidationRules = [{
                type: 'required',
                get message() {
                    return getLocalizedValidationErrorMessage(null, (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Parameters', 'AnalyticsCoreStringId.QueryBuilder_Parameters'), this._collectionItemNamePlaceholder);
                }
            }];
        parameter.itemsProvider = this._itemsProvider;
        return parameter;
    }
    _validateUriSource() {
        const defaultValidationErrorMessage = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Invalid URI.', 'AnalyticsCoreStringId.ReportDesigner_Wizard_JsonSource_UriValidationError');
        const endpointUriDisplayName = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Web Service Endpoint (URI):', 'DataAccessUIStringId.WizardPageChooseJsonSource_URI');
        this._isUriValid(false);
        if (this._lastValidateDeferred) {
            this._lastValidateDeferred.reject();
        }
        const resultDeferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        this._lastValidateDeferred = resultDeferred;
        try {
            const jsonDataSource = new JsonDataSource({});
            const serializedJsonSource = this._getSerializedUriSource(jsonDataSource);
            this._lastValidatedJsonSourceJSON = serializedJsonSource;
            const validationResultHandler = (data) => {
                if (resultDeferred.state && resultDeferred.state() === 'rejected')
                    return;
                this._isUriValid(data.isUriValid);
                const faultMessage = getLocalizedValidationErrorMessage((data.isUriValid ? '' : data && data.faultMessage) || defaultValidationErrorMessage, endpointUriDisplayName);
                resultDeferred.resolve({
                    isUriValid: data.isUriValid,
                    faultMessage: faultMessage
                });
            };
            this._requestWrapper.validateJsonUri(jsonDataSource)
                .done(validationResultHandler)
                .fail((data = {}) => {
                data.isValid = false;
                validationResultHandler(data);
            });
        }
        catch (ex) {
            this._isUriValid(false);
        }
        return resultDeferred.promise();
    }
    _isCollectionValid(collectionName) {
        return !this[collectionName]().length || this[collectionName]().every(x => x.name());
    }
    _isParametersValid() {
        return this._isCollectionValid('parameters');
    }
    _isBasicHttpAuthValid() {
        return !this.basicHttpAuth.password() || !!this.basicHttpAuth.userName();
    }
    _getSourceUriInfo() {
        const sourceUri = {
            propertyName: 'sourceUri',
            displayName: 'Web Service Endpoint (URI):',
            localizationId: 'DataAccessUIStringId.WizardPageChooseJsonSource_URI',
            defaultVal: '',
            editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text'),
            validatorOptions: null,
            isRequired: true,
            editorOptions: {
                elementAttr: {
                    title: this.sourceUri
                }
            }
        };
        const _self = this;
        sourceUri.validatorOptions = {
            onInitialized: (e) => {
                this._sourceUriValidatorsReady(true);
            },
            onDisposed: () => {
                this._sourceUriValidatorsReady(false);
            },
            validationRules: [
                {
                    type: 'custom',
                    assignValueFirst: true,
                    isDeferred: external_ko_namespaceObject.pureComputed(() => this._noEmptyProperties()),
                    get message() {
                        return _self._lastValidationMessage();
                    },
                    validationCallback: this._sourceUriValidationCallback
                }
            ]
        };
        return sourceUri;
    }
    _getBasicHttpAuthInfo() {
        const basicHttpAuthName = {
            propertyName: 'userName', displayName: 'Username:', localizationId: 'DataAccessUIStringId.WizardPageConfigureJsonConnection_UsernameText', editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text'),
            validatorOptions: undefined,
        };
        const basicHttpAuth = {
            propertyName: 'basicHttpAuth', displayName: 'Basic HTTP Authentication', localizationId: 'DataAccessUIStringId.WizardPageConfigureJsonConnection_BasicHttpAuthText', info: [
                basicHttpAuthName,
                (0,external_DevExpress_Analytics_Internal_namespaceObject.createPasswordSerializationInfo)({ propertyName: 'password', displayName: 'Password:', localizationId: 'DataAccessUIStringId.WizardPageConfigureJsonConnection_PasswordText' }, false),
            ], editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('objecteditor')
        };
        const onValidatorInitialied = (e) => {
            const authNmeValidatorInstance = e && e.component;
            if (this._authNameValidatorInstance && this._authNameValidatorInstance != authNmeValidatorInstance) {
                this._authNameValidatorInstance.dispose();
            }
            this._authNameValidatorInstance = authNmeValidatorInstance;
            this._basicAuthValidatorsReady(true);
        };
        const authNameValidatorDisposed = () => {
            this._authNameValidatorInstance = null;
        };
        basicHttpAuthName.validatorOptions = {
            onInitialized: onValidatorInitialied,
            onDisposed: authNameValidatorDisposed,
            validationRules: [{
                    type: 'custom',
                    reevaluate: true,
                    assignValueFirst: true,
                    get message() {
                        return getLocalizedValidationErrorMessage(null, (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(basicHttpAuth.displayName, basicHttpAuth.localizationId), (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(basicHttpAuthName.displayName, basicHttpAuthName.localizationId));
                    },
                    validationCallback: (params) => {
                        return this._isBasicHttpAuthValid();
                    }
                }]
        };
        return basicHttpAuth;
    }
    _getParametersInfo() {
        const parameters = {
            propertyName: 'parameters', displayName: 'Parameters', localizationId: 'AnalyticsCoreStringId.QueryBuilder_Parameters',
            array: true,
            addHandler: () => {
                const newName = (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueNameForNamedObjectsArray)(this.parameters(), 'parameter');
                return this._getPatchedParameter(JsonParameter.from({ '@Name': newName }));
            },
            editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('commonCollection'),
            editorOptions: null,
            template: '#dx-jsonwizard-parametercollection'
        };
        return parameters;
    }
    _getResultUriInfo() {
        const _resultUri = {
            propertyName: 'resultUri',
            displayName: 'Resulting URI:',
            localizationId: 'DataAccessUIStringId.WizardPageChooseJsonSource_ResultingUri',
            defaultVal: '',
            editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('text'),
            editorOptions: {
                readOnly: true,
                elementAttr: {
                    title: this.resultUri
                }
            }
        };
        return _resultUri;
    }
    _getResultUri() {
        if (!this.sourceUri() || !this._isUriValid() || !this._isParametersValid())
            return null;
        if (this.sourceUri() && this.parameters().length === 0)
            return this.sourceUri();
        const uriParts = this.sourceUri().split('?');
        let url = uriParts[0];
        const query = uriParts[1];
        url = this._appendPathSegmentsToUri(uriParts[0]);
        return this._appendQuerySegmentsToUri(url, query);
    }
    _appendPathSegmentsToUri(uri) {
        const segments = this.parameters()
            .filter(p => p.itemType() === 'PathParameter')
            .map(p => '{' + p.name() + '}');
        segments.forEach(segment => {
            uri = uri.replace(/[\/]+$/g, '') + '/' + segment.replace(/^[\/]+/g, '');
        });
        return uri;
    }
    _appendQuerySegmentsToUri(uri, originalQuery) {
        const separator = '';
        let uriArguments = originalQuery || '';
        const querySegments = this.parameters()
            .filter(p => p.itemType() === 'QueryParameter')
            .map(p => p.name() + '={?}');
        querySegments.forEach(q => {
            if (!!uriArguments)
                uriArguments += '&';
            uriArguments += q;
        });
        if (uriArguments.length > 0)
            uri += '?' + uriArguments;
        return uri;
    }
    constructor(_requestWrapper, _itemsProvider) {
        super();
        this._requestWrapper = _requestWrapper;
        this._itemsProvider = _itemsProvider;
        this._isUriValid = external_ko_namespaceObject.observable(false);
        this._lastValidatedJsonSourceJSON = '';
        this._authNameValidatorInstance = null;
        this._isInitUri = true;
        this._collectionItemNamePlaceholder = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Name', 'AnalyticsCoreStringId.CollectionEditor_Name_Placeholder');
        this._lastValidateDeferred = null;
        this._sourceUriValidatorsReady = external_ko_namespaceObject.observable(true);
        this._basicAuthValidatorsReady = external_ko_namespaceObject.observable(false);
        this._validationRequested = external_ko_namespaceObject.observable(false).extend({ deferred: true });
        this._noEmptyProperties = external_ko_namespaceObject.pureComputed(() => {
            const isBasicHttpAuthValid = this._isBasicHttpAuthValid();
            const isParametersValid = this._isParametersValid();
            const sourceUriNotEmpty = !!this.sourceUri();
            return isBasicHttpAuthValid && isParametersValid && sourceUriNotEmpty;
        });
        this._lastValidationMessage = external_ko_namespaceObject.observable();
        this._getSerializedUriSource = (jsonDataSource = new JsonDataSource({})) => {
            this.applySettings(jsonDataSource);
            return JSON.stringify(jsonDataSource.source.serialize(true));
        };
        this._sourceUriValidationCallback = (params) => {
            if (!this.sourceUri()) {
                if (this._isInitUri) {
                    this._isInitUri = false;
                    return true;
                }
                params.rule.message = getLocalizedValidationErrorMessage(null, (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Web Service Endpoint (URI):', 'DataAccessUIStringId.WizardPageChooseJsonSource_URI'));
                return false;
            }
            let serverValidationResult = false;
            this._isInitUri = false;
            const serializedJsonSource = this._getSerializedUriSource();
            if (serializedJsonSource != this._lastValidatedJsonSourceJSON) {
                if (!this._lastValidationMessage())
                    this._lastValidationMessage((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Validation...', 'AnalyticsCoreStringId.Validation'));
                this._validateUriSource().done((result) => {
                    serverValidationResult = params.rule.isValid = result.isUriValid;
                    result.faultMessage && (params.rule.message = result.faultMessage);
                    this._lastValidationMessage(serverValidationResult ? null : result.faultMessage);
                    params.validator.validate();
                    setTimeout(() => this._repaintSummary(), 1);
                });
            }
            else {
                return !this._lastValidationMessage();
            }
            return serverValidationResult;
        };
        this.isValid = external_ko_namespaceObject.pureComputed(() => {
            const isPropertiesNotEmpty = this._noEmptyProperties();
            const isUriValid = this._isUriValid();
            return isPropertiesNotEmpty && isUriValid;
        });
        this._validatorsReady = external_ko_namespaceObject.pureComputed({
            read: () => {
                const sourceUriValidatorReady = this._sourceUriValidatorsReady();
                const authIsEmpty = !this.basicHttpAuth.password() && !this.basicHttpAuth.userName();
                const authInitialized = this._basicAuthValidatorsReady();
                return sourceUriValidatorReady && (authInitialized || authIsEmpty);
            },
            write: (newVal) => {
                this._basicAuthValidatorsReady(newVal);
            }
        });
        this.sourceUri = external_ko_namespaceObject.observable('');
        this.basicHttpAuth = {
            password: external_ko_namespaceObject.observable(''),
            userName: external_ko_namespaceObject.observable(''),
        };
        this.parameters = external_ko_namespaceObject.observableArray([]);
        this._disposables.push(this.resultUri = external_ko_namespaceObject.pureComputed(() => {
            return this._getResultUri();
        }));
        this._disposables.push(this.grid = new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(external_ko_namespaceObject.observable(this)));
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            const editorsInitialized = this._validatorsReady();
            const validationRequested = this._validationRequested();
            if (editorsInitialized && validationRequested) {
                this._validate();
            }
        }));
        this._disposables.push(this._validatorsReady);
    }
    _onValidationSummaryInitialized(args) {
        super._onValidationSummaryInitialized(args);
        this._isInitUri = true;
        setTimeout(() => {
            this._validate();
        }, 1);
    }
    _applyParametersToSource(jsonDataSource) {
        const parametersByType = this.parameters().reduce((result, parameter) => ({
            ...result,
            [parameter.itemType()]: [...(result[parameter.itemType()] || []), parameter],
        }), {});
        ['PathParameter', 'QueryParameter', 'Header'].forEach(type => {
            if (parametersByType[type])
                jsonDataSource.source[parameterTypeToPropertyMap[type]](parametersByType[type]);
        });
    }
    applySettings(jsonDataSource) {
        jsonDataSource.source.uri(this.sourceUri());
        jsonDataSource.source.json(undefined);
        jsonDataSource.source.authenticationInfo.password(this.basicHttpAuth.password());
        jsonDataSource.source.authenticationInfo.userName(this.basicHttpAuth.userName());
        this._applyParametersToSource(jsonDataSource);
    }
    getInfo() {
        const sourceUri = this._getSourceUriInfo();
        const basicHttpAuth = this._getBasicHttpAuthInfo();
        const parameters = this._getParametersInfo();
        const resultUriInfo = this._getResultUriInfo();
        return [sourceUri, basicHttpAuth, parameters, resultUriInfo];
    }
    reset() {
        this.sourceUri('');
        this.basicHttpAuth.password('');
        this.basicHttpAuth.userName('');
        this.parameters([]);
    }
    setValue(dataSource) {
        this.sourceUri(dataSource.source.uri());
        this.basicHttpAuth.userName(dataSource.source.authenticationInfo.userName());
        this.basicHttpAuth.password(dataSource.source.authenticationInfo.password());
        const sourceParameters = [].concat.apply([], [dataSource.source.pathParameters(), dataSource.source.queryParameters(), dataSource.source.headers()]);
        this.parameters(sourceParameters.map(x => this._getPatchedParameter(x)));
    }
    dispose() {
        this._authNameValidatorInstance && this._authNameValidatorInstance.dispose();
        if (this._lastValidateDeferred) {
            this._lastValidateDeferred.reject();
            this._lastValidateDeferred = null;
        }
        super.dispose();
        this.disposeObservableArray(this.parameters);
    }
    onChange(_onChange) {
        let timeoutId = null;
        const localOnChange = () => {
            _onChange();
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => this._validate(), 1);
        };
        this._disposables.push(...subscribeProperties([this.sourceUri, this.basicHttpAuth.password, this.basicHttpAuth.userName, this.resultUri], localOnChange));
        this._disposables.push(subscribeArray(this.parameters, (item) => {
            item._disposables.push(...subscribeProperties([item.name, item.itemType, item.value], localOnChange));
        }, localOnChange));
    }
    isEmpty() {
        return !this.sourceUri();
    }
    _validate() {
        if (this._validationSummary && this._validationGroup) {
            if (this._validatorsReady()) {
                this._validationGroup.validate();
                this._validationRequested(false);
            }
            else {
                this._validationRequested(true);
            }
        }
    }
}

;// ./dist/query-builder/wizard/pages/jsonDataSourceWizard/chooseJsonSourcePage.js











class ChooseJsonSourcePage extends WizardPageBase {
    _onValidationGroupInitialized(e) {
        this.__validationGroup = e.component;
    }
    _onValidationGroupDisposing(e) {
        this.__validationGroup = null;
    }
    _onValidationSummaryInitialized(e) {
        this.__validationSummary = e.component;
        this.__validationGroup && this.__validationSummary && this._connectionName && this._connectionName() && setTimeout(() => {
            this.__validationGroup && this.__validationGroup.validate();
        }, 1);
    }
    _onValidationSummaryDisposing(e) {
        this.__validationSummary = null;
    }
    constructor(_requestWrapper = new RequestWrapper(), itemsProvider) {
        super();
        this._requestWrapper = _requestWrapper;
        this.__validationGroup = null;
        this.__validationSummary = null;
        this._jsonSourceTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('JSON Source:', 'DataAccessUIStringId.WizardPageChooseJsonSource_SourceType');
        this._jsonConnectionTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Connection Name:', 'AnalyticsCoreStringId.ReportDesigner_Wizard_Json_ConnectionName');
        this._connectionNameValidationRules = [{
                type: 'required',
                get message() {
                    return getLocalizedValidationErrorMessage(null, this._jsonConnectionTitle);
                }
            }];
        this._connectionName = external_ko_namespaceObject.observable('');
        this._validationGroup = {
            onInitialized: (args) => this._onValidationGroupInitialized(args),
            onDisposing: (args) => this._onValidationGroupDisposing(args)
        };
        this._validationSummary = {
            onInitialized: (args) => this._onValidationSummaryInitialized(args),
            onDisposing: (args) => this._onValidationSummaryDisposing(args)
        };
        this._sources = [];
        this._disposables.push(this._jsonStringSettings = new JsonDataSourceJsonSourcePageStringSettings());
        this._jsonStringSettings.onChange(() => this._onChange());
        this._disposables.push(this._jsonUriSetting = new JsonDataSourceJsonSourcePageUriSettings(this._requestWrapper, itemsProvider));
        this._jsonUriSetting.onChange(() => this._onChange());
        this._sources = [
            { value: this._jsonUriSetting, displayValue: 'Web Service Endpoint (URI)', localizationId: 'DataAccessUIStringId.WizardPageChooseJsonSource_SourceType_Uri' },
            { value: this._jsonStringSettings, displayValue: 'JSON String', localizationId: 'DataAccessUIStringId.WizardPageChooseJsonSource_SourceType_Custom' }
        ];
        const selectedSource = external_ko_namespaceObject.observable();
        this._selectedSource = external_ko_namespaceObject.pureComputed({
            read: () => selectedSource(),
            write: (newVal) => {
                if (selectedSource() === newVal)
                    return;
                selectedSource(newVal);
                newVal._validatorsReady && newVal._validatorsReady(false);
                setTimeout(() => {
                    newVal._validate && newVal._validate();
                }, 1);
                this._onChange();
            }
        });
        this._selectedSource(this._sources[0].value);
        this._disposables.push(this._selectedSource);
        this._disposables.push(this._connectionName.subscribe(() => this._onChange()));
    }
    canNext() {
        const connectionNameNotEmpty = !!this._connectionName();
        const isCurrentSourceValid = this._selectedSource().isValid();
        const isCurrentSourceEmpty = this._selectedSource().isEmpty();
        return connectionNameNotEmpty && isCurrentSourceValid && !isCurrentSourceEmpty;
    }
    commit() {
        const jsonDataSource = new JsonDataSource({});
        this._selectedSource().applySettings(jsonDataSource);
        const serialized = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(jsonDataSource);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            jsonSource: JSON.stringify(serialized.Source),
            newConnectionName: this._connectionName()
        }).promise();
    }
    initialize(state) {
        this.__validationGroup = null;
        this.__validationSummary = null;
        const jsonDataSource = _restoreJsonDataSourceFromState(state);
        if (jsonDataSource.source.uri()) {
            this._selectedSource(this._jsonUriSetting);
        }
        else if (jsonDataSource.source.json()) {
            this._selectedSource(this._jsonStringSettings);
        }
        this._selectedSource().setValue(jsonDataSource);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
}
function _registerChooseJsonSourcePage(factory, requestWrapper, getItemsProviderCallback) {
    factory.registerMetadata(JsonDataSourceWizardPageId.ChooseJsonSourcePage, {
        setState: (data, state) => {
            state.jsonSource = data.jsonSource;
            state.newConnectionName = data.newConnectionName;
        },
        getState: (state) => {
            return state.jsonDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.jsonSource = defaultState.jsonSource;
        },
        create: () => {
            return new ChooseJsonSourcePage(requestWrapper, getItemsProviderCallback && getItemsProviderCallback());
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Create a data connection.', 'AnalyticsCoreStringId.JsonDSWizard_CreateNewConnectionPage_Description'),
        template: 'dxrd-page-jsonsource'
    });
}

;// ./dist/query-builder/wizard/pages/jsonDataSourceWizard/specifyJsonConnectionPage.js






class SpecifyJsonConnectionPage extends ChooseJsonConnectionPage {
    constructor(connections, allowCreateNewJsonConnection, itemsProvider, _requestWrapper = new RequestWrapper(), _getJsonConnectionsCallback) {
        super(connections, allowCreateNewJsonConnection, _getJsonConnectionsCallback);
        this._requestWrapper = _requestWrapper;
        this._disposables.push(this._specifySourceData = new ChooseJsonSourcePage(_requestWrapper, itemsProvider));
        this._specifySourceData.onChange(() => this._onChange());
    }
    commit() {
        const deffered = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        let _promise;
        if (this._createNew()) {
            _promise = this._specifySourceData.commit();
        }
        else {
            _promise = super.commit();
        }
        _promise.done(state => {
            deffered.resolve(state);
        });
        return deffered.promise();
    }
    canNext() {
        if (this._createNew()) {
            return this._specifySourceData.canNext();
        }
        else {
            return super.canNext();
        }
    }
    initialize(state) {
        const deffered = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        super.initialize(state).done(() => {
            this._specifySourceData.initialize(state).done(() => {
                deffered.resolve(this);
            });
        });
        return deffered.promise();
    }
}
function _registerSpecifyJsonConnectionPage(factory, connections, allowCreateNewJsonConnection, getItemsProviderCallBack, getJsonConnectionsCallback) {
    factory.registerMetadata(JsonDataSourceWizardPageId.SpecifyJsonConnectionPage, {
        create: () => new SpecifyJsonConnectionPage(connections, allowCreateNewJsonConnection, getItemsProviderCallBack && getItemsProviderCallBack(), undefined, getJsonConnectionsCallback),
        description: allowCreateNewJsonConnection ?
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Do you want to use an existing data connection?', 'AnalyticsCoreStringId.JsonDSWizard_ChooseConnection_Description') :
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose a data connection.', 'AnalyticsCoreStringId.SqlDSWizard_PageChooseConnection'),
        getState: (state) => state.jsonDataSourceWizard,
        setState: (data, state) => {
            state.connectionName = data.connectionName;
            state.jsonSource = data.jsonSource;
            state.newConnectionName = data.newConnectionName;
        },
        resetState: (state, defaultState) => {
            state.connectionName = defaultState.connectionName;
            state.jsonSource = defaultState.jsonSource;
            state.newConnectionName = defaultState.newConnectionName;
        },
        template: 'dxrd-page-specify-connection'
    });
}

;// ./dist/query-builder/wizard/internal/_treeListNode.js



const defaultObjectDataSourceItemSpecifics = 'Default';
class TreeNodeBase extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(name, displayName, specifics, isChecked = false, afterCheckToggled, pathParts) {
        super();
        this.name = name;
        this.displayName = displayName;
        this.specifics = specifics;
        this.checked = external_ko_namespaceObject.pureComputed(() => this._checked());
        this.isList = false;
        this.pathParts = pathParts;
        this.path = pathParts?.join('.');
        this._checked = external_ko_namespaceObject.observable(isChecked);
        this._afterCheckToggled = afterCheckToggled || (() => void (0));
    }
    unChecked() {
        return this.checked() === false;
    }
    toggleChecked() {
        this.setChecked(!this.checked.peek());
        this._afterCheckToggled(this);
    }
    setChecked(value) {
        this._checked(value);
    }
}
class TreeLeafNode extends TreeNodeBase {
    constructor(name, displayName, specifics, isChecked = false, nodeArguments = null, afterCheckToggled, pathParts) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, pathParts);
        this.name = name;
        this.displayName = displayName;
        this.specifics = specifics;
        this.hasQuery = false;
        this.arguments = nodeArguments;
    }
}
class TreeNode extends TreeNodeBase {
    constructor(name, displayName, specifics, isChecked, afterCheckToggled, pathParts) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, pathParts);
        this.countChecked = external_ko_namespaceObject.pureComputed(() => {
            let count = 0;
            for (let i = 0; i < this.children().length; i++) {
                if (!this.children()[i].unChecked()) {
                    if (count > 1)
                        break;
                    count++;
                }
            }
            return count;
        });
        this.isList = true;
        this.children = external_ko_namespaceObject.observableArray([]);
        this.checked = external_ko_namespaceObject.pureComputed({
            read: () => {
                if (!this.initialized()) {
                    return this._checked();
                }
                else {
                    let selectedItems = 0;
                    let partiallySelectedItems = 0;
                    this.children().forEach(item => {
                        if (item.checked() === true) {
                            selectedItems++;
                        }
                        else if (item.checked() !== false) {
                            partiallySelectedItems++;
                        }
                    });
                    if (selectedItems === 0 && partiallySelectedItems === 0) {
                        return false;
                    }
                    if (selectedItems === this.children.peek().length) {
                        return true;
                    }
                    return undefined;
                }
            }
        });
    }
    initialized() {
        return this.children().length > 0;
    }
    setChecked(value) {
        this._checked(value);
        this.children.peek().forEach(item => {
            item.setChecked(value);
        });
    }
    initializeChildren(children) {
        this.children(children || []);
    }
}
class ParameterTreeNode extends TreeNode {
    constructor(name, displayName, specifics, isChecked, afterCheckToggled, pathParts) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, pathParts);
        this.countChecked = external_ko_namespaceObject.pureComputed(() => {
            let count = 0;
            this.hasParamsToEdit(false);
            for (let i = 0; i < this.children().length; i++) {
                const child = this.children()[i];
                if (!child.unChecked()) {
                    if (count > 1)
                        break;
                    count++;
                    if (child.arguments && child.arguments.length > 0)
                        this.hasParamsToEdit(true);
                    if (child.specifics === 'query')
                        this.hasParamsToEdit(true);
                }
            }
            return count;
        });
        this.hasParamsToEdit = external_ko_namespaceObject.observable(false);
    }
}
class QueriesTreeNode extends ParameterTreeNode {
    constructor(name, displayName, specifics, isChecked, callbacks, afterCheckToggled) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, [name]);
        this.addAction = {
            clickAction: (item) => {
                if (this.disableCustomSql()) {
                    return this.addQuery();
                }
                else {
                    return this.showPopover();
                }
            },
            imageClassName: 'dxrd-image-add',
            imageTemplateName: 'dxrd-svg-operations-add',
            templateName: 'dx-treelist-action-with-popover',
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Add query', 'AnalyticsCoreStringId.SqlDSWizard_AddQuery')
        };
        this.itemClickAction = (e) => {
            this.popoverVisible(false);
            e.itemData.addAction();
        };
        this.className = 'dx-addqueries-popover';
        this.popoverVisible = external_ko_namespaceObject.observable(false);
        this.addQuery = () => {
            callbacks().showQbCallBack();
        };
        this.addCustomQuery = () => {
            callbacks().showQbCallBack(null, true);
        };
        this.disableCustomSql = () => callbacks && callbacks().disableCustomSql;
        this.target = '.' + this.addAction.templateName;
        this.selectionDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this.children().length;
        });
    }
    getActions(context) {
        const result = [];
        if (context.path.indexOf('queries') === 0) {
            result.push(this.addAction);
        }
        return result;
    }
    popoverListItems() {
        return [
            {
                name: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Run Query Builder', 'DataAccessUIStringId.Button_QueryBuilder'),
                addAction: () => this.addQuery()
            },
            {
                name: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Write Custom SQL', 'AnalyticsCoreStringId.SqlDSWizard_WriteCustomSQL'),
                addAction: () => this.addCustomQuery()
            }
        ];
    }
    showPopover() {
        this.popoverVisible(true);
    }
}
class TreeQueryNode extends TreeLeafNode {
    constructor(name, displayName, specifics, isChecked, parameters, callbacks, afterCheckToggled, query, pathParts) {
        super(name, displayName, specifics, isChecked, null, afterCheckToggled, pathParts);
        this.query = query;
        this.editAction = {
            clickAction: (item) => {
                return this.editQuery();
            },
            imageClassName: 'dx-image-edit',
            imageTemplateName: 'dxrd-svg-operations-edit',
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Edit query', 'AnalyticsCoreStringId.SqlDSWizard_EditQuery')
        };
        this.removeAction = {
            clickAction: (item) => {
                this.removeQuery({ model: item.data });
            },
            imageClassName: 'dxrd-image-recycle-bin',
            imageTemplateName: 'dxrd-svg-operations-recycle_bin',
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Remove query', 'AnalyticsCoreStringId.SqlDSWizard_RemoveQuery')
        };
        this.parameters = parameters;
        this.removeQuery = (e) => {
            if (!e.model.unChecked()) {
                e.model.toggleChecked();
            }
            callbacks().deleteAction(e.model.name);
        };
        this.editQuery = (e) => {
            callbacks().showQbCallBack(this.name);
        };
        this.hasQuery = true;
    }
    setObservableName(getter, setter) {
        ['name', 'displayName'].forEach((propertyName) => Object.defineProperty(this, propertyName, {
            get() {
                return getter();
            },
            set(newVal) {
                setter(newVal);
            },
            configurable: true
        }));
    }
    getActions(context) {
        const result = [];
        result.push(this.removeAction);
        result.push(this.editAction);
        return result;
    }
}
class FieldTreeNode extends TreeNodeBase {
    constructor(name, displayName, specifics, isChecked, pathParts, afterCheckToggled, isDraggable = false) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, pathParts);
        this.visible = external_ko_namespaceObject.observable(true);
        this.disabled = external_ko_namespaceObject.observable(false);
        this.isComplex = specifics === defaultObjectDataSourceItemSpecifics;
        if (isDraggable)
            this.dragData = { noDragable: false };
    }
}
class DataMemberTreeNode extends TreeNode {
    constructor(name, displayName, specifics, isChecked, pathParts, afterCheckToggled) {
        super(name, displayName, specifics, isChecked, afterCheckToggled, pathParts);
        this.visible = external_ko_namespaceObject.observable(true);
        this.isComplex = this.isList && specifics === defaultObjectDataSourceItemSpecifics;
        this.checked = external_ko_namespaceObject.pureComputed({
            read: () => {
                if (!this.initialized()) {
                    return this._checked();
                }
                else {
                    let selectedItems = 0;
                    let partiallySelectedItems = 0;
                    const visibleChildren = this.children().filter(item => item.visible());
                    visibleChildren.forEach(item => {
                        if (item.checked() === true) {
                            selectedItems++;
                        }
                        else if (item.checked() !== false) {
                            partiallySelectedItems++;
                        }
                    });
                    if (selectedItems === 0 && partiallySelectedItems === 0) {
                        return false;
                    }
                    if (selectedItems === visibleChildren.length) {
                        return true;
                    }
                    return undefined;
                }
            }
        });
    }
    setChecked(value) {
        super.setChecked(this.visible() ? value : false);
    }
}
class SingleCheckedDataMemberTreeNode extends DataMemberTreeNode {
    constructor(name, displayName, specifics, isChecked, pathParts, afterCheckToggled) {
        super(name, displayName, specifics, isChecked, pathParts, afterCheckToggled);
        this.checked = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this._checked();
            }
        });
    }
}

;// ./dist/query-builder/wizard/internal/_treeNodeItemsProvider.js






class TreeNodeItemsProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _createTree() {
        if (!this._fullTreeLoaded)
            return external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when(this._rootItems().map(item => this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(item.path), true)))
                .always(() => this._fullTreeLoaded = true);
        else {
            return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve().promise();
        }
    }
    _createTreePart(pathParts, deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred(), checkedPath) {
        if (this._fullTreeLoaded)
            return deferred.resolve().promise();
        if (pathParts.length === 0)
            return deferred.resolve();
        if (!checkedPath) {
            const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (pathParts.length === 1) {
                this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(pathParts[0], pathParts)).done(() => deferred.resolve()).fail(() => deferred.reject());
            }
            else
                this._createTreePart(pathParts.slice(1), deferred, [pathParts[0]]);
            return deferred;
        }
        else {
            const newParentPath = [].concat([], checkedPath, pathParts[0]);
            const request = new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(newParentPath.join('.'), newParentPath);
            if (!this._getParentNode(request)) {
                this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(checkedPath.join('.'), checkedPath)).done((res) => {
                    this._createTreePart(pathParts.slice(1), deferred, newParentPath);
                }).fail(() => deferred.reject());
            }
            else {
                this._createTreePart(pathParts.slice(1), deferred, newParentPath);
            }
        }
    }
    _setChecked(item) {
        item.setChecked(true);
        if (item instanceof DataMemberTreeNode) {
            item.children().forEach(x => this._setChecked(x));
        }
    }
    selectAllItems(onlyRoot = true) {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        this._createTree().always(() => {
            if (onlyRoot) {
                this._rootItems().forEach(x => x.setChecked(true));
            }
            else {
                this._rootItems().forEach(x => this._setChecked(x));
            }
            deferred.resolve();
        });
        return deferred.promise();
    }
    selectItemsByPath(path) {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        const pathParts = path.split('.');
        this._createTreePart(pathParts).done(() => {
            this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(pathParts.join('.'), pathParts)).done((items) => {
                items.forEach(item => {
                    if (item instanceof TreeNodeBase) {
                        item.setChecked(true);
                    }
                });
            }).always(() => deferred.resolve());
        });
        return deferred.promise();
    }
    selectItemByPath(path) {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        const pathParts = path.split('.');
        this._createTreePart(pathParts).done(() => {
            const fieldName = pathParts.pop();
            this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(pathParts.join('.'), pathParts)).done((items) => {
                const item = items.filter(x => x.name === fieldName)[0];
                if (item instanceof TreeNodeBase) {
                    item.setChecked(true);
                }
            }).always(() => deferred.resolve());
        });
        return deferred.promise();
    }
    _getParentNode(pathRequest) {
        let parentNode = this._rootItems().filter(item => item.path === (pathRequest.id || pathRequest.ref))[0];
        if (!parentNode)
            return;
        let childPath = parentNode.path;
        for (let index = 1; index < pathRequest.pathParts.length; index++) {
            if (!parentNode)
                return;
            childPath += '.' + pathRequest.pathParts[index];
            parentNode = parentNode.children().filter(item => (0,external_DevExpress_Analytics_Internal_namespaceObject.isList)(item) && item.path == childPath)[0];
        }
        return parentNode;
    }
    _getDefaultTreeNodeCheckState(item) {
        return false;
    }
    constructor(fieldListProvider, rootItems, generateTreeNode, generateTreeLeafNode) {
        super();
        this._fullTreeLoaded = false;
        this._rootItems = external_ko_namespaceObject.observableArray([]);
        this._checkedRootNodesCount = external_ko_namespaceObject.computed(() => {
            if (!this._rootItems || this._rootItems().length === 0)
                return 0;
            let count = 0;
            for (let i = 0; i < this._rootItems().length && count < 1; i++) {
                count += this._rootItems()[i].unChecked() ? 0 : 1;
            }
            return count;
        });
        this.hasCheckedItems = external_ko_namespaceObject.computed(() => {
            return !(this._checkedRootNodesCount() === 0);
        });
        this.getRootItems = () => this._rootItems();
        this.getItems = (pathRequest, collectChilds = false) => {
            const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (!pathRequest.fullPath && pathRequest.pathParts.length === 0) {
                result.resolve(this._rootItems());
            }
            else {
                fieldListProvider.getItems(pathRequest).done((value) => {
                    const currentParentNode = this._getParentNode(pathRequest);
                    if (!currentParentNode || !currentParentNode.children)
                        return result.reject();
                    if (currentParentNode.children().length === 0) {
                        const array = [];
                        const listPath = [];
                        value.forEach(item => {
                            const isChecked = this._getDefaultTreeNodeCheckState(item);
                            const pathParts = pathRequest.fullPath.split('.');
                            if (this.isList(item, currentParentNode)) {
                                if (pathRequest.pathParts.length <= 5) {
                                    listPath.push([].concat(pathParts, [item.name]));
                                    array.push(generateTreeNode(item, isChecked, [...pathParts, item.name]));
                                }
                            }
                            else {
                                array.push(generateTreeLeafNode(item, isChecked, [...pathParts, item.name]));
                            }
                        });
                        currentParentNode.initializeChildren(array);
                        if (collectChilds)
                            external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when(listPath.map(x => this.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(x.join('.'), x), collectChilds))).always(() => result.resolve(array));
                        else
                            result.resolve(array);
                    }
                    else {
                        result.resolve(currentParentNode.children());
                    }
                });
            }
            return result.promise();
        };
        this._disposables.push(rootItems.subscribe((newValue) => {
            this._fullTreeLoaded = false;
            this._rootItems(newValue.map(item => {
                const isChecked = this._getDefaultTreeNodeCheckState(item);
                return generateTreeNode(item, isChecked, [item.name]);
            }));
        }));
        this._disposables.push(this.hasCheckedItems);
    }
    isList(dataMember, parentNode) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.isList)(dataMember);
    }
}

;// ./dist/query-builder/wizard/internal/_jsonTreeNodeItemsProvider.js

class JsonTreeNodeItemsProvider extends TreeNodeItemsProvider {
    constructor(fieldListProvider, rootItems, generateTreeNode, generateTreeLeafNode) {
        super(fieldListProvider, rootItems, generateTreeNode, generateTreeLeafNode);
    }
    _getDefaultTreeNodeCheckState(item) {
        return item.isSelected;
    }
    getNodeByPath(pathRequest) {
        let listNode = this._rootItems().filter(item => item.path === (pathRequest.pathParts || [])[0])[0];
        if (!listNode)
            return;
        let childPath = listNode.path;
        for (let index = 1; index < pathRequest.pathParts.length; index++) {
            if (!listNode)
                return;
            childPath += '.' + pathRequest.pathParts[index];
            listNode = listNode.children().filter(item => item.path == childPath)[0];
        }
        return listNode;
    }
}

;// ./dist/query-builder/utils/_jsonSchemaFieldListProviderFactory.js





class JsonSchemaFieldListProviderFactory {
    static _getNodesByPath(pathRequest, parentNode) {
        if (!pathRequest.fullPath) {
            return parentNode.nodes;
        }
        else {
            let currentNodes = parentNode.nodes;
            const pathParts = (0,external_DevExpress_Analytics_Utils_namespaceObject._getPathPartsFromRequest)(pathRequest);
            for (let i = 0; i < pathParts.length; i++) {
                const pathPart = (currentNodes || []).filter((node) => node.name() == pathParts[i])[0];
                if (!pathPart)
                    return [];
                currentNodes = pathPart.nodes;
            }
            return currentNodes;
        }
    }
    static _getInnerItemsByPath(pathRequest, parentNode) {
        const nodes = this._getNodesByPath(pathRequest, parentNode);
        return (nodes || []).map((node) => this._nodetoDataMemberInfo(node));
    }
    static _createFieldListCallback(createRootNode) {
        return (pathRequest) => {
            const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
            createRootNode().done((rootNode) => {
                const itemsByPath = this._getInnerItemsByPath(pathRequest, rootNode);
                deferred.resolve(itemsByPath);
            }).fail(reason => deferred.reject(reason));
            return deferred.promise();
        };
    }
    static _nodetoDataMemberInfo(node) {
        const type = external_DevExpress_Analytics_Internal_namespaceObject.koUtils.unwrap(node.type) || '';
        return {
            name: external_DevExpress_Analytics_Internal_namespaceObject.koUtils.unwrap(node.name),
            displayName: external_DevExpress_Analytics_Internal_namespaceObject.koUtils.unwrap(node.displayName) || external_DevExpress_Analytics_Internal_namespaceObject.koUtils.unwrap(node.name),
            isSelected: external_DevExpress_Analytics_Internal_namespaceObject.koUtils.unwrap(node.selected),
            isList: node.nodes && node.nodes.length > 0,
            specifics: type ? DBColumn.GetSpecific(type) : 'Default',
            data: node,
            dataType: type
        };
    }
    static create(createRootNode, rootItems) {
        const callback = this._createFieldListCallback(createRootNode);
        return new external_DevExpress_Analytics_Internal_namespaceObject.FieldListProvider(callback, rootItems);
    }
}

;// ./dist/query-builder/wizard/pages/jsonDataSourceWizard/chooseJsonSchemaPage.js












class ChooseJsonSchemaPageBase extends WizardPageBase {
    _clear() {
        this._rootItems([]);
        this._fieldSelectedPath('');
    }
    _createRootNode() {
        const parentNode = new JsonNode({});
        parentNode.nodes = [this._rootItems()[0].data];
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve(parentNode).promise();
    }
    _getSchemaToDataMemberInfo(root, path) {
        let nodeAcc = root;
        for (let i = 0; i < path.pathParts.length; i++) {
            nodeAcc = nodeAcc.nodes.filter(node => node.name() === path.pathParts[i])[0];
            if (!nodeAcc)
                return null;
        }
        return nodeAcc;
    }
    _updateFieldListProvider() {
        this._fieldListItemsProvider()?.dispose();
        const fieldListProvider = JsonSchemaFieldListProviderFactory.create(() => this._createRootNode(), this._rootItems);
        this._fieldListItemsProvider(new JsonTreeNodeItemsProvider(fieldListProvider, this._rootItems, this._createTreeNode, this._createLeafTreeNode));
    }
    _resetSelectionRecursive(currentNode, selectedRootElement) {
        if (currentNode === selectedRootElement) {
            return;
        }
        currentNode.selected && currentNode.selected(false);
        (currentNode.nodes || []).forEach(node => this._resetSelectionRecursive(node, selectedRootElement));
    }
    _mapJsonSchema(jsonNode, path) {
        const treelistNode = this._fieldListItemsProvider().getNodeByPath(path);
        if (!treelistNode)
            return;
        jsonNode.selected(treelistNode.checked() !== false);
        (jsonNode.nodes || []).forEach(innerJsonNode => {
            const nextPathParts = path.pathParts.concat(innerJsonNode.name());
            const nextFullPath = nextPathParts.join('.');
            const nextPath = { fullPath: nextFullPath, path: innerJsonNode.name(), id: nextFullPath, pathParts: nextPathParts };
            this._mapJsonSchema(innerJsonNode, nextPath);
        });
        return jsonNode;
    }
    _filterRootElementList(rootElementList, jsonSchema) {
        return rootElementList;
    }
    canNext() {
        return false;
    }
    canFinish() {
        return this._fieldListItemsProvider().hasCheckedItems();
    }
    constructor(_requestWrapper = new RequestWrapper()) {
        super();
        this._requestWrapper = _requestWrapper;
        this._rootItems = external_ko_namespaceObject.observableArray([]);
        this._fieldListItemsProvider = external_ko_namespaceObject.observable(null);
        this._fieldSelectedPath = external_ko_namespaceObject.observable(null);
        this._createTreeNode = (item, isChecked, pathParts) => {
            const node = new DataMemberTreeNode(item.name, item.displayName, item.specifics, isChecked, pathParts);
            this._disposables.push(node.checked.subscribe(() => this._onChange()));
            return node;
        };
        this._createLeafTreeNode = (item, isChecked, pathParts) => {
            const node = new FieldTreeNode(item.name, item.displayName, item.specifics, isChecked, pathParts);
            this._disposables.push(node.checked.subscribe(() => this._onChange()));
            return node;
        };
        this._rootElementTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Root element:', 'DataAccessUIStringId.WizardPageChooseJsonSchema_RootElement');
        this._updateFieldListProvider();
        this._fieldListModel = {
            expandRootItems: true,
            itemsProvider: this._fieldListItemsProvider,
            selectedPath: this._fieldSelectedPath,
            treeListController: null,
            templateName: 'dxrd-treelist-with-checkbox'
        };
    }
    dispose() {
        super.dispose();
        this._fieldListItemsProvider()?.dispose();
    }
}
class ChooseJsonSchemaPage extends ChooseJsonSchemaPageBase {
    _updatePage(jsonSchema) {
        const rootElementList = jsonSchema.getRootElementPartList(this._allowObjectRootElements);
        if (this._rootElementList() !== rootElementList) {
            this._rootElementList(this._filterRootElementList(rootElementList, jsonSchema));
        }
        if (this._dataSource.rootElement()) {
            const dataSourceRootElementPath = ['root', this._dataSource.rootElement()].join('.');
            const rootElementToSelect = this._rootElementList().filter(item => item.fullPath === dataSourceRootElementPath)[0] || this._rootElementList()[0];
            this._selectedRootElement(rootElementToSelect);
        }
        this._onChange();
    }
    constructor(requestWrapper, _allowObjectRootElements = true, _callbacks) {
        super(requestWrapper);
        this._allowObjectRootElements = _allowObjectRootElements;
        this._callbacks = _callbacks;
        this._cachedState = {
            connectionName: null,
            jsonSource: null
        };
        this._rootElementList = external_ko_namespaceObject.observable([]);
        this._selectedRootElement = external_ko_namespaceObject.observable(null);
        let rootElementSubscription = null;
        this._disposables.push(this._rootElementList.subscribe((rootElements) => {
            rootElementSubscription && rootElementSubscription.dispose();
            rootElementSubscription = this._selectedRootElement.subscribe((selectedPath) => {
                if (!selectedPath)
                    return this._rootItems([]);
                const rootNode = this._getSchemaToDataMemberInfo(this._dataSource.schema, selectedPath);
                if (rootNode) {
                    this._rootItems([{
                            name: external_ko_namespaceObject.unwrap(rootNode.name),
                            isSelected: external_ko_namespaceObject.unwrap(rootNode.selected),
                            displayName: external_ko_namespaceObject.unwrap(rootNode.displayName) || external_ko_namespaceObject.unwrap(rootNode.name),
                            data: rootNode,
                            specifics: rootNode.nodes.length > 0 ? 'List' : 'Default'
                        }]);
                }
            });
            this._selectedRootElement(rootElements[0]);
        }));
    }
    initialize(state) {
        if ((state.connectionName && this._cachedState.connectionName === state.connectionName) ||
            (state.jsonSource && this._cachedState.jsonSource === state.jsonSource))
            return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
        this._clear();
        this._cachedState = {
            connectionName: state.connectionName,
            jsonSource: state.jsonSource
        };
        const oldDataSourceId = this._dataSource && this._dataSource.id;
        this._dataSource = _restoreJsonDataSourceFromState(state, this._requestWrapper);
        if ((oldDataSourceId && oldDataSourceId != this._dataSource.id) || !this._dataSource.schema.nodes.length) {
            return this._dataSource.getSchema(this._callbacks && this._callbacks.getParameters && this._callbacks.getParameters())
                .done((schema) => this._updatePage(schema));
        }
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().done((schema) => this._updatePage(schema)).resolve(this._dataSource.schema).promise();
    }
    _clear() {
        super._clear();
        this._rootElementList([]);
        this._selectedRootElement(null);
        this._dataSource && this._dataSource.jsonSchemaProvider.reset();
        this._cachedState = {
            connectionName: null,
            jsonSource: null
        };
    }
    commit() {
        const rootItem = this._rootItems()[0];
        if (!rootItem)
            return;
        const currentRootNode = this._rootItems()[0].data;
        this._resetSelectionRecursive(this._dataSource.schema, currentRootNode);
        const currentRootPath = currentRootNode.name();
        const pathFromCurrentRoot = { fullPath: currentRootPath, path: '', id: currentRootPath, pathParts: [currentRootPath] };
        this._mapJsonSchema(this._rootItems()[0].data, pathFromCurrentRoot);
        const selectedRootElementPath = this._selectedRootElement().pathParts.slice(1).join('.');
        this._dataSource.rootElement(selectedRootElementPath);
        const serialized = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(this._dataSource);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            connectionName: this._dataSource.connectionName(),
            dataSourceName: serialized['@Name'],
            jsonScheme: JSON.stringify(serialized.Schema),
            rootElement: serialized['@RootElement']
        }).promise();
    }
}
function _registerChooseJsonSchemaPage(factory, requestWrapper, callbacks) {
    factory.registerMetadata(JsonDataSourceWizardPageId.ChooseJsonSchemaPage, {
        setState: (data, state) => {
            state.dataSourceName = data.dataSourceName;
            state.jsonScheme = data.jsonScheme;
            state.rootElement = data.rootElement;
        },
        getState: (state) => {
            return state.jsonDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.dataSourceName = defaultState.dataSourceName;
            state.jsonScheme = defaultState.jsonScheme;
            state.rootElement = defaultState.rootElement;
        },
        create: () => {
            return new ChooseJsonSchemaPage(requestWrapper, undefined, callbacks);
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Select data fields.', 'DataAccessUIStringId.WizardPageChooseJsonSchema'),
        template: 'dxrd-jsondatasource-fields-page'
    });
}

;// external "DevExpress.ui.dxSelectBox"
const external_DevExpress_ui_dxSelectBox_namespaceObject = DevExpress.ui.dxSelectBox;
var external_DevExpress_ui_dxSelectBox_default = /*#__PURE__*/__webpack_require__.n(external_DevExpress_ui_dxSelectBox_namespaceObject);
;// ./dist/query-builder/wizard/pages/mongoDBDataSourceWizard/chooseMongoDBCollectionPage.js














class ChooseMongoDBCollectionPage extends WizardPageBase {
    _getDatabases() {
        return this._availableCollections.filter(database => database.collections?.length).map(db => db.databaseName);
    }
    _getCollections(dbName) {
        const database = this._availableCollections.find(database => database.databaseName === dbName);
        return database?.collections || [];
    }
    _revalidate() {
        const collections = this._collections();
        const isQueryValid = (query) => {
            return query.databaseName && !!query.collectionName;
        };
        this._isValid(collections.length > 0 && collections.every(collection => isQueryValid(collection)));
        this._addQueryDisabled(collections.length > 0 && !isQueryValid(collections[collections.length - 1]));
        this._onChange();
    }
    _aliasValidationCallback(alias, query) {
        for (const collection of this._collections()) {
            if (collection.id === query.id)
                continue;
            if ((collection.alias || collection.collectionName) === alias) {
                return false;
            }
            if (!alias && (collection.alias || collection.collectionName) === query.collectionName) {
                return false;
            }
        }
        return true;
    }
    constructor(_requestWrapper = new RequestWrapper(), rtl = false) {
        super();
        this._requestWrapper = _requestWrapper;
        this._filterEditorOptions = external_ko_namespaceObject.observable(null);
        this._filedListProvider = external_ko_namespaceObject.observable(null);
        this._filterEditorVisible = external_ko_namespaceObject.observable(false);
        this._availableCollections = [];
        this._collections = external_ko_namespaceObject.observableArray([]);
        this._isValid = external_ko_namespaceObject.observable(false);
        this._addQueryDisabled = external_ko_namespaceObject.observable(false);
        this.addDisposable(this._collections.subscribe(() => {
            this._revalidate();
        }), this._filterEditorVisible.subscribe((visible) => {
            if (!visible) {
                const querySate = this._collections().find(query => query.id === this._editedQuery.id());
                querySate.filterString = this._editedQuery.filterString();
                this._grid?.refresh();
            }
        }));
        const filterCellTemplate = (container, options) => {
            container = (0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(container);
            const query = MongoDBQuery.fromCollection(options.row.data);
            const valueObservable = query.filterString;
            const subscription = valueObservable.subscribe((newValue) => {
                options.setValue(newValue);
            });
            const filterOptions = new external_DevExpress_Analytics_Widgets_namespaceObject.FilterStringOptions(valueObservable, external_ko_namespaceObject.observable(options.row.data.collectionName));
            const fieldListProvider = JsonSchemaFieldListProviderFactory.create(() => {
                const query = MongoDBQuery.fromCollection(options.row.data);
                return this._dataSource.getQuerySchema(query);
            }, null);
            let childContext = this._bindingContext.createChildContext({
                value: valueObservable,
                disabled: !options.row.data.collectionName || !options.row.data.databaseName,
                buttonAction: () => {
                    this._editedQuery = query;
                    this._filedListProvider(fieldListProvider);
                    this._filterEditorOptions(filterOptions);
                    this._filterEditorVisible(true);
                }
            });
            let child = document.createElement('div');
            container.append(child);
            external_ko_namespaceObject.renderTemplate('dxrd-wizard-mongoDB-filterstring', childContext, {}, child, 'replaceNode');
            (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(container, function () {
                child = null;
                childContext = null;
                subscription.dispose();
            });
        };
        this._collectionsGrid = {
            dataSource: this._collections,
            rtlEnabled: rtl,
            editing: {
                mode: 'cell',
                allowUpdating: true,
                allowDeleting: true,
            },
            showRowLines: true,
            height: '100%',
            showBorders: true,
            onRowUpdated: (e) => {
                this._revalidate();
            },
            onInitialized: (e) => {
                this._bindingContext = external_ko_namespaceObject.contextFor((0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(e.element));
                this._grid = e.component;
            },
            columns: [
                {
                    dataField: 'databaseName',
                    caption: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Database', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_DatabaseNameColumnName'),
                    editCellTemplate: (cellElement, cellInfo) => {
                        cellElement = (0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(cellElement);
                        const selectBox = document.createElement('div');
                        cellElement.appendChild(selectBox);
                        new (external_DevExpress_ui_dxSelectBox_default())(selectBox, {
                            items: this._getDatabases(),
                            value: cellInfo.value,
                            placeholder: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Choose Database', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_ChooseDatabasePlaceholder'),
                            onValueChanged: (e) => {
                                cellInfo.setValue(e.value);
                                cellInfo.component.cellValue(cellInfo.rowIndex, 'collectionName', null);
                            }
                        });
                    }
                },
                {
                    dataField: 'collectionName',
                    caption: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Collection', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_CollectionNameColumnName'),
                    editCellTemplate: (cellElement, cellInfo) => {
                        cellElement = (0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(cellElement);
                        const dbName = cellInfo.row?.data?.databaseName;
                        const collections = this._getCollections(dbName);
                        const selectBox = document.createElement('div');
                        cellElement.appendChild(selectBox);
                        new (external_DevExpress_ui_dxSelectBox_default())(selectBox, {
                            items: collections,
                            value: cellInfo.value,
                            placeholder: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Choose Collection', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_ChooseCollectionPlaceholder'),
                            onValueChanged: (e) => {
                                cellInfo.setValue(e.value);
                            }
                        });
                    }
                },
                {
                    dataField: 'alias',
                    caption: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_QueryAliasColumnName'),
                    validationRules: [{
                            type: 'custom',
                            validationCallback: (options) => {
                                return this._aliasValidationCallback(options.value, options.data);
                            },
                            get message() { return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias is not unique', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_AliasUniqueMessage'); }
                        }]
                },
                {
                    dataField: 'filterString',
                    caption: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Filter', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries_FilterColumnName'),
                    cssClass: 'dxrd-wizard-mongoDB-filtereditor',
                    editCellTemplate: filterCellTemplate,
                },
                {
                    type: 'buttons',
                    width: 50,
                    buttons: [{
                            icon: 'delete',
                            onClick: (e) => {
                                const collectionIndex = this._collections().findIndex(collection => collection.databaseName === e.row.data.databaseName);
                                this._collections.splice(collectionIndex, 1);
                            },
                            cssClass: 'dxrd-querybuilder-grid-actions dxd-icon-highlighted dxrd-image-recycle-bin',
                            template: (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-svg-operations-recycle_bin'),
                        }]
                }
            ]
        };
    }
    _addCollection() {
        this._collections.push(new MongoDBQuery().toCollection());
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            collections: this._collections().map(collection => JSON.stringify(collection)),
        }).promise();
    }
    canNext() {
        return this._isValid();
    }
    initialize(state) {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        if (!this._connectionJson) {
            this._collections((state.collections || []).map(collection => JSON.parse(collection)));
        }
        else if (this._connectionJson === state.connectionJson) {
            return deferred.resolve().promise();
        }
        else {
            this._collections([]);
        }
        this._dataSource = _restoreMongoDBDataSourceFromState(state, this._requestWrapper);
        this._connectionJson = state.connectionJson;
        this._dataSource.getAvailableCollections().done((collections) => {
            this._availableCollections = collections.mongoDBCollections;
            deferred.resolve();
        }).fail((error) => {
            deferred.reject(error);
        });
        return deferred.promise();
    }
    dispose() {
        super.dispose();
        this._bindingContext = null;
    }
}
function _registerChooseMongoDBCollectionPage(factory, requestWrapper, rtl) {
    factory.registerMetadata(MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage, {
        create: () => new ChooseMongoDBCollectionPage(requestWrapper, rtl),
        description: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Configure queries.', 'DataAccessUIStringId.WizardPageConfigureMongoDBQueries'),
        getState: (state) => state.mongoDBDataSourceWizard,
        setState: (data, state) => {
            state.collections = data.collections;
        },
        resetState: (state, defaultState) => {
            state.collections = defaultState.collections;
        },
        template: 'dxrd-wizard-mongoDB-collections'
    });
}

;// ./dist/query-builder/wizard/pages/mongoDBDataSourceWizard/chooseMongoDBDataConnectionPage.js






class ChooseMongoDBDataConnectionPage extends ChooseAvailableItemPage {
    commit() {
        const selectedItem = this.selectedItems()[0].name;
        const connection = this._connection.name() === selectedItem ? this._connection
            : new MongoDBDataConnection();
        connection.name(selectedItem);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            connectionJson: connection.serialize(),
        }).promise();
    }
    initialize(state) {
        this._connection = MongoDBDataConnection.from(state.connectionJson);
        return super.initialize(state);
    }
    _getSelectedItem(state) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.items(), 'name', this._connection.name()) || super._getSelectedItem();
    }
}
function _registerChooseMongoDBDataConnectionPage(factory, wizardOptions) {
    factory.registerMetadata(MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage, {
        create: () => {
            return new ChooseMongoDBDataConnectionPage(wizardOptions.connectionStrings?.mongodb, false, wizardOptions.getMongoDBConnectionStrings);
        },
        description: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Choose a data connection.', 'AnalyticsCoreStringId.SqlDSWizard_PageChooseConnection'),
        getState: (state) => state.mongoDBDataSourceWizard,
        setState: (data, state) => {
            state.connectionJson = data.connectionJson;
        },
        resetState: (state, defaultState) => {
            state.connectionJson = defaultState.connectionJson;
        },
        template: 'dxrd-page-selectitems'
    });
}

;// ./dist/query-builder/wizard/pages/mongoDBDataSourceWizard/chooseMongoDBSchemaPage.js








class ChooseMongoDBSchemaPage extends ChooseJsonSchemaPageBase {
    get _queries() {
        return this._dataSource.queries();
    }
    _getQuaryByName(name) {
        return this._queries.find(query => query.alias() === name || query.collectionName() === name);
    }
    _getRootNode(schema) {
        return schema.nodes[0];
    }
    _updateSchema() {
        if (!this._selectedQuery)
            return;
        this._isLoading(true);
        return this._dataSource.loadQueries().done(() => {
            this._updateFieldListProvider();
            const rootNode = this._getRootNode(this._selectedQuery.schema);
            if (rootNode) {
                this._rootItems(rootNode.nodes.map(node => {
                    return {
                        name: external_ko_namespaceObject.unwrap(node.name),
                        isSelected: external_ko_namespaceObject.unwrap(node.selected),
                        displayName: external_ko_namespaceObject.unwrap(node.displayName) || external_ko_namespaceObject.unwrap(node.name),
                        data: node,
                        specifics: node.nodes.length > 0 ? 'List' : 'Default'
                    };
                }));
            }
            this._onChange();
        }).always(() => {
            this._isLoading(false);
        });
    }
    _mapQuerySchema(query) {
        if (!query?.schema)
            return;
        const rootNode = this._getRootNode(query.schema);
        rootNode?.nodes.forEach(node => {
            const name = node.name();
            this._mapJsonSchema(node, {
                fullPath: name,
                path: name,
                pathParts: [name],
            });
        });
    }
    _updateQueries(state) {
        let currentQueryHasChanged = true;
        const queries = (state.collections || []).map(collection => {
            const query = MongoDBQuery.fromCollection(JSON.parse(collection));
            const existingQuery = this._queries.find(current => {
                return current.id() === query.id() &&
                    current.collectionName() === query.collectionName() &&
                    current.databaseName() === query.databaseName();
            });
            if (existingQuery === this._selectedQuery) {
                currentQueryHasChanged = false;
                this._selectedQuery = query;
            }
            query.schema = existingQuery?.schema;
            return query;
        });
        if (currentQueryHasChanged) {
            this._selectedQuery = null;
            this._selectedQueryName(null);
        }
        this._dataSource.queries(queries);
        this._updateAvailableQueries();
    }
    _updateAvailableQueries() {
        const queries = this._queries.map(query => query.alias() || query.collectionName());
        this._availableQueries(queries);
        if (!queries || queries.length === 0) {
            this._selectedQueryName(null);
            return;
        }
        else {
            if (!this._selectedQueryName() || !queries.includes(this._selectedQueryName())) {
                this._selectedQueryName(queries[0]);
            }
        }
        if (!this._isLoading()) {
            this._updateSchema();
        }
    }
    _createRootNode() {
        const parentNode = new JsonNode({});
        parentNode.nodes = this._rootItems().map(item => item.data);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve(parentNode).promise();
    }
    constructor(requestWrapper) {
        super(requestWrapper);
        this._querySelectBoxTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Query name:', 'DataAccessUIStringId.WizardPageChooseMongoDBSchema_QueryName');
        this._availableQueries = external_ko_namespaceObject.observable([]);
        this._selectedQueryName = external_ko_namespaceObject.observable(null);
        this._isLoading = external_ko_namespaceObject.observable(false);
        this.addDisposable(this._selectedQueryName.subscribe((name) => {
            this._mapQuerySchema(this._selectedQuery);
            if (name) {
                this._selectedQuery = this._getQuaryByName(name);
                this._updateSchema();
            }
            else {
                this._selectedQuery = null;
            }
        }));
    }
    initialize(state) {
        this._clear();
        if (!this._dataSource || this._connectionJson !== state.connectionJson) {
            this._dataSource = _restoreMongoDBDataSourceFromState(state, this._requestWrapper);
        }
        this._connectionJson = state.connectionJson;
        this._updateQueries(state);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    commit() {
        const rootItem = this._rootItems()[0];
        if (!rootItem)
            return;
        this._mapQuerySchema(this._selectedQuery);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            queries: this._dataSource.saveQueries(),
        }).promise();
    }
}
function _registerChooseMongoDBSchemaPage(factory, requestWrapper) {
    factory.registerMetadata(MongoDBDataSourceWizardPageId.ChooseMongoDBSchemaPage, {
        setState: (data, state) => {
            state.queries = data.queries;
        },
        getState: (state) => {
            return state.mongoDBDataSourceWizard;
        },
        resetState: () => { },
        create: () => {
            return new ChooseMongoDBSchemaPage(requestWrapper);
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Select data fields.', 'DataAccessUIStringId.WizardPageChooseMongoDBSchema'),
        template: 'dxrd-mongoDBdatasource-fields-page'
    });
}

;// ./dist/query-builder/utils/_requestwrapper.js

function wrapGetSelectStatement(callback) {
    return (connection, queryJSON) => {
        if (callback)
            return callback(connection, queryJSON);
        return new RequestWrapper().getSelectStatement(connection, queryJSON);
    };
}
function wrapRebuildResultSchema(callback) {
    return (dataSource, queryName, relationsEditing) => {
        if (callback)
            return callback(dataSource, queryName, relationsEditing);
        return new RequestWrapper().rebuildResultSchema(dataSource, queryName, relationsEditing);
    };
}
function wrapGetFederationdResultSchema(callback) {
    return (dataSource) => {
        if (callback)
            return callback(dataSource);
        return new RequestWrapper().getFederationResultSchema(dataSource);
    };
}

;// ./dist/query-builder/elements/columnModelMeta.js


const AggregationType = {
    None: 'None',
    Count: 'Count',
    Max: 'Max',
    Min: 'Min',
    Avg: 'Avg',
    Sum: 'Sum',
    CountDistinct: 'CountDistinct',
    AvgDistinct: 'AvgDistinct',
    SumDistinct: 'SumDistinct'
};
const columnSerializationInfo = [
    query_builder_metadata_name,
    { propertyName: 'displayType', displayName: 'Type', localizationId: 'DataAccessUIStringId.ParametersColumn_Type', disabled: true, editor: editorTemplates.getEditor('text') },
    { propertyName: 'alias', displayName: 'Alias', localizationId: 'DataAccessUIStringId.QueryBuilderColumns_Alias', editor: editorTemplates.getEditor('text') },
    selected,
    {
        propertyName: 'sortingType',
        displayName: 'Sort Type',
        editor: editorTemplates.getEditor('combobox'),
        defaultVal: 'Unsorted',
        valuesArray: [
            { value: 'Unsorted', displayValue: 'Unsorted', localizationId: 'DataAccessUIStringId.SortingTypeNone' },
            { value: 'Ascending', displayValue: 'Ascending', localizationId: 'DataAccessUIStringId.SortingTypeAscending' },
            { value: 'Descending', displayValue: 'Descending', localizationId: 'DataAccessUIStringId.SortingTypeDescending' }
        ],
        localizationId: 'AnalyticsCoreStringId.QueryBuilder_SortType'
    },
    { propertyName: 'sortOrder', displayName: 'Sort Order', editor: editorTemplates.getEditor('numeric'), localizationId: 'DataAccessUIStringId.QueryBuilderColumns_SortOrder' },
    { propertyName: 'groupBy', displayName: 'Group By', editor: editorTemplates.getEditor('bool'), defaultVal: false, localizationId: 'DataAccessUIStringId.QueryBuilderColumns_GroupBy' },
    {
        propertyName: 'aggregate',
        displayName: 'Aggregate',
        editor: editorTemplates.getEditor('comboboxUndo'),
        values: AggregationType,
        defaultVal: AggregationType.None,
        localizationId: 'DataAccessUIStringId.QueryBuilderColumns_Aggregate'
    }
];

;// ./dist/query-builder/elements/columnExpressionMeta.js

const ColumnType = {
    RecordsCount: 'RecordsCount',
    Column: 'Column',
    Expression: 'Expression',
    AllColumns: 'AllColumns',
    AllColumnsQuery: 'AllColumnsQuery'
};
const columnExpressionSerializationsInfo = [
    { propertyName: 'expression', modelName: '#text' },
    { propertyName: 'table', modelName: '@Table' },
    { propertyName: 'column', modelName: '@Name' },
    { propertyName: 'aggregate', modelName: '@Aggregate', defaultVal: AggregationType.None },
    { propertyName: 'alias', modelName: '@Alias' },
    { propertyName: 'descending', modelName: '@Descending', defaultVal: false },
    { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/elements/columnExpression.js







class ColumnExpression {
    constructor(model, query, serializer) {
        this.initialize(model, query, serializer);
    }
    actualName() {
        return this.alias() || this.column();
    }
    initialize(model, query, serializer) {
        serializer = serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        serializer.deserialize(this, model);
        this._dependedTables = [];
        if (this.table()) {
            const tableRef = query.getTable(this.table());
            this.table = external_ko_namespaceObject.pureComputed(() => tableRef.actualName());
            this._dependedTables.push(tableRef);
        }
        else if (this.expression()) {
            try {
                this._criteria = external_DevExpress_Analytics_Criteria_Utils_namespaceObject.CriteriaOperatorStateMachine.parse(this.expression());
                (0,external_DevExpress_Analytics_Criteria_Utils_namespaceObject.criteriaForEach)(this._criteria, (operand) => {
                    if (operand instanceof external_DevExpress_Analytics_Criteria_namespaceObject.OperandProperty) {
                        const dependedTable = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(query.tables(), table => operand.propertyName.indexOf(table.actualName() + '.') === 0);
                        dependedTable && this._dependedTables.push(dependedTable);
                    }
                });
            }
            catch (e) {
            }
        }
        this.__table = this.table;
        this.__column = this.column;
        this.__expression = this.expression;
    }
    toTable() {
        this.table = this.__table;
        this.column = this.__column;
        this.expression = external_ko_namespaceObject.observable(null);
        this.itemType(ColumnType[ColumnType.Column]);
        if (!!this.__column.peek() && this.alias().indexOf('Expr') === 0)
            this.alias(null);
    }
    getInfo() {
        return columnExpressionSerializationsInfo;
    }
    isDepended(tableActualName) {
        return !!(0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._dependedTables, depended => depended.actualName() === tableActualName);
    }
}

;// ./dist/query-builder/utils/_columnExpressionCollectionHelper.js






class ColumnExpressionCollectionHelper {
    static addToColumnCache(collection) {
        collection.forEach(column => {
            this.columnCache[column.table()] ?? (this.columnCache[column.table()] = {});
            this.columnCache[column.table()][column.column()] = column;
        });
    }
    static clearCache() {
        this.columnCache = {};
    }
    static find(collection, tableName, columnName) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(collection(), item => item.column() === columnName && item.table() === tableName);
    }
    static findByName(collection, actualName) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(collection(), (item) => item.actualName() === actualName);
    }
    static removeDependend(collection, tableName) {
        collection.remove((item) => item.isDepended(tableName));
    }
    static toExpresson(column, columns, value) {
        column.table = external_ko_namespaceObject.observable(null);
        column.column = external_ko_namespaceObject.observable(null);
        column.expression = column['__expression'];
        column.itemType(ColumnType[ColumnType.Expression]);
        if (!column.alias())
            column.alias(ColumnExpressionCollectionHelper.setUniqueAlias(columns, 'Expr'));
        column.expression(value);
    }
    static setUniqueAlias(collection, alias) {
        if (ColumnExpressionCollectionHelper.findByName(collection, alias)) {
            return (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueName)(collection().map((item) => item.actualName()), alias + '_');
        }
        return alias;
    }
    static createNew(query, collection, tableName, columnName) {
        let column = this.columnCache[tableName] && this.columnCache[tableName][columnName];
        const model = { '@Table': tableName, '@Name': columnName, '@ItemType': 'Column' };
        if (column) {
            model['@Alias'] = column.alias();
            column.initialize(model, query);
        }
        else {
            column = new ColumnExpression(model, query);
        }
        if (query.columns === collection && !column.alias() && ColumnExpressionCollectionHelper.findByName(collection, column.actualName())) {
            column.alias(this.setUniqueAlias(collection, column.table() + '_' + column.column()));
        }
        return column;
    }
    static addNew(query, collection, table, column, lazy = false) {
        const newItem = this.createNew(query, collection, table, column);
        (lazy ? collection() : collection).push(newItem);
        return newItem;
    }
    static remove(collection, tableName, columnName, lazy = false) {
        if (!lazy) {
            this.addToColumnCache(collection.remove(item => item.column() === columnName && item.table() === tableName));
        }
        else
            for (let i = 0; i < collection().length; i++) {
                if (collection()[i].column() === columnName && collection()[i].table() === tableName) {
                    this.addToColumnCache(collection().splice(i, 1));
                    return;
                }
            }
    }
    static columnTypeToFederated(type) {
        switch (type) {
            case ColumnType[ColumnType.Expression]:
                return FederatedQueryExpressionType[FederatedQueryExpressionType.SelectExpression];
            case ColumnType[ColumnType.AllColumns]:
                return FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllNodeColumnsExpression];
            case ColumnType[ColumnType.AllColumnsQuery]:
                return FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllColumnsExpression];
            default:
                return FederatedQueryExpressionType[FederatedQueryExpressionType.SelectColumnExpression];
        }
    }
    static federatedTypeToColumn(type) {
        switch (type) {
            case FederatedQueryExpressionType[FederatedQueryExpressionType.SelectExpression]:
                return ColumnType[ColumnType.Expression];
            case FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllNodeColumnsExpression]:
                return ColumnType[ColumnType.AllColumns];
            case FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllColumnsExpression]:
                return ColumnType[ColumnType.AllColumnsQuery];
            default:
                return ColumnType[ColumnType.Column];
        }
    }
}
ColumnExpressionCollectionHelper.columnCache = {};

;// ./dist/query-builder/wizard/internal/_queryBuilderPopup.js








class QueryBuilderPopupBase extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(applyNewQuery, rtl = false, customizeQBInitializationData = (options) => options) {
        super();
        this.customizeQBInitializationData = customizeQBInitializationData;
        this._querySource = external_ko_namespaceObject.observable(null);
        this._dbSchemaProvider = external_ko_namespaceObject.observable(null);
        this.designer = external_ko_namespaceObject.observable();
        this.qbOptions = external_ko_namespaceObject.observable(null);
        this.okButtonDisabled = external_ko_namespaceObject.pureComputed(() => { return this.designer() && !this.designer().model().isValid(); });
        this.isVisible = external_ko_namespaceObject.observable(false);
        this.showLoadIndicator = external_ko_namespaceObject.observable(false);
        this.localizationIdMap = {
            'title': { text: 'Query Builder', localizationId: 'DataAccessUIStringId.QueryBuilder' },
            'loading': { text: 'Loading...', localizationId: 'AnalyticsCoreStringId.Loading' },
            'previewResults': { text: 'Preview Results...', localizationId: 'DataAccessUIStringId.QueryBuilderButtons_PreviewResults' },
            'cancel': { text: 'Cancel', localizationId: 'AnalyticsCoreStringId.SearchDialog_Cancel' },
            'ok': { text: 'OK', localizationId: 'DataAccessUIStringId.Button_OK' }
        };
        this._rtl = rtl;
        this._applyQuery = applyNewQuery;
        this.qbOptions(this.qbOptions() || this.customizeQBInitializationData({
            queryBuilderModel: this.designer,
            dbSchemaProvider: this._dbSchemaProvider,
            querySource: this._querySource,
            callbacks: { CustomizeMenuActions: QueryBuilderPopup.customizeQueryBuilderActions },
            rtl: this._rtl
        }));
    }
    show(query, dataSource) {
        this._dataSource = dataSource;
        this._dbSchemaProvider(dataSource.dbSchemaProvider);
        this._querySource(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(query));
        this.isVisible(true);
        this.designer().updateSurface();
    }
    cancelHandler() {
        ColumnExpressionCollectionHelper.clearCache();
        this.isVisible(false);
    }
    previewHandler() {
        this.designer().showPreview();
    }
    okHandler() {
        if (this.designer().model().canSave()) {
            ColumnExpressionCollectionHelper.clearCache();
            this._applyQuery(this.createQuery(), this.showLoadIndicator)
                .done(() => {
                this.isVisible(false);
            });
        }
    }
    onHiddingHandler() {
        this.designer().dataPreview.isVisible(false);
    }
    popupViewModel(element) {
        const $container = (0,external_DevExpress_Analytics_Internal_namespaceObject.getParentContainer)(element);
        return {
            visible: this.isVisible,
            title: this.getDisplayText('title'),
            showTitle: true,
            shading: true,
            fullScreen: false,
            width: '95%',
            height: '95%',
            container: $container,
            wrapperAttr: { class: 'dxrd-querybuilder-popup' },
            position: { of: $container },
            onHidding: this.onHiddingHandler
        };
    }
    getDisplayText(key) {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(this.localizationIdMap[key].text, this.localizationIdMap[key].localizationId);
    }
}
QueryBuilderPopupBase.customizeQueryBuilderActions = (sender, args) => {
    const actions = args.Actions;
    const del = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Delete'), undo = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Undo'), redo = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Redo');
    actions.splice(0, actions.length, del, undo, redo);
};
class QueryBuilderPopup extends QueryBuilderPopupBase {
    constructor(applyNewQuery, rtl = false, customizeQBInitializationData = (options) => options) {
        super(applyNewQuery, rtl, customizeQBInitializationData);
        this.customizeQBInitializationData = customizeQBInitializationData;
    }
    getDataSource() {
        return this._dataSource;
    }
    createQuery() {
        return new TableQuery(this.designer().model().serialize(), this.getDataSource());
    }
}

;// ./dist/query-builder/wizard/internal/_selectQuerySqlTextProvider.js


class SelectQuerySqlTextProvider {
    constructor(_selectStatementCallback, _connection) {
        this._selectStatementCallback = _selectStatementCallback;
        this._connection = _connection;
    }
    getQuerySqlText(newQuery) {
        const queryJSON = JSON.stringify({ 'Query': new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(newQuery) });
        return this._selectStatementCallback(this._connection(), queryJSON)
            .fail((data) => {
            const error = (0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data);
            (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)('Unable to build a SQL string' + (error ? ': ' + error : '.'));
        });
    }
}

;// external "window[\"ace\"]"
const external_window_ace_namespaceObject = window["ace"];
var external_window_ace_default = /*#__PURE__*/__webpack_require__.n(external_window_ace_namespaceObject);
;// ./dist/query-builder/widgets/ace/_ace-mode-sql.js

if (!!(external_window_ace_default())) {
    external_window_ace_default().define('ace/mode/sql_highlight_rules', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text_highlight_rules', 'ace/mode/doc_comment_highlight_rules'], function (require, exports, module) {
        'use strict';
        const oop = external_window_ace_default().require('ace/lib/oop');
        const DocCommentHighlightRules = external_window_ace_default().require('ace/mode/doc_comment_highlight_rules').DocCommentHighlightRules;
        const TextHighlightRules = external_window_ace_default().require('ace/mode/text_highlight_rules').TextHighlightRules;
        const SqlServerHighlightRules = function () {
            let logicalOperators = 'ALL|AND|ANY|BETWEEN|EXISTS|IN|LIKE|NOT|OR|SOME';
            logicalOperators += '|NULL|IS|APPLY|INNER|OUTER|LEFT|RIGHT|JOIN|CROSS';
            const builtinFunctions = ('OPENDATASOURCE|OPENQUERY|OPENROWSET|OPENXML|' +
                'AVG|CHECKSUM_AGG|COUNT|COUNT_BIG|GROUPING|GROUPING_ID|MAX|MIN|STDEV|STDEVP|SUM|VAR|VARP|' +
                'DENSE_RANK|NTILE|RANK|ROW_NUMBER' +
                '@@DATEFIRST|@@DBTS|@@LANGID|@@LANGUAGE|@@LOCK_TIMEOUT|@@MAX_CONNECTIONS|@@MAX_PRECISION|@@NESTLEVEL|@@OPTIONS|@@REMSERVER|@@SERVERNAME|@@SERVICENAME|@@SPID|@@TEXTSIZE|@@VERSION|' +
                'CAST|CONVERT|PARSE|TRY_CAST|TRY_CONVERT|TRY_PARSE' +
                '@@CURSOR_ROWS|@@FETCH_STATUS|CURSOR_STATUS|' +
                '@@DATEFIRST|@@LANGUAGE|CURRENT_TIMESTAMP|DATEADD|DATEDIFF|DATEFROMPARTS|DATENAME|DATEPART|DATETIME2FROMPARTS|DATETIMEFROMPARTS|DATETIMEOFFSETFROMPARTS|DAY|EOMONTH|GETDATE|GETUTCDATE|ISDATE|MONTH|SET DATEFIRST|SET DATEFORMAT|SET LANGUAGE|SMALLDATETIMEFROMPARTS|SP_HELPLANGUAGE|SWITCHOFFSET|SYSDATETIME|SYSDATETIMEOFFSET|SYSUTCDATETIME|TIMEFROMPARTS|TODATETIMEOFFSET|YEAR|' +
                'CHOOSE|IIF|' +
                'ABS|ACOS|ASIN|ATAN|ATN2|CEILING|COS|COT|DEGREES|EXP|FLOOR|LOG|LOG10|PI|POWER|RADIANS|RAND|ROUND|SIGN|SIN|SQRT|SQUARE|TAN|' +
                '@@PROCID|APPLOCK_MODE|APPLOCK_TEST|APP_NAME|ASSEMBLYPROPERTY|COLUMNPROPERTY|COL_LENGTH|COL_NAME|DATABASEPROPERTYEX|DATABASE_PRINCIPAL_ID|DB_ID|DB_NAME|FILEGROUPPROPERTY|FILEGROUP_ID|FILEGROUP_NAME|FILEPROPERTY|FILE_ID|FILE_IDEX|FILE_NAME|FULLTEXTCATALOGPROPERTY|FULLTEXTSERVICEPROPERTY|INDEXKEY_PROPERTY|INDEXPROPERTY|INDEX_COL|OBJECTPROPERTY|OBJECTPROPERTYEX|OBJECT_DEFINITION|OBJECT_ID|OBJECT_NAME|OBJECT_SCHEMA_NAME|ORIGINAL_DB_NAME|PARSENAME|SCHEMA_ID|SCHEMA_NAME|SCOPE_IDENTITY|SERVERPROPERTY|STATS_DATE|TYPEPROPERTY|TYPE_ID|TYPE_NAME|' +
                'CERTENCODED|CERTPRIVATEKEY|CURRENT_USER|DATABASE_PRINCIPAL_ID|HAS_PERMS_BY_NAME|IS_MEMBER|IS_ROLEMEMBER|IS_SRVROLEMEMBER|ORIGINAL_LOGIN|PERMISSIONS|PWDCOMPARE|PWDENCRYPT|SCHEMA_ID|SCHEMA_NAME|SESSION_USER|SUSER_ID|SUSER_NAME|SUSER_SID|SUSER_SNAME|SYS.FN_BUILTIN_PERMISSIONS|SYS.FN_GET_AUDIT_FILE|SYS.FN_MY_PERMISSIONS|SYSTEM_USER|USER_ID|USER_NAME|' +
                'ASCII|CHAR|CHARINDEX|CONCAT|DIFFERENCE|FORMAT|LEN|LOWER|LTRIM|NCHAR|PATINDEX|QUOTENAME|REPLACE|REPLICATE|REVERSE|RTRIM|SOUNDEX|SPACE|STR|STUFF|SUBSTRING|UNICODE|UPPER|' +
                '$PARTITION|@@ERROR|@@IDENTITY|@@PACK_RECEIVED|@@ROWCOUNT|@@TRANCOUNT|BINARY_CHECKSUM|CHECKSUM|CONNECTIONPROPERTY|CONTEXT_INFO|CURRENT_REQUEST_ID|ERROR_LINE|ERROR_MESSAGE|ERROR_NUMBER|ERROR_PROCEDURE|ERROR_SEVERITY|ERROR_STATE|FORMATMESSAGE|GETANSINULL|GET_FILESTREAM_TRANSACTION_CONTEXT|HOST_ID|HOST_NAME|ISNULL|ISNUMERIC|MIN_ACTIVE_ROWVERSION|NEWID|NEWSEQUENTIALID|ROWCOUNT_BIG|XACT_STATE|' +
                '@@CONNECTIONS|@@CPU_BUSY|@@IDLE|@@IO_BUSY|@@PACKET_ERRORS|@@PACK_RECEIVED|@@PACK_SENT|@@TIMETICKS|@@TOTAL_ERRORS|@@TOTAL_READ|@@TOTAL_WRITE|FN_VIRTUALFILESTATS|' +
                'PATINDEX|TEXTPTR|TEXTVALID|' +
                'COALESCE|NULLIF');
            const dataTypes = ('BIGINT|BINARY|BIT|CHAR|CURSOR|DATE|DATETIME|DATETIME2|DATETIMEOFFSET|DECIMAL|FLOAT|HIERARCHYID|IMAGE|INTEGER|INT|MONEY|NCHAR|NTEXT|NUMERIC|NVARCHAR|REAL|SMALLDATETIME|SMALLINT|SMALLMONEY|SQL_VARIANT|TABLE|TEXT|TIME|TIMESTAMP|TINYINT|UNIQUEIDENTIFIER|VARBINARY|VARCHAR|XML');
            const builtInStoredProcedures = 'sp_addextendedproc|sp_addextendedproperty|sp_addmessage|sp_addtype|sp_addumpdevice|sp_add_data_file_recover_suspect_db|sp_add_log_file_recover_suspect_db|sp_altermessage|sp_attach_db|sp_attach_single_file_db|sp_autostats|sp_bindefault|sp_bindrule|sp_bindsession|sp_certify_removable|sp_clean_db_file_free_space|sp_clean_db_free_space|sp_configure|sp_control_plan_guide|sp_createstats|sp_create_plan_guide|sp_create_plan_guide_from_handle|sp_create_removable|sp_cycle_errorlog|sp_datatype_info|sp_dbcmptlevel|sp_dbmmonitoraddmonitoring|sp_dbmmonitorchangealert|sp_dbmmonitorchangemonitoring|sp_dbmmonitordropalert|sp_dbmmonitordropmonitoring|sp_dbmmonitorhelpalert|sp_dbmmonitorhelpmonitoring|sp_dbmmonitorresults|sp_db_increased_partitions|sp_delete_backuphistory|sp_depends|sp_describe_first_result_set|sp_describe_undeclared_parameters|sp_detach_db|sp_dropdevice|sp_dropextendedproc|sp_dropextendedproperty|sp_dropmessage|sp_droptype|sp_execute|sp_executesql|sp_getapplock|sp_getbindtoken|sp_help|sp_helpconstraint|sp_helpdb|sp_helpdevice|sp_helpextendedproc|sp_helpfile|sp_helpfilegroup|sp_helpindex|sp_helplanguage|sp_helpserver|sp_helpsort|sp_helpstats|sp_helptext|sp_helptrigger|sp_indexoption|sp_invalidate_textptr|sp_lock|sp_monitor|sp_prepare|sp_prepexec|sp_prepexecrpc|sp_procoption|sp_recompile|sp_refreshview|sp_releaseapplock|sp_rename|sp_renamedb|sp_resetstatus|sp_sequence_get_range|sp_serveroption|sp_setnetname|sp_settriggerorder|sp_spaceused|sp_tableoption|sp_unbindefault|sp_unbindrule|sp_unprepare|sp_updateextendedproperty|sp_updatestats|sp_validname|sp_who|sys.sp_merge_xtp_checkpoint_files|sys.sp_xtp_bind_db_resource_pool|sys.sp_xtp_checkpoint_force_garbage_collection|sys.sp_xtp_control_proc_exec_stats|sys.sp_xtp_control_query_exec_stats|sys.sp_xtp_unbind_db_resource_pool';
            let keywords = 'ABSOLUTE|ACTION|ADA|ADD|ADMIN|AFTER|AGGREGATE|ALIAS|ALL|ALLOCATE|ALTER|AND|ANY|ARE|ARRAY|AS|ASC|ASENSITIVE|ASSERTION|ASYMMETRIC|AT|ATOMIC|AUTHORIZATION|BACKUP|BEFORE|BEGIN|BETWEEN|BIT_LENGTH|BLOB|BOOLEAN|BOTH|BREADTH|BREAK|BROWSE|BULK|BY|CALL|CALLED|CARDINALITY|CASCADE|CASCADED|CASE|CATALOG|CHARACTER|CHARACTER_LENGTH|CHAR_LENGTH|CHECK|CHECKPOINT|CLASS|CLOB|CLOSE|CLUSTERED|COALESCE|COLLATE|COLLATION|COLLECT|COLUMN|COMMIT|COMPLETION|COMPUTE|CONDITION|CONNECT|CONNECTION|CONSTRAINT|CONSTRAINTS|CONSTRUCTOR|CONTAINS|CONTAINSTABLE|CONTINUE|CORR|CORRESPONDING|COVAR_POP|COVAR_SAMP|CREATE|CROSS|CUBE|CUME_DIST|CURRENT|CURRENT_CATALOG|CURRENT_DATE|CURRENT_DEFAULT_TRANSFORM_GROUP|CURRENT_PATH|CURRENT_ROLE|CURRENT_SCHEMA|CURRENT_TIME|CURRENT_TRANSFORM_GROUP_FOR_TYPE|CYCLE|DATA|DATABASE|DBCC|DEALLOCATE|DEC|DECLARE|DEFAULT|DEFERRABLE|DEFERRED|DELETE|DENY|DEPTH|DEREF|DESC|DESCRIBE|DESCRIPTOR|DESTROY|DESTRUCTOR|DETERMINISTIC|DIAGNOSTICS|DICTIONARY|DISCONNECT|DISK|DISTINCT|DISTRIBUTED|DOMAIN|DOUBLE|DROP|DUMP|DYNAMIC|EACH|ELEMENT|ELSE|END|END-EXEC|EQUALS|ERRLVL|ESCAPE|EVERY|EXCEPT|EXCEPTION|EXEC|EXECUTE|EXISTS|EXIT|EXTERNAL|EXTRACT|FETCH|FILE|FILLFACTOR|FILTER|FIRST|FOR|FOREIGN|FORTRAN|FOUND|FREE|FREETEXT|FREETEXTTABLE|FROM|FULL|FULLTEXTTABLE|FUNCTION|FUSION|GENERAL|GET|GLOBAL|GO|GOTO|GRANT|GROUP|HAVING|HOLD|HOLDLOCK|HOST|HOUR|IDENTITY|IDENTITYCOL|IDENTITY_INSERT|IF|IGNORE|IMMEDIATE|IN|INCLUDE|INDEX|INDICATOR|INITIALIZE|INITIALLY|INNER|INOUT|INPUT|INSENSITIVE|INSERT|INTEGER|INTERSECT|INTERSECTION|INTERVAL|INTO|IS|ISOLATION|ITERATE|JOIN|KEY|KILL|LANGUAGE|LARGE|LAST|LATERAL|LEADING|LESS|LEVEL|LIKE|LIKE_REGEX|LIMIT|LINENO|LN|LOAD|LOCAL|LOCALTIME|LOCALTIMESTAMP|LOCATOR|MAP|MATCH|MEMBER|MERGE|METHOD|MINUTE|MOD|MODIFIES|MODIFY|MODULE|MULTISET|NAMES|NATIONAL|NATURAL|NCLOB|NEW|NEXT|NO|NOCHECK|NONCLUSTERED|NONE|NORMALIZE|NOT|NULL|NULLIF|OBJECT|OCCURRENCES_REGEX|OCTET_LENGTH|OF|OFF|OFFSETS|OLD|ON|ONLY|OPEN|OPERATION|OPTION|OR|ORDER|ORDINALITY|OUT|OUTER|OUTPUT|OVER|OVERLAPS|OVERLAY|PAD|PARAMETER|PARAMETERS|PARTIAL|PARTITION|PASCAL|PATH|PERCENT|PERCENTILE_CONT|PERCENTILE_DISC|PERCENT_RANK|PIVOT|PLAN|POSITION|POSITION_REGEX|POSTFIX|PRECISION|PREFIX|PREORDER|PREPARE|PRESERVE|PRIMARY|PRINT|PRIOR|PRIVILEGES|PROC|PROCEDURE|PUBLIC|RAISERROR|RANGE|READ|READS|READTEXT|RECONFIGURE|RECURSIVE|REF|REFERENCES|REFERENCING|REGR_AVGX|REGR_AVGY|REGR_COUNT|REGR_INTERCEPT|REGR_R2|REGR_SLOPE|REGR_SXX|REGR_SXY|REGR_SYY|RELATIVE|RELEASE|REPLICATION|RESTORE|RESTRICT|RESULT|RETURN|RETURNS|REVERT|REVOKE|ROLE|ROLLBACK|ROLLUP|ROUTINE|ROW|ROWCOUNT|ROWGUIDCOL|ROWS|RULE|SAVE|SAVEPOINT|SCHEMA|SCOPE|SCROLL|SEARCH|SECOND|SECTION|SECURITYAUDIT|SELECT|SEMANTICKEYPHRASETABLE|SEMANTICSIMILARITYDETAILSTABLE|SEMANTICSIMILARITYTABLE|SENSITIVE|SEQUENCE|SESSION|SET|SETS|SETUSER|SHUTDOWN|SIMILAR|SIZE|SOME|SPECIFIC|SPECIFICTYPE|SQL|SQLCA|SQLCODE|SQLERROR|SQLEXCEPTION|SQLSTATE|SQLWARNING|START|STATE|STATEMENT|STATIC|STATISTICS|STDDEV_POP|STDDEV_SAMP|STRUCTURE|SUBMULTISET|SUBSTRING_REGEX|SYMMETRIC|SYSTEM|TABLESAMPLE|TEMPORARY|TERMINATE|TEXTSIZE|THAN|THEN|TIMEZONE_HOUR|TIMEZONE_MINUTE|TO|TOP|TRAILING|TRAN|TRANSACTION|TRANSLATE|TRANSLATE_REGEX|TRANSLATION|TREAT|TRIGGER|TRIM|TRUNCATE|TSEQUAL|UESCAPE|UNDER|UNION|UNIQUE|UNKNOWN|UNNEST|UNPIVOT|UPDATE|UPDATETEXT|USAGE|USE|USER|USING|VALUE|VALUES|VARIABLE|VARYING|VAR_POP|VAR_SAMP|VIEW|WAITFOR|WHEN|WHENEVER|WHERE|WHILE|WIDTH_BUCKET|WINDOW|WITH|WITHIN|WITHIN GROUP|WITHOUT|WORK|WRITE|WRITETEXT|XMLAGG|XMLATTRIBUTES|XMLBINARY|XMLCAST|XMLCOMMENT|XMLCONCAT|XMLDOCUMENT|XMLELEMENT|XMLEXISTS|XMLFOREST|XMLITERATE|XMLNAMESPACES|XMLPARSE|XMLPI|XMLQUERY|XMLSERIALIZE|XMLTABLE|XMLTEXT|XMLVALIDATE|ZONE';
            keywords += '|KEEPIDENTITY|KEEPDEFAULTS|IGNORE_CONSTRAINTS|IGNORE_TRIGGERS|XLOCK|FORCESCAN|FORCESEEK|HOLDLOCK|NOLOCK|NOWAIT|PAGLOCK|READCOMMITTED|READCOMMITTEDLOCK|READPAST|READUNCOMMITTED|REPEATABLEREAD|ROWLOCK|SERIALIZABLE|SNAPSHOT|SPATIAL_WINDOW_MAX_CELLS|TABLOCK|TABLOCKX|UPDLOCK|XLOCK|IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX|EXPAND|VIEWS|FAST|FORCE|KEEP|KEEPFIXED|MAXDOP|MAXRECURSION|OPTIMIZE|PARAMETERIZATION|SIMPLE|FORCED|RECOMPILE|ROBUST|PLAN|SPATIAL_WINDOW_MAX_CELLS|NOEXPAND|HINT';
            keywords += '|LOOP|HASH|MERGE|REMOTE';
            keywords += '|TRY|CATCH|THROW';
            keywords += '|TYPE';
            keywords = keywords.split('|');
            keywords = keywords.filter(function (value, index, self) {
                return logicalOperators.split('|').indexOf(value) === -1 && builtinFunctions.split('|').indexOf(value) === -1 && dataTypes.split('|').indexOf(value) === -1;
            });
            keywords = keywords.sort().join('|');
            const keywordMapper = this.createKeywordMapper({
                'constant.language': logicalOperators,
                'storage.type': dataTypes,
                'support.function': builtinFunctions,
                'support.storedprocedure': builtInStoredProcedures,
                'keyword': keywords
            }, 'identifier', true);
            const setStatements = 'SET ANSI_DEFAULTS|SET ANSI_NULLS|SET ANSI_NULL_DFLT_OFF|SET ANSI_NULL_DFLT_ON|SET ANSI_PADDING|SET ANSI_WARNINGS|SET ARITHABORT|SET ARITHIGNORE|SET CONCAT_NULL_YIELDS_NULL|SET CURSOR_CLOSE_ON_COMMIT|SET DATEFIRST|SET DATEFORMAT|SET DEADLOCK_PRIORITY|SET FIPS_FLAGGER|SET FMTONLY|SET FORCEPLAN|SET IDENTITY_INSERT|SET IMPLICIT_TRANSACTIONS|SET LANGUAGE|SET LOCK_TIMEOUT|SET NOCOUNT|SET NOEXEC|SET NUMERIC_ROUNDABORT|SET OFFSETS|SET PARSEONLY|SET QUERY_GOVERNOR_COST_LIMIT|SET QUOTED_IDENTIFIER|SET REMOTE_PROC_TRANSACTIONS|SET ROWCOUNT|SET SHOWPLAN_ALL|SET SHOWPLAN_TEXT|SET SHOWPLAN_XML|SET STATISTICS IO|SET STATISTICS PROFILE|SET STATISTICS TIME|SET STATISTICS XML|SET TEXTSIZE|SET XACT_ABORT'.split('|');
            const isolationLevels = 'READ UNCOMMITTED|READ COMMITTED|REPEATABLE READ|SNAPSHOP|SERIALIZABLE'.split('|');
            for (let i = 0; i < isolationLevels.length; i++) {
                setStatements.push('SET TRANSACTION ISOLATION LEVEL ' + isolationLevels[i]);
            }
            this.$rules = {
                start: [{
                        token: 'string.start',
                        regex: "'",
                        next: [{
                                token: 'constant.language.escape',
                                regex: /''/
                            }, {
                                token: 'string.end',
                                next: 'start',
                                regex: "'"
                            }, {
                                defaultToken: 'string'
                            }]
                    },
                    DocCommentHighlightRules.getStartRule('doc-start'), {
                        token: 'comment',
                        regex: '--.*$'
                    }, {
                        token: 'comment',
                        start: '/\\*',
                        end: '\\*/'
                    }, {
                        token: 'constant.numeric',
                        regex: '[+-]?\\d+(?:(?:\\.\\d*)?(?:[eE][+-]?\\d+)?)?\\b'
                    }, {
                        token: keywordMapper,
                        regex: '@{0,2}[a-zA-Z_$][a-zA-Z0-9_$]*\\b(?!])'
                    }, {
                        token: 'constant.class',
                        regex: '@@?[a-zA-Z_$][a-zA-Z0-9_$]*\\b'
                    }, {
                        token: 'keyword.operator',
                        regex: '\\+|\\-|\\/|\\/\\/|%|<@>|@>|<@|&|\\^|~|<|>|<=|=>|==|!=|<>|=|\\*'
                    }, {
                        token: 'paren.lparen',
                        regex: '[\\(]'
                    }, {
                        token: 'paren.rparen',
                        regex: '[\\)]'
                    }, {
                        token: 'punctuation',
                        regex: ',|;'
                    }, {
                        token: 'text',
                        regex: '\\s+'
                    }],
                comment: [
                    DocCommentHighlightRules.getTagRule(), {
                        token: 'comment',
                        regex: '\\*\\/',
                        next: 'no_regex'
                    }, {
                        defaultToken: 'comment',
                        caseInsensitive: true
                    }
                ]
            };
            for (let i = 0; i < setStatements.length; i++) {
                this.$rules.start.unshift({
                    token: 'set.statement',
                    regex: setStatements[i]
                });
            }
            this.embedRules(DocCommentHighlightRules, 'doc-', [DocCommentHighlightRules.getEndRule('start')]);
            this.normalizeRules();
            const completions = [];
            const addCompletions = function (arr, meta) {
                arr.forEach(function (v) {
                    completions.push({
                        name: v,
                        value: v,
                        score: 0,
                        meta: meta
                    });
                });
            };
            addCompletions(builtInStoredProcedures.split('|'), 'procedure');
            addCompletions(logicalOperators.split('|'), 'operator');
            addCompletions(builtinFunctions.split('|'), 'function');
            addCompletions(dataTypes.split('|'), 'type');
            addCompletions(setStatements, 'statement');
            addCompletions(keywords.split('|'), 'keyword');
            this.completions = completions;
        };
        oop.inherits(SqlServerHighlightRules, TextHighlightRules);
        exports.SqlHighlightRules = SqlServerHighlightRules;
    });
    external_window_ace_default().define('ace/mode/sql', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text', 'ace/mode/sql_highlight_rules', 'ace/range'], function (require, exports, module) {
        'use strict';
        const oop = external_window_ace_default().require('ace/lib/oop');
        const TextMode = external_window_ace_default().require('ace/mode/text').Mode;
        const SqlHighlightRules = external_window_ace_default().require('ace/mode/sql_highlight_rules').SqlHighlightRules;
        const Range = external_window_ace_default().require('ace/range').Range;
        const Mode = function () {
            this.HighlightRules = SqlHighlightRules;
        };
        oop.inherits(Mode, TextMode);
        (function () {
            this.lineCommentStart = '--';
            this.$id = 'ace/mode/sql';
        }).call(Mode.prototype);
        exports.Mode = Mode;
    });
}

;// ./dist/query-builder/widgets/ace/_options.js

function createDefaultSQLAceOptions(readOnly = false) {
    return {
        showLineNumbers: false,
        showPrintMargin: false,
        enableBasicAutocompletion: false,
        enableLiveAutocompletion: false,
        readOnly: readOnly,
        highlightSelectedWord: readOnly,
        showGutter: false,
        highlightActiveLine: false
    };
}
function createDefaultSQLAdditionalOptions(value) {
    return {
        onChange: (session) => {
            value(session.getValue());
        },
        onValueChange: (editor) => {
            editor.resize(true);
        },
        changeTimeout: 200,
        overrideEditorFocus: true,
        setUseWrapMode: true
    };
}
function createDefaultSQLLanguageHelper() {
    return {
        getLanguageMode: () => 'ace/mode/sql',
        createCompleters: () => { return []; }
    };
}

;// ./dist/query-builder/wizard/internal/_selectStatementQueryControl.js









class SelectStatementQueryControl extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(sqlTextProvider, disableCustomSql) {
        super();
        this._tableQueryString = external_ko_namespaceObject.observable('');
        this._query = external_ko_namespaceObject.observable();
        this._needToCustomizeParameters = external_ko_namespaceObject.pureComputed(() => {
            return this._query() && (this._query().type() === SqlQueryType.customSqlQuery || this._query().parameters().length > 0);
        });
        this.template = 'dxrd-select-control';
        this.aceOptions = createDefaultSQLAceOptions();
        this.additionalOptions = createDefaultSQLAdditionalOptions((newVal) => { this.sqlString(newVal); });
        this.aceAvailable = (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.aceAvailable)();
        this.languageHelper = createDefaultSQLLanguageHelper();
        this.caption = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('SQL string:', 'DataAccessUIStringId.QueryControl_SqlString');
        this.sqlString = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this._query() && this._query().type() === SqlQueryType.customSqlQuery ? this._query().sqlString() : this._tableQueryString();
            },
            write: (val) => {
                if (this._query().type() !== SqlQueryType.customSqlQuery) {
                    const customQuery = new CustomSqlQuery({ '@Name': this._query().name() }, this._query().parent);
                    customQuery.parameters(this._query().parameters());
                    customQuery.sqlString(val);
                    this._query(customQuery);
                }
                else {
                    this._query().sqlString(val);
                }
            }
        });
        this.isNextDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this.sqlString() || !this._needToCustomizeParameters();
        });
        this.isFinishDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this.sqlString() || this._needToCustomizeParameters();
        });
        this._sqlTextProvider = sqlTextProvider;
        this.disableCustomSql = () => disableCustomSql;
        this.aceOptions.readOnly = this.disableCustomSql();
        this._disposables.push(this._needToCustomizeParameters, this.sqlString);
    }
    setQuery(query, isInProcess) {
        if (this._query() !== query && query.type() === SqlQueryType.tableQuery) {
            isInProcess && isInProcess(true);
            return this._sqlTextProvider.getQuerySqlText(query)
                .done((response) => {
                if (response.errorMessage)
                    (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)(response.errorMessage);
                this._tableQueryString(response.sqlSelectStatement);
                this._query(query);
            })
                .always(() => { isInProcess && isInProcess(false); });
        }
        else {
            this._query(query);
            return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve().promise();
        }
    }
    getQuery() {
        return this._query();
    }
    get runQueryBuilderDisabled() {
        return false;
    }
}

;// ./dist/query-builder/wizard/internal/_storedProceduresQueryControl.js










class StoredProceduresQueryControl extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _needToProcessParameters(procedure) {
        return procedure.arguments.some(StoredProceduresQueryControl._availableConvertToParameter);
    }
    static _availableConvertToParameter(arg) {
        return arg.direction !== DBStoredProcedureArgumentDirection.Out;
    }
    get _selectedProcedure() {
        return this.selectedProcedure()[0];
    }
    set _selectedProcedure(value) {
        this.selectedProcedure(value ? [value] : []);
    }
    constructor() {
        super();
        this.template = 'dxrd-procedures-control';
        this.storedProcedures = external_ko_namespaceObject.observableArray([]);
        this.selectedProcedure = external_ko_namespaceObject.observableArray([]);
        this.caption = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Select a stored procedure:', 'DataAccessUIStringId.StoredProcControl_Caption');
        this.generateStoredProcedureDisplayName = procedure => StoredProceduresQueryControl.generateStoredProcedureDisplayName(procedure);
        this.isNextDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this._selectedProcedure || !this._needToProcessParameters(this._selectedProcedure);
        });
        this.isFinishDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this._selectedProcedure || this._needToProcessParameters(this._selectedProcedure);
        });
        this.storedProcedures.subscribe((newProcedures) => {
            if (!newProcedures) {
                this._selectedProcedure = null;
            }
            else if (this._selectedProcedure) {
                this._selectedProcedure = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.storedProcedures(), 'name', this._selectedProcedure.name);
            }
            else if (this._query && this._query.procName()) {
                this._selectedProcedure = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.storedProcedures(), 'name', this._query.procName());
            }
            else {
                this._selectedProcedure = newProcedures[0];
            }
        });
    }
    scrollActiveItem(e) {
        const model = e.model;
        const procedure = model.selectedProcedure.peek();
        e.component.scrollToItem(procedure[0]);
    }
    static generateStoredProcedureDisplayName(procedure) {
        if (procedure.arguments.length === 0)
            return procedure.name;
        return procedure.arguments.reduce((value, item, index, array) => {
            return value += item.name + (index < array.length - 1 ? ', ' : ')');
        }, procedure.name + '(');
    }
    setQuery(query) {
        this._query = query;
        this._selectedProcedure = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.storedProcedures(), 'name', query.procName());
        return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve().promise();
    }
    getQuery() {
        if (!this._selectedProcedure)
            return null;
        const newQuery = new StoredProcQuery({ '@Name': this._query.name() || this._selectedProcedure.name, 'ProcName': this._selectedProcedure.name }, this._query.parent);
        this._selectedProcedure.arguments.forEach((arg) => {
            if (StoredProceduresQueryControl._availableConvertToParameter(arg)) {
                newQuery.parameters.push((0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this._query.parameters(), 'name', arg.name) || new DataSourceParameter({ '@Name': arg.name, '@Type': DBColumn.GetType(arg.type) }, null, storedProcParameterSerializationsInfo(DBColumn.GetType(arg.type))));
            }
        });
        return newQuery;
    }
    get runQueryBuilderDisabled() {
        return true;
    }
}

;// ./dist/query-builder/wizard/pages/sqlDataSourceWizard/configureQueryPage.js




















class ConfigureQueryPage extends WizardPageBase {
    constructor(_options) {
        super();
        this._options = _options;
        this._connection = () => {
            return this._dataSource().connection;
        };
        this._dataSource = () => {
            return this._dataSourceWrapper && this._dataSourceWrapper.sqlDataSource;
        };
        this.queryNameCaption = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Query Name', 'AnalyticsCoreStringId.SqlDSWizard_QueryName') + ':';
        this.queryControl = external_ko_namespaceObject.observable();
        this.runQueryBuilderBtnText = external_ko_namespaceObject.pureComputed(() => {
            return (!this._selectStatementControl.sqlString() || this._selectStatementControl.getQuery().type() === SqlQueryType.tableQuery) ?
                (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Run Query Builder...', 'DataAccessUIStringId.Button_QueryBuilder') :
                (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Create New Query...', 'AnalyticsCoreStringId.SqlDSWizard_CreateNewQuery');
        }).extend({ deferred: true });
        this.placeholder = () => {
            const queryControl = this.queryControl();
            const query = queryControl && queryControl.getQuery();
            const queryName = this.initialName || (query && query.generateName());
            return queryName ? (0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Type custom query name (current name: {0})', 'AnalyticsCoreStringId.SqlDSWizard_QueryNamePlaceholder'), queryName) : queryName;
        };
        this.queryName = external_ko_namespaceObject.observable('');
        this.queryTypeItems = [ConfigureQueryPage.QUERY_TEXT, ConfigureQueryPage.SP_TEXT];
        this.selectedQueryType = external_ko_namespaceObject.observable();
        this.initialName = '';
        this._proceduresList = new StoredProceduresQueryControl();
        this._disposables.push(this._selectStatementControl = new SelectStatementQueryControl(new SelectQuerySqlTextProvider(wrapGetSelectStatement(this._options.callbacks.selectStatement), this._connection), this._options.disableCustomSql));
        this._disposables.push(this.selectedQueryType.subscribe((value) => {
            if (value === ConfigureQueryPage.SP_TEXT) {
                this[__loadingStateFunctionName] && this[__loadingStateFunctionName](true);
                this._dataSource().dbSchemaProvider.getDbStoredProcedures().done((procedures) => {
                    this._proceduresList.storedProcedures([]);
                    this._proceduresList.storedProcedures(procedures);
                }).always((x) => this[__loadingStateFunctionName] && this[__loadingStateFunctionName](false));
                this.queryControl(this._proceduresList);
            }
            else {
                this.queryControl(this._selectStatementControl);
            }
        }));
        this.selectedQueryType(ConfigureQueryPage.QUERY_TEXT);
        this.popupQueryBuilder = new QueryBuilderPopup((newQuery, isInProcess) => {
            this.queryName(newQuery.name() || newQuery.generateName());
            return this._selectStatementControl.setQuery(newQuery, isInProcess);
        }, this._options.rtl, this._options.callbacks.customizeQBInitData);
    }
    canNext() {
        return !this.queryControl().isNextDisabled();
    }
    canFinish() {
        return !this.queryControl().isFinishDisabled() || !this.queryControl().isNextDisabled();
    }
    runQueryBuilder() {
        const query = this.queryControl().getQuery();
        if (query && query.type() === SqlQueryType.tableQuery) {
            query.name(this.queryName());
            this.popupQueryBuilder.show(query, this._dataSource());
        }
        else {
            this.popupQueryBuilder.show(new TableQuery({ '@Name': this.queryName() }, this._dataSource()), this._dataSource());
        }
    }
    localizeQueryType(queryTypeString) {
        return ConfigureQueryPage.QUERY_TEXT === queryTypeString ?
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(ConfigureQueryPage.QUERY_TEXT, 'DataAccessUIStringId.WizardPageConfigureQuery_Query') :
            (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(ConfigureQueryPage.SP_TEXT, 'DataAccessUIStringId.WizardPageConfigureQuery_StoredProcedure');
    }
    initialize(state) {
        this._dataSourceWrapper = _restoreSqlDataSourceFromState(state, this._options.requestWrapper);
        this._proceduresList.setQuery(new StoredProcQuery({}, this._dataSource()));
        this._selectStatementControl.setQuery(new CustomSqlQuery({}, this._dataSource()));
        this.popupQueryBuilder.isVisible(false);
        if (this._dataSourceWrapper.sqlQuery) {
            this.initialName = this._dataSourceWrapper.sqlQuery.name();
            this.queryName(this.initialName);
            this.selectedQueryType(this._dataSourceWrapper.sqlQuery.type() === SqlQueryType.storedProcQuery ? ConfigureQueryPage.SP_TEXT : ConfigureQueryPage.QUERY_TEXT);
            return this.queryControl().setQuery(this._dataSourceWrapper.sqlQuery);
        }
        else {
            this.selectedQueryType(ConfigureQueryPage.QUERY_TEXT);
        }
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    commit() {
        const query = this.queryControl().getQuery();
        if (query) {
            const newQueryName = this.queryName() || this.initialName;
            newQueryName && query.name(newQueryName);
            if (!query.name() || !this._dataSourceWrapper.sqlQuery || this._dataSourceWrapper.sqlQuery.name() !== query.name())
                query.name(generateQueryUniqueName(this._dataSource().queries(), query));
            this._dataSourceWrapper.sqlQuery = query;
        }
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            queryName: this._dataSourceWrapper.sqlQuery.name(),
            sqlDataSourceJSON: this._dataSourceWrapper.save()
        }).promise();
    }
}
ConfigureQueryPage.QUERY_TEXT = 'Query';
ConfigureQueryPage.SP_TEXT = 'Stored Procedure';
function _registerConfigureQueryPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(SqlDataSourceWizardPageId.ConfigureQueryPage, {
        create: () => {
            return new ConfigureQueryPage(dataSourceWizardOptions);
        },
        setState: (data, state) => {
            state.queryName = data.queryName;
            state.sqlDataSourceJSON = data.sqlDataSourceJSON;
        },
        getState: (state) => {
            return state.sqlDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.sqlDataSourceJSON = defaultState.sqlDataSourceJSON;
            state.queryName = defaultState.queryName;
        },
        template: 'dxrd-wizard-create-query-page',
        description: (0,external_DevExpress_Analytics_Internal_namespaceObject.localizeWithUpdateLocalizationMethod)('Create a query or select a stored procedure') || (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Create a query or select a stored procedure.', external_DevExpress_Analytics_Internal_namespaceObject.StringId.WizardPageConfigureQuery)
    });
}

;// ./dist/query-builder/wizard/pages/sqlDataSourceWizard/configureParametersPage.js












class ConfigureQueryParametersPage extends WizardPageBase {
    _isParametersValid() {
        return this.getParameters().every(x => x.isValid());
    }
    constructor(parametersConverter = {
        createParameterViewModel: parameter => parameter,
        getParameterFromViewModel: parameterViewModel => parameterViewModel
    }, _requestWrapper) {
        super();
        this.parametersConverter = parametersConverter;
        this._requestWrapper = _requestWrapper;
        this._sqlDataSourceWrapper = new _SqlDataSourceWrapper(undefined, undefined, this._requestWrapper);
        this.removeButtonTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Remove', 'DataAccessUIStringId.Button_Remove');
        this.parametersEditorOptions = {
            addHandler: () => {
                return this.parametersConverter.createParameterViewModel(new DataSourceParameter({
                    '@Name': (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueNameForNamedObjectsArray)(this.parametersEditorOptions.values.peek().peek(), 'parameter'),
                    '@Type': external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32
                }));
            },
            values: external_ko_namespaceObject.observable(external_ko_namespaceObject.observableArray([])),
            displayName: 'Parameters',
            level: 0,
            info: external_ko_namespaceObject.observable({
                displayName: 'Parameters', localizationId: 'DevExpress.DataAccess.Sql.SqlQuery.Parameters',
                propertyName: 'parameters',
                modelName: 'Parameter',
                array: true,
                editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('commonCollection'),
                template: '#dxrd-parameter-collection-item'
            }),
            editorTemplate: '#dxrd-wizard-datasource-parameters',
            hideButtons: external_ko_namespaceObject.observable(false),
            collapsed: false
        };
    }
    canNext() {
        return false;
    }
    canFinish() {
        return this._isParametersValid();
    }
    getParameters() {
        return this.parametersEditorOptions.values()();
    }
    initialize(data) {
        this._sqlDataSourceWrapper = _restoreSqlDataSourceFromState(data, this._requestWrapper);
        this.parametersEditorOptions.hideButtons(this._sqlDataSourceWrapper.sqlQuery.type() === SqlQueryType.storedProcQuery);
        setTimeout(() => {
            this.parametersEditorOptions.values(external_ko_namespaceObject.observableArray(this._sqlDataSourceWrapper.sqlQuery.parameters().map(item => this.parametersConverter.createParameterViewModel(item))));
        }, 100);
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    commit() {
        this._sqlDataSourceWrapper.sqlQuery.parameters(this.parametersEditorOptions.values()().map(item => this.parametersConverter.getParameterFromViewModel(item)));
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            sqlDataSourceJSON: this._sqlDataSourceWrapper.save()
        }).promise();
    }
}
function _registerConfigureParametersPage(factory, requestWrapper, parametersConverter) {
    factory.registerMetadata(SqlDataSourceWizardPageId.ConfigureParametersPage, {
        create: () => new ConfigureQueryParametersPage(parametersConverter, requestWrapper),
        getState: (state) => state.sqlDataSourceWizard,
        setState: (result, state) => state.sqlDataSourceJSON = result.sqlDataSourceJSON,
        resetState: () => void 0,
        template: 'dxrd-page-configure-parameters',
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure query parameters.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureParameters')
    });
}

;// ./dist/query-builder/wizard/internal/_dbSchemaItemsProvider.js







class DBSchemaItemsProvider extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(dbSchemaProvider, customQueries, showQbCallBack, disableCustomSql, afterCheckToggled) {
        super();
        this._callBack = external_ko_namespaceObject.observable({
            deleteAction: (name) => {
                this._customQueries
                    .remove((0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._customQueries(), item => (item.name() || item.generateName()) === name));
            },
            showQbCallBack: null,
            disableCustomSql: false,
        });
        this._checkedRootNodesCount = external_ko_namespaceObject.pureComputed(() => {
            let count = 0;
            for (let i = 0; i < this._rootItems.length && count < 2; i++) {
                count += this._rootItems[i].countChecked();
            }
            return count;
        });
        this.hasCheckedItems = external_ko_namespaceObject.pureComputed(() => {
            return !(this._checkedRootNodesCount() === 0);
        });
        this.nextButtonDisabled = external_ko_namespaceObject.pureComputed(() => {
            if (this._checkedRootNodesCount() > 1) {
                return false;
            }
            return !this.hasParametersToEdit();
        });
        this.hasParametersToEdit = external_ko_namespaceObject.pureComputed(() => {
            for (let i = 0; i < this._rootItems.length; i++) {
                if (this._rootItems[i]['hasParamsToEdit'] && this._rootItems[i]['hasParamsToEdit']())
                    return true;
            }
            return false;
        });
        this.tables = () => this._tables;
        this.views = () => this._views;
        this.procedures = () => this._procedures;
        this.queries = () => this._queries;
        this.customQueries = () => this._customQueries;
        this._callBack().showQbCallBack = showQbCallBack;
        this._callBack().disableCustomSql = disableCustomSql;
        this._tables = new TreeNode('tables', (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Tables', 'DataAccessStringId.ConfigureMultiQueryPage_TableCategory'), 'list', false, afterCheckToggled);
        this._views = new TreeNode('views', (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Views', 'DataAccessStringId.ConfigureMultiQueryPage_ViewCategory'), 'list', false, afterCheckToggled);
        this._procedures = new ParameterTreeNode('procedures', (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Stored Procedures', 'DataAccessStringId.ConfigureMultiQueryPage_SpCategory'), 'list', false, afterCheckToggled);
        this._queries = new QueriesTreeNode('queries', (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Queries', 'DataAccessStringId.ConfigureMultiQueryPage_QueryCategory'), 'list', false, this._callBack, afterCheckToggled);
        this._disposables.push(...[this._tables, this._views, this._procedures, this._queries]);
        this._rootItems = [
            this._tables,
            this._views,
            this._procedures,
            this._queries
        ];
        this.itemsRequestDeferreds = external_ko_namespaceObject.observableArray([]);
        this.getItems = (pathRequest) => {
            const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (!pathRequest.fullPath) {
                result.resolve(this._rootItems);
            }
            else if (pathRequest.fullPath === 'tables') {
                dbSchemaProvider.getDbTables().done((dbSchema) => {
                    if (this._tables.children().length === 0) {
                        const tables = [];
                        dbSchema.tables.forEach(table => {
                            if (!table.isView) {
                                tables.push(new TreeNode(table.name, table.name, 'table', this._tables.checked.peek(), afterCheckToggled));
                            }
                        });
                        this._tables.initializeChildren(tables);
                        result.resolve(tables);
                    }
                    else {
                        result.resolve(this._tables.children());
                    }
                }).fail((failResponse) => result.reject(failResponse));
            }
            else if (pathRequest.fullPath === 'views') {
                dbSchemaProvider.getDbViews().done((dbSchema) => {
                    if (this._views.children().length === 0) {
                        const views = [];
                        dbSchema.tables.forEach(table => {
                            if (table.isView) {
                                views.push(new TreeNode(table.name, table.name, 'view', this._views.checked.peek(), afterCheckToggled));
                            }
                        });
                        this._views.initializeChildren(views);
                        result.resolve(views);
                    }
                    else {
                        result.resolve(this._views.children());
                    }
                }).fail((failResponse) => result.reject(failResponse));
            }
            else if (pathRequest.fullPath === 'procedures') {
                dbSchemaProvider.getDbStoredProcedures().done((storedProcedures) => {
                    if (this._procedures.children().length === 0) {
                        const procedures = storedProcedures.map(proc => {
                            const node = new TreeLeafNode(proc.name, StoredProceduresQueryControl.generateStoredProcedureDisplayName(proc), 'procedure', this._procedures.checked.peek(), proc.arguments, afterCheckToggled);
                            this._disposables.push(node);
                            return node;
                        });
                        this._procedures.initializeChildren(procedures);
                        result.resolve(procedures);
                    }
                    else {
                        result.resolve(this._procedures.children());
                    }
                }).fail((failResponse) => result.reject(failResponse));
            }
            else if (pathRequest.fullPath === 'queries') {
                const queries = customQueries().map(query => {
                    const name = query.name() || query.generateName();
                    const currentQuery = this._queries.children().filter(q => q['query'] === query)[0];
                    if (currentQuery)
                        return currentQuery;
                    const queryNode = new TreeQueryNode(name, name, 'query', !!currentQuery && currentQuery.checked(), query.parameters, this._callBack, afterCheckToggled, query);
                    this._disposables.push(queryNode);
                    queryNode.setObservableName(() => query.name() || query.generateName(), (newVal) => query.name(newVal));
                    return queryNode;
                });
                this._queries.initializeChildren(queries);
                result.resolve(queries);
            }
            else {
                dbSchemaProvider.getDbTable(pathRequest.path, pathRequest.fullPath).done((table) => {
                    let tableTreeNode;
                    if (table.isView) {
                        tableTreeNode = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._views.children(), item => item.name === table.name);
                    }
                    else {
                        tableTreeNode = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._tables.children(), item => item.name === table.name);
                    }
                    if (tableTreeNode.children().length === 0) {
                        const columns = table.columns.map(column => {
                            const node = new TreeLeafNode(column.name, column.name, 'column', tableTreeNode.checked.peek(), null, afterCheckToggled);
                            this._disposables.push(node);
                            return node;
                        });
                        tableTreeNode.initializeChildren(columns);
                        result.resolve(columns);
                    }
                    else {
                        result.resolve(tableTreeNode.children());
                    }
                }).fail((failResponse) => result.reject(failResponse));
            }
            this.addRequestDeferred(result);
            return result.promise();
        };
        this._customQueries = customQueries;
    }
    addRequestDeferred(deferred) {
        this.itemsRequestDeferreds.push(deferred);
        deferred.always(() => {
            this.itemsRequestDeferreds.remove(deferred);
        });
    }
    dispose() {
        super.dispose();
        this.itemsRequestDeferreds().forEach(deferred => deferred.reject());
        this.itemsRequestDeferreds([]);
    }
}

;// ./dist/query-builder/wizard/stateManager.js

class StateManager {
    _getPageState(pageId, state = this.globalState) {
        return this.pageFactory.getMetadata(pageId).getState(state);
    }
    constructor(globalState, pageFactory) {
        this.globalState = globalState;
        this.pageFactory = pageFactory;
        this.defaultState = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, globalState);
    }
    setPageState(pageId, data) {
        this.pageFactory.getMetadata(pageId).setState(data, this.getPageState(pageId));
    }
    getPageState(pageId) {
        return this._getPageState(pageId);
    }
    resetPageState(pageId) {
        const defaultState = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, this._getPageState(pageId, this.defaultState));
        this.pageFactory.getMetadata(pageId).resetState(this.getPageState(pageId), defaultState);
    }
    getCurrentState() {
        return this.globalState;
    }
    reset() {
        this.globalState.reset();
    }
}

;// ./dist/query-builder/wizard/pageIterator.js



class PageIterator extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    dispose() {
        this._pages.forEach(x => x.dispose());
        this._pages = [];
    }
    __resetPages(fromIndex) {
        if (fromIndex < this._pages.length) {
            for (let index = this._pages.length - 1; index >= fromIndex; index--) {
                this.stateManager.resetPageState(this._pages[index].pageId);
                this._onResetPage(this._pages[index]);
                this._pages[index].dispose();
                this._pages.splice(index, 1);
            }
        }
    }
    _nextPage() {
        return this._pages[this._currentIndex + 1];
    }
    _getNextExistingPage() {
        this._currentIndex += 1;
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        deferred.resolve(this._pages[this._currentIndex]);
        return deferred.promise();
    }
    _resetPages() {
        this.__resetPages(this._currentIndex + 1);
    }
    _getNextNewPage(nextPageId) {
        this._currentIndex += 1;
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        this.__resetPages(this._currentIndex);
        const pageMetadata = this.pageFactory.getMetadata(nextPageId);
        const newPage = new _WrappedWizardPage(nextPageId, pageMetadata.create(), pageMetadata.template, pageMetadata.description);
        this._pages.push(newPage);
        deferred.resolve(newPage);
        return deferred.promise();
    }
    constructor(pageFactory, stateManager, _onResetPage = () => void 0) {
        super();
        this.pageFactory = pageFactory;
        this.stateManager = stateManager;
        this._onResetPage = _onResetPage;
        this._pages = [];
        this._currentIndex = 0;
    }
    _getStartPage(pageId) {
        pageId = pageId || this.getNextPageId();
        const pageMetadata = this.pageFactory.getMetadata(pageId);
        const startPage = new _WrappedWizardPage(pageId, pageMetadata.create(), pageMetadata.template, pageMetadata.description);
        this._pages.push(startPage);
        return startPage;
    }
    _getNextPage() {
        const currentPage = this._getCurrentPage();
        if (currentPage.isChanged || !this._nextPage()) {
            const nextPageId = this.getNextPageId(this._getCurrentPage().pageId);
            if (!nextPageId)
                return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().reject().promise();
            currentPage.isChanged = false;
            return this._getNextNewPage(nextPageId);
        }
        else
            return this._getNextExistingPage();
    }
    _getPreviousPage() {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        if (this._currentIndex - 1 < 0) {
            deferred.reject(null);
        }
        else {
            this._currentIndex -= 1;
            deferred.resolve(this._pages[this._currentIndex]);
        }
        return deferred.promise();
    }
    _goToPage(pageId) {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        const page = this._pages.filter(page => page.pageId === pageId)[0];
        if (page) {
            this._currentIndex = this._pages.indexOf(page);
            deferred.resolve(page);
        }
        else {
            deferred.reject(null);
        }
        return deferred.promise();
    }
    _getCurrentPage() {
        return this._pages[this._currentIndex];
    }
    _getCurrentState() {
        return this.stateManager.getCurrentState();
    }
    getNextPageId(pageId) {
        return '';
    }
}

;// ./dist/query-builder/wizard/wizard.js







class BaseWizard extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _createLoadingState(page) {
        if (!page[__loadingStateFunctionName]) {
            page[__loadingStateFunctionName] = (newVal) => this._loadingState(newVal);
        }
    }
    _createNextAction(page) {
        if (!page[__nextActionFunctionName]) {
            page[__nextActionFunctionName] = () => this.nextAction();
        }
    }
    _loadingState(active) {
        if (active) {
            if (!this._currentActivateCount) {
                this._loadingTimeout && clearTimeout(this._loadingTimeout);
                this._loadingTimeout = setTimeout(() => {
                    if (this._currentActivateCount)
                        this.isLoading(true);
                }, 100);
            }
            this._currentActivateCount++;
        }
        else {
            this._currentActivateCount--;
            if (!this._currentActivateCount) {
                this._loadingTimeout && clearTimeout(this._loadingTimeout);
                this.isLoading(false);
            }
        }
    }
    _callBeforeFinishHandler(state, wizardModel) {
        this.events.call('beforeFinish', { state: state });
    }
    _callAfterFinishHandler(state, result) {
        this.events.call('afterFinish', { state: state });
    }
    onFinish() {
        this._currentPage(null);
        this.iterator.dispose();
    }
    constructor(pageFactory, finishCallback) {
        super();
        this.pageFactory = pageFactory;
        this.events = new external_DevExpress_Analytics_Utils_namespaceObject.EventManager();
        this._loadingTimeout = null;
        this._currentActivateCount = 0;
        this.isLoading = external_ko_namespaceObject.observable(false);
        this._currentPage = external_ko_namespaceObject.observable();
        this.isVisible = external_ko_namespaceObject.observable(false);
        this._finishCallback = finishCallback;
        this._disposables.push(this.events);
    }
    initialize(state = {}, createIterator = (pageFactory, stateManager) => new PageIterator(pageFactory, stateManager)) {
        this.events.call('beforeInitialize', { wizard: this, state: state });
        this.stateManager = new StateManager(state, this.pageFactory);
        this.iterator = createIterator(this.pageFactory, this.stateManager);
        this.events.call('afterInitialize', { wizard: this });
    }
    isFirstPage() {
        return this._currentPage() && this._currentPage().pageId == this.iterator.getNextPageId();
    }
    canNext() {
        return !this.isLoading() && this._currentPage() && this.pageFactory.getMetadata(this._currentPage().pageId).canNext(this._currentPage().page);
    }
    canFinish() {
        return !this.isLoading() && this._currentPage() && this.pageFactory.getMetadata(this._currentPage().pageId).canFinish(this._currentPage().page);
    }
    _initPage(page) {
        this.events.call('beforePageInitialize', _createBeforeInitializePageEventArgs(page, this));
        this._createLoadingState(page.page);
        this._createNextAction(page.page);
        return page.initialize(this.stateManager.getPageState(page.pageId));
    }
    start() {
        this.events.call('beforeStart', { wizard: this });
        this._loadingState(true);
        const startPage = this.iterator._getStartPage();
        this._initPage(startPage).done(() => {
            this._currentPage(startPage);
            this.events.call('afterPageInitialize', _createPageEventArgs(startPage, this));
        }).always(() => this._loadingState(false)).fail(() => {
            this.isVisible(false);
        });
    }
    canRunWizard() {
        return true;
    }
    nextAction() {
        if (!this.canNext())
            return;
        const currentPage = this.iterator._getCurrentPage();
        this._loadingState(true);
        const revertPreviosPage = () => this.iterator._getPreviousPage().always(() => {
            this.iterator._resetPages();
            this._loadingState(false);
        });
        currentPage.commit().done((result) => {
            if (currentPage.isChanged)
                this.stateManager.setPageState(currentPage.pageId, result);
            this.iterator._getNextPage().done(page => {
                if (page) {
                    this._initPage(page).done(() => {
                        this._currentPage(page);
                        this.events.call('afterPageInitialize', _createPageEventArgs(page, this));
                        this._loadingState(false);
                    }).fail(() => revertPreviosPage());
                }
                else
                    revertPreviosPage();
            }).fail(() => this._loadingState(false));
        }).fail(() => this._loadingState(false));
    }
    previousAction() {
        if (this.isFirstPage())
            return;
        this._loadingState(true);
        this.iterator._getPreviousPage().done((page) => {
            if (page) {
                this._currentPage(page);
            }
        }).always(() => this._loadingState(false));
    }
    goToPage(pageId) {
        this._loadingState(true);
        this.iterator._goToPage(pageId).done((page) => {
            if (page) {
                this._currentPage(page);
            }
        }).always(() => this._loadingState(false));
    }
    finishAction() {
        if (!this.canFinish())
            return;
        this._loadingState(true);
        const currentPage = this.iterator._getCurrentPage();
        currentPage.commit().done((result) => {
            this.stateManager.setPageState(currentPage.pageId, result);
            this.iterator._resetPages();
            if (this._finishCallback) {
                const currentState = this.stateManager.getCurrentState();
                this._callBeforeFinishHandler(currentState);
                this._finishCallback(currentState)
                    .done((result) => {
                    this.onFinish();
                    this._callAfterFinishHandler(currentState, result);
                    this.isVisible(false);
                })
                    .always(() => {
                    this._loadingState(false);
                });
            }
            else {
                this._loadingState(false);
                this.isVisible(false);
            }
        });
    }
}

;// ./dist/query-builder/wizard/popupWizard.js






class PopupWizard extends BaseWizard {
    static _getLoadPanelViewModel(element, observableVisible) {
        const $container = (0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(element).closest('.dxrd-wizard-content');
        return {
            animation: {
                show: { type: 'fade', from: 0, to: 1, duration: 700 },
                hide: { type: 'fade', from: 1, to: 0, duration: 700 }
            },
            deferRendering: false,
            message: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Loading...', 'AnalyticsCoreStringId.Loading'),
            visible: observableVisible,
            shading: true,
            shadingColor: 'transparent',
            position: { of: $container },
            container: $container
        };
    }
    constructor(pageFactory, finishCallback) {
        super(pageFactory, finishCallback);
        this.height = external_ko_namespaceObject.observable(520);
        this.width = external_ko_namespaceObject.observable(810);
        this._extendCssClass = '';
        this._container = external_DevExpress_Analytics_Internal_namespaceObject.getParentContainer;
        this.itemsProvider = external_ko_namespaceObject.observable();
        this.nextButton = {
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Next', 'AnalyticsCoreStringId.Wizard_Next'),
            disabled: external_ko_namespaceObject.computed(() => !this.canNext()),
            onClick: () => this.nextAction()
        };
        this.cancelButton = {
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Cancel', 'AnalyticsCoreStringId.SearchDialog_Cancel'),
            onClick: () => {
                this.iterator.dispose();
                this.isVisible(false);
            }
        };
        this.previousButton = {
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Previous', 'AnalyticsCoreStringId.Wizard_Previous'),
            disabled: external_ko_namespaceObject.computed(() => this.isFirstPage()),
            onClick: () => this.previousAction()
        };
        this.finishButton = {
            text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Finish', 'AnalyticsCoreStringId.Wizard_Finish'),
            type: 'default',
            disabled: external_ko_namespaceObject.computed(() => {
                return !this.canFinish();
            }),
            onClick: () => this.finishAction()
        };
        this._titleTemplate = (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dx-wizard-headerNew');
        this.title = 'Popup Wizard';
    }
    start() {
        super.start();
        this.isVisible(true);
    }
    _wizardPopupPosition(element) {
        return {
            of: (0,external_DevExpress_Analytics_Internal_namespaceObject.getParentContainer)(element)
        };
    }
    _loadPanelViewModel(element) {
        return this._getLoadPanelViewModel(element, this.isLoading);
    }
    _getLoadPanelViewModel(element, observableVisible) {
        return PopupWizard._getLoadPanelViewModel(element, observableVisible);
    }
}

;// ./dist/query-builder/wizard/internal/_dbSchemaTreeListController.js



class DBSchemaTreeListController extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListController {
    constructor(_customizeDBSchemaTreeListActions, searchOptions = new external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListSearchOptions()) {
        super();
        this._customizeDBSchemaTreeListActions = _customizeDBSchemaTreeListActions;
        this.searchOptions = searchOptions;
    }
    getActions(value) {
        if (!value.data)
            return [];
        if (!(value.data instanceof QueriesTreeNode || value.data instanceof TreeQueryNode))
            return [];
        const result = value.data.getActions(value);
        this._customizeDBSchemaTreeListActions && this._customizeDBSchemaTreeListActions(value.data, result);
        return result;
    }
    canSelect(value) {
        return true;
    }
}

;// ./dist/query-builder/wizard/pages/multiQueryWizard/multiQueryConfigurePage.js






























class MultiQueryConfigurePage extends WizardPageBase {
    _addQueryAlgorithm(elements, specifics, dataSource, customQueries) {
        if (!elements.unChecked() || specifics === 'queries') {
            if (elements.children().length === 0) {
                this._itemsProvider().getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(specifics));
            }
            if (specifics === 'tables' || specifics === 'views') {
                this._addQueryFromTables(elements, dataSource);
            }
            else if (specifics === 'procedures') {
                this._addQueryFromStoredProcedures(elements, dataSource);
            }
            else {
                this._addQueryFromCustomQueries(elements, customQueries, dataSource.queries);
            }
        }
        else {
            elements.children().forEach(node => {
                MultiQueryConfigurePage._removeQuery(dataSource.queries, node);
            });
        }
    }
    _addQueryFromTables(elements, dataSource) {
        for (let i = 0; i < elements.children().length; i++) {
            const table = elements.children()[i];
            if (!table.unChecked()) {
                const columns = {};
                const queryJSON = {
                    'Columns': columns,
                    'Tables': {
                        'SelectedTables': {
                            'Item1': {
                                '@Name': table.name,
                                '@ControlType': 'Table',
                                '@ItemType': 'Table'
                            }
                        },
                    }
                };
                if (table.children().length === 0) {
                    this._itemsProvider().getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest('tables.' + table.name)).done(() => {
                        table.initializeChildren(table.children());
                    });
                }
                for (let j = 0; j < table.children().length; j++) {
                    const column = table.children()[j];
                    if (column.checked()) {
                        columns['Item' + (j + 1)] = {
                            '@Table': table.name,
                            '@Name': column.name,
                            '@ItemType': 'Column'
                        };
                    }
                }
                MultiQueryConfigurePage._pushQuery(new TableQuery(queryJSON, dataSource), table, dataSource.queries);
            }
            else {
                MultiQueryConfigurePage._removeQuery(dataSource.queries, table);
            }
        }
    }
    _addQueryFromStoredProcedures(elements, dataSource) {
        for (let i = 0; i < elements.children().length; i++) {
            const procedure = elements.children()[i];
            if (procedure.checked()) {
                const newQuery = new StoredProcQuery({ '@Name': procedure.name, 'ProcName': procedure.name }, dataSource);
                procedure.arguments.forEach((arg) => {
                    newQuery.parameters.push(new DataSourceParameter({ '@Name': arg.name, '@Type': DBColumn.GetType(arg.type) }, null, storedProcParameterSerializationsInfo(DBColumn.GetType(arg.type))));
                });
                MultiQueryConfigurePage._pushQuery(newQuery, procedure, dataSource.queries);
            }
            else {
                MultiQueryConfigurePage._removeQuery(dataSource.queries, procedure);
            }
        }
    }
    _addQueryFromCustomQueries(elements, queries, allQueries) {
        for (let i = 0; i < elements.children().length; i++) {
            const queryNode = elements.children()[i];
            const query = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(queries.peek(), item => queryNode.name === (item.name() || item.generateName()));
            if (queryNode.checked()) {
                query.name(generateQueryUniqueName(allQueries.peek(), query));
                this._checkedQueries.push(query);
            }
        }
    }
    _getItemsPromise(pathRequest) {
        return this._itemsProvider().getItems(pathRequest);
    }
    _resetDataSourceResult() {
        this._customResetOptions();
        this._dataSource().relations([]);
        this._dataSource().resultSet = null;
        this._onChange();
    }
    _setQueryCore(query) {
        const provider = this._fieldListModel().itemsProvider;
        const queryEditIndex = this._queryEditIndex();
        if (queryEditIndex >= 0) {
            this._itemsProvider().queries().children()[queryEditIndex].name = query.name();
            provider.customQueries().splice(queryEditIndex, 1, query);
        }
        else {
            query.name(generateQueryUniqueName(provider.customQueries().peek(), query));
            provider.customQueries().push(query);
            this._selectedPath('queries.' + query.name());
            const childrens = this._itemsProvider().queries().children();
            const children = childrens[childrens.length - 1];
            children.setChecked(true);
            children['_afterCheckToggled'] && children['_afterCheckToggled'](children);
        }
        this._resetDataSourceResult();
    }
    static _pushQuery(newQuery, node, queries) {
        if (!(0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(queries.peek(), item => item.name() === (newQuery.name() || newQuery.generateName()))) {
            newQuery.name(generateQueryUniqueName(queries.peek(), newQuery));
            queries.push(newQuery);
        }
        node.hasQuery = true;
    }
    static _removeQuery(queries, node) {
        if (node.hasQuery) {
            let queryIndex = -1;
            const existUncheck = queries.peek().some((value, index) => {
                if (value.name() === node.name || value.generateName() === node.name) {
                    queryIndex = index;
                    return true;
                }
                return false;
            });
            if (existUncheck) {
                queries.splice(queryIndex, 1);
            }
            node.hasQuery = false;
        }
    }
    constructor(_options) {
        super();
        this._options = _options;
        this._selectedPath = external_ko_namespaceObject.observable(null);
        this._itemsProvider = external_ko_namespaceObject.observable();
        this._customQueries = external_ko_namespaceObject.observableArray([]);
        this._checkedQueries = external_ko_namespaceObject.observableArray([]);
        this._sqlDataSourceWrapper = new _SqlDataSourceWrapper(undefined, undefined, undefined);
        this._dataSource = () => {
            return this._sqlDataSourceWrapper && this._sqlDataSourceWrapper.sqlDataSource;
        };
        this._dataConnection = () => {
            return this._dataSource() && this._dataSource().connection;
        };
        this._showStatementPopup = (query) => {
            this._popupSelectStatement.isVisible(true);
            this._popupSelectStatement.query = query;
            this._popupSelectStatement.data(query.sqlString());
        };
        this._showQbCallBack = (name = null, isCustomQuery = false) => {
            if (name !== null) {
                const query = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._customQueries(), item => name === (item.name() || item.generateName()));
                this._queryEditIndex(this._customQueries().indexOf(query));
                if (query.type() === SqlQueryType.customSqlQuery) {
                    this._showStatementPopup(query);
                }
                else {
                    this._popupQueryBuilder.show(query, this._dataSource());
                }
            }
            else {
                this._queryEditIndex(-1);
                if (isCustomQuery) {
                    this._showStatementPopup(new CustomSqlQuery({ '@Name': null }, this._dataSource()));
                }
                else {
                    const queryNew = new TableQuery({ '@Name': null }, this._dataSource());
                    this._popupQueryBuilder.show(queryNew, this._dataSource());
                }
            }
        };
        this._popupSelectStatement = ({
            isVisible: external_ko_namespaceObject.observable(false),
            title: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Custom SQL Editor', 'AnalyticsCoreStringId.SqlDSWizard_CustomSqlEditor'),
            query: null,
            data: external_ko_namespaceObject.observable(),
            okButtonText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('OK', 'DataAccessUIStringId.Button_OK'),
            okButtonHandler: (e) => {
                this._popupSelectStatement.query.sqlString(e.model.data());
                this._setCustomSqlQuery(this._popupSelectStatement.query);
                e.model.isVisible(false);
            },
            aceOptions: createDefaultSQLAceOptions(),
            aceAvailable: (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.aceAvailable)(),
            additionalOptions: createDefaultSQLAdditionalOptions((newVal) => { this._popupSelectStatement.data(newVal); }),
            languageHelper: createDefaultSQLLanguageHelper(),
            closest(element, parentSelector) {
                return (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).closest(parentSelector)[0];
            }
        });
        this._customResetOptions = () => void (0);
        this._queryEditIndex = external_ko_namespaceObject.observable(-1);
        this.disableCustomSql = true;
        this._scrollViewHeight = 'calc(100% - 37px)';
        this._customizeDBSchemaTreeListActions = null;
        this._isDataLoadingInProcess = external_ko_namespaceObject.observable(false);
        this._callbacks = this._options.callbacks;
        this._sqlTextProvider = new SelectQuerySqlTextProvider(wrapGetSelectStatement(this._callbacks.selectStatement), this._dataConnection);
        this._popupQueryBuilder = new QueryBuilderPopup((newQuery, isInProcess) => this._setTableQuery(newQuery, isInProcess), _options.rtl, _options.callbacks.customizeQBInitData);
        this._fieldListModel = external_ko_namespaceObject.observable(null);
        this._disposables.push(this._hasParametersToEdit = external_ko_namespaceObject.pureComputed(() => this._itemsProvider().hasParametersToEdit()));
        this._disposables.push(this._sqlDataSourceWrapper);
        this.disableCustomSql = this._options.disableCustomSql;
        this._getItemsAfterCheck = (node) => {
            this._resetDataSourceResult();
            if (node.checked.peek() && node.isList) {
                if (node.name === 'tables' || node.name === 'views') {
                    this._itemsProvider().getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(node.name)).done(() => {
                        if (node.isList && node.children.peek().length > 0) {
                            external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when(node.children.peek().map(item => this._getItemsPromise(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(node.name + '.' + item.name))));
                        }
                    }).always(() => this._onChange());
                }
                else if (node.specifics === 'table' || node.specifics === 'view') {
                    this['_itemsProvider']().getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(node.specifics + '.' + node.name)).always(() => this._onChange());
                }
                else if (node.name === 'procedures') {
                    this['_itemsProvider']().getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(node.name)).always(() => this._onChange());
                }
            }
            else {
                this._onChange();
            }
        };
    }
    canNext() {
        return !this._itemsProvider().nextButtonDisabled() && this.canFinish();
    }
    canFinish() {
        return this._itemsProvider() && this._itemsProvider().hasCheckedItems() && !this._isDataLoadingInProcess();
    }
    _AddQueryWithBuilder() {
    }
    _runQueryBuilder() {
    }
    _loadPanelViewModel(element) {
        return PopupWizard._getLoadPanelViewModel(element, this._isDataLoadingInProcess);
    }
    _setTableQuery(query, isInProcess) {
        isInProcess && isInProcess(true);
        return this._sqlTextProvider.getQuerySqlText(query)
            .done(() => this._setQueryCore(query))
            .always(() => {
            isInProcess && isInProcess(false);
        });
    }
    _setCustomSqlQuery(query) {
        this._setQueryCore(query);
    }
    _createTreeListFactory() {
        return new MultiQueryTreeListItemFactory();
    }
    commit() {
        this._dataSource().queries.removeAll();
        this._checkedQueries.removeAll();
        this._addQueryAlgorithm(this._itemsProvider().tables(), 'tables', this._dataSource());
        this._addQueryAlgorithm(this._itemsProvider().views(), 'views', this._dataSource());
        this._addQueryAlgorithm(this._itemsProvider().procedures(), 'procedures', this._dataSource());
        this._addQueryAlgorithm(this._itemsProvider().queries(), 'queries', this._dataSource(), this._customQueries);
        external_ko_namespaceObject.utils.arrayPushAll(this._dataSource().queries(), this._checkedQueries());
        this._dataSource().queries.valueHasMutated();
        const checkedCustomQueries = this._itemsProvider().queries().children().filter((x) => x.checked()).map(query => query.name);
        const customQueries = this._sqlDataSourceWrapper.sqlDataSource.queries().filter(x => checkedCustomQueries.some(queryName => queryName === x.name()) || x instanceof StoredProcQuery);
        customQueries.forEach((item) => this._sqlDataSourceWrapper.sqlDataSource.queries.remove(item));
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            sqlDataSourceJSON: this._sqlDataSourceWrapper.save(),
            customQueries: customQueries.map(x => JSON.stringify(serializer.serialize(x)))
        }).promise();
    }
    initialize(state) {
        this._sqlDataSourceWrapper = _restoreSqlDataSourceFromState(state, this._options.requestWrapper);
        const customQueriesPromise = this._callbacks.customQueriesPreset
            ? this._callbacks.customQueriesPreset(this._dataSource())
            : new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve([]).promise();
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        customQueriesPromise.done((queries) => {
            this._customQueries(queries);
            this._selectedPath('');
            const itemsProvider = new DBSchemaItemsProvider(this._dataSource().dbSchemaProvider, this._customQueries, this._showQbCallBack, this.disableCustomSql, this._getItemsAfterCheck);
            this._disposables.push(itemsProvider);
            this._itemsProvider(itemsProvider);
            this._disposables.push(itemsProvider.itemsRequestDeferreds.subscribe((itemsLoadingDeferreds) => this._isDataLoadingInProcess(itemsLoadingDeferreds.length > 0)));
            this._getItemsPromise(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest('queries'));
            const searchOptions = new external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListSearchOptions();
            searchOptions.autoLoadItems = AutoQueryPreload();
            this._fieldListModel({
                factory: this._createTreeListFactory(),
                itemsProvider: this._itemsProvider(),
                loadChildItemsForCollapsedNodes: AutoQueryPreload(),
                selectedPath: this._selectedPath,
                treeListController: new DBSchemaTreeListController(this._customizeDBSchemaTreeListActions, searchOptions),
                templateName: 'dxrd-treelist-with-checkbox'
            });
            this._popupQueryBuilder.isVisible(false);
            deferred.resolve();
        })
            .fail(deferred.reject);
        return deferred.promise();
    }
}
function _registerMultiQueryConfigurePage(factory, wizardOptions) {
    factory.registerMetadata(SqlDataSourceWizardPageId.MultiQueryConfigurePage, {
        create: () => {
            return new MultiQueryConfigurePage(wizardOptions);
        },
        setState: (data, state) => {
            state.sqlDataSourceJSON = data.sqlDataSourceJSON;
            state.customQueries = data.customQueries;
        },
        getState: (state) => {
            return state.sqlDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.sqlDataSourceJSON = defaultState.sqlDataSourceJSON;
            state.customQueries = defaultState.customQueries;
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Columns selected from specific tables and/or views will be automatically included into a separate query.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMultiQuery'),
        template: 'dxrd-wizard-add-queries-page'
    });
}

;// ./dist/query-builder/wizard/internal/_configureParametersUtils.js







class ParametersTreeListItemBase extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(parameter) {
        super();
        this.editor = external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('commonCollection');
        this.isList = false;
        this.contenttemplate = 'dx-treelist-accordion-contenttemplate-custom-with-actions';
        this.actionsTemplate = 'dx-treelist-item-actions';
        this.dataSourceParameter = external_ko_namespaceObject.observable(parameter);
        this._name = parameter.name;
        this._displayName = parameter.displayName;
    }
    get name() {
        return this._name();
    }
    get displayName() {
        return this._displayName || this.name;
    }
}
class ParametersTreeListItem extends ParametersTreeListItemBase {
    constructor(parameter, parent) {
        super(parameter);
        this.parent = parent;
    }
    query() {
        return this.parent.query();
    }
}
class ParametersTreeListRootItemBase {
    constructor(name) {
        this.isList = true;
        this.specifics = 'List';
        this.visible = external_ko_namespaceObject.observable(true);
        this.name = this.displayName = name;
        this.parameters = external_ko_namespaceObject.observableArray([]);
    }
    removeChild(parameter) {
        this.parameters.remove(parameter);
    }
}
class ParametersTreeListRootItem extends ParametersTreeListRootItemBase {
    constructor(query) {
        super(query.name());
        this._query = query;
    }
    query() {
        return this._query;
    }
}
class ParametersTreeListController extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListController {
    constructor(rootItems, createNewParameter) {
        super();
        this._createNewParameter = createNewParameter;
        this._rootItems = rootItems;
    }
    hasItems(item) {
        return item.isList;
    }
    getActions(treeListItem) {
        const actions = [];
        if (!treeListItem.data)
            return actions;
        if (treeListItem.data.query().type() === SqlQueryType.storedProcQuery) {
            return treeListItem.data.isList ? [] : [external_DevExpress_Analytics_Widgets_Internal_namespaceObject.treeListEditAction];
        }
        if (treeListItem.data.isList) {
            const item = treeListItem.data;
            actions.push({
                clickAction: () => {
                    return item.parameters.push(new ParametersTreeListItem(this._createNewParameter(item.name, item.parameters()), item));
                },
                imageClassName: 'dxrd-image-add',
                imageTemplateName: 'dxrd-svg-operations-add',
                text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Add parameter', 'AnalyticsCoreStringId.FieldListActions_AddParameter')
            });
        }
        else {
            const parameter = treeListItem.data;
            actions.push({
                clickAction: () => {
                    parameter.parent.removeChild(parameter);
                },
                imageClassName: 'dxrd-image-recycle-bin',
                imageTemplateName: 'dxrd-svg-operations-recycle_bin',
                text: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Remove parameter', 'DataAccessUIStringId.Button_Remove'),
            });
            actions.push(external_DevExpress_Analytics_Widgets_Internal_namespaceObject.treeListEditAction);
        }
        return actions;
    }
    canSelect(value) {
        return true;
    }
}

;// ./dist/query-builder/wizard/pages/multiQueryWizard/multiQueryConfigureParametersPage.js
















function _canEditQueryParameters(query, customQueries) {
    if (query.type() === SqlQueryType.tableQuery || query.type() === SqlQueryType.customSqlQuery) {
        return customQueries.indexOf(query) > -1;
    }
    return query.type() === SqlQueryType.storedProcQuery && query.parameters().length > 0;
}
class MultiQueryConfigureParametersPage extends WizardPageBase {
    _isParametersValid() {
        return this._getParameters().every(x => x.isValid());
    }
    canNext() {
        return this._isParametersValid() && this._sqlDataSourceWrapper.sqlDataSource.queries().length > 1;
    }
    canFinish() {
        return this._isParametersValid() && this._sqlDataSourceWrapper.sqlDataSource.queries().length >= 1;
    }
    constructor(parametersConverter = {
        createParameterViewModel: parameter => parameter,
        getParameterFromViewModel: parameterViewModel => parameterViewModel
    }, _requestWrapper) {
        super();
        this.parametersConverter = parametersConverter;
        this._requestWrapper = _requestWrapper;
        this._sqlDataSourceWrapper = new _SqlDataSourceWrapper(undefined, undefined, this._requestWrapper);
        this._selectedPath = external_ko_namespaceObject.observable(null);
        this._rootItems = external_ko_namespaceObject.observableArray();
        this._createNewParameter = (queryName, parameters) => {
            const newParameter = new DataSourceParameter({
                '@Name': (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueNameForNamedObjectsArray)(parameters, 'parameter'),
                '@Type': external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32
            });
            this._selectedPath(queryName + '.' + newParameter.name());
            return this.parametersConverter.createParameterViewModel(newParameter);
        };
        this._scrollViewHeight = '100%';
        this._fieldListModel = external_ko_namespaceObject.observable(null);
        this._removeButtonTitle = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Remove', 'DataAccessUIStringId.Button_Remove');
        const callback = () => this._onChange();
        this._disposables.push(subscribeArray(this._rootItems, (item) => {
            this._disposables.push(subscribeArray(item.parameters, (parameter) => {
                this._disposables.push(subscribeObject(parameter.dataSourceParameter, (value) => {
                    this._disposables.push(...subscribeProperties([value.name, value['value'], value['type']], callback));
                }, callback));
            }, callback));
        }, callback));
        this._parametersEditorOptions = {
            addHandler: () => {
                return this.parametersConverter.createParameterViewModel(new DataSourceParameter({
                    '@Name': (0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueNameForNamedObjectsArray)(this._parametersEditorOptions.values.peek().peek(), 'parameter'),
                    '@Type': external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemInt32
                }));
            },
            values: external_ko_namespaceObject.observable(external_ko_namespaceObject.observableArray([])),
            displayName: 'Parameters',
            level: 0,
            info: external_ko_namespaceObject.observable({
                displayName: 'Parameters', localizationId: 'DevExpress.DataAccess.Sql.SqlQuery.Parameters',
                propertyName: 'parameters',
                modelName: 'Parameter',
                array: true,
                editor: external_DevExpress_Analytics_Widgets_namespaceObject.editorTemplates.getEditor('commonCollection'),
                template: '#dxrd-parameter-collection-item'
            }),
            editorTemplate: '#dxrd-wizard-datasource-parameters',
            hideButtons: external_ko_namespaceObject.observable(false),
            collapsed: false
        };
    }
    _getParameters() {
        return [].concat.apply([], (this._rootItems() || []).map((x) => {
            return x.parameters().map((param) => {
                return param.dataSourceParameter();
            });
        }));
    }
    initialize(state) {
        const newRootItemsWithParameters = [];
        const rootItems = this._rootItems();
        this._sqlDataSourceWrapper = _restoreSqlDataSourceFromState(state, this._requestWrapper);
        this._sqlDataSourceWrapper.sqlDataSource.queries().forEach(query => {
            if (_canEditQueryParameters(query, this._sqlDataSourceWrapper.customQueries)) {
                const parent = new ParametersTreeListRootItem(query);
                parent.parameters(query.parameters().map(parameterModel => {
                    return new ParametersTreeListItem(this.parametersConverter.createParameterViewModel(parameterModel), parent);
                }));
                newRootItemsWithParameters.push(parent);
            }
        });
        rootItems.filter(x => newRootItemsWithParameters.every(newItem => newItem.name !== x.name)).forEach((removedItem) => {
            rootItems.splice(this._rootItems().indexOf(removedItem), 1);
        });
        newRootItemsWithParameters.forEach((newItem) => {
            const currentItem = rootItems.filter(x => x.name === newItem.name)[0];
            if (currentItem) {
                newItem.parameters().filter(newParam => currentItem.parameters().every(x => x.name !== newParam.name)).forEach((param) => {
                    currentItem.parameters.push(param);
                });
            }
            else {
                rootItems.push(newItem);
            }
        });
        this._rootItems.valueHasMutated();
        this._fieldListModel({
            itemsProvider: {
                getItems: pathRequest => {
                    const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
                    if (!pathRequest.fullPath) {
                        result.resolve(this._rootItems());
                    }
                    else {
                        const parent = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._rootItems(), item => item.name === pathRequest.ref);
                        result.resolve(parent.parameters());
                    }
                    return result.promise();
                }
            },
            templateName: 'dx-treelist-item-with-hover',
            selectedPath: this._selectedPath,
            treeListController: new ParametersTreeListController(this._rootItems(), this._createNewParameter),
        });
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    commit() {
        this._rootItems().forEach(item => {
            const customQuery = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._sqlDataSourceWrapper.customQueries, (query) => query.name() === item.query().name());
            customQuery.parameters(item.parameters().map(parameterViewModel => {
                return this.parametersConverter.getParameterFromViewModel(parameterViewModel.dataSourceParameter());
            }));
        });
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            sqlDataSourceJSON: this._sqlDataSourceWrapper.sqlDataSourceJSON,
            customQueries: this._sqlDataSourceWrapper.saveCustomQueries()
        }).promise();
    }
}
function _registerMultiQueryConfigureParametersPage(factory, requestWrapper, parametersConverter) {
    factory.registerMetadata(SqlDataSourceWizardPageId.MultiQueryConfigureParametersPage, {
        create: () => new MultiQueryConfigureParametersPage(parametersConverter, requestWrapper),
        getState: (state) => state.sqlDataSourceWizard,
        setState: (result, state) => state.customQueries = result.customQueries,
        resetState: () => void 0,
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure query parameters.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureParameters'),
        template: 'dxrd-configure-query-parameters-page'
    });
}

;// ./dist/query-builder/widgets/masterdetaileditor/_masterDetailEditorPopupManager.js
class MasterDetailEditorPopupManager {
    _updateActions(viewModel) {
        this._popupService['subscription'] = this._popupService.visible.subscribe((newVal) => {
            this.target.isSelected && this.target.isSelected(newVal);
        });
        this._popupService.data({
            data: this._popupItems,
            template: 'dx-filtereditor-popup-common',
            click: (data) => {
                viewModel[this._action](data);
                this._popupService.visible(false);
            }
        });
    }
    constructor(target, popupService, action, popupItems) {
        this.showPopup = (_, element) => {
            if (this._popupService['subscription']) {
                this._popupService['subscription'].dispose();
            }
            this._popupService.title('');
            this._updateActions(this.target);
            this._popupService.target(element);
            this._popupService.visible(true);
        };
        this.target = target;
        this._action = action;
        this._popupService = popupService;
        this._popupItems = popupItems;
    }
}

;// ./dist/query-builder/widgets/masterdetaileditor/_keyColumnSurface.js


class KeyColumnSurface {
    constructor(column, queryName, _isMaster = false) {
        this._isMaster = _isMaster;
        this.getTitle = () => this._isMaster ? (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Master Query', 'AnalyticsCoreStringId.Wizard_MasterDetailRelationship_MasterQuery') : (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Detail Query', 'AnalyticsCoreStringId.Wizard_MasterDetailRelationship_DetailQuery');
        this.isSelected = external_ko_namespaceObject.observable(false);
        this.selectColumnText = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('<Select a Column>', 'DataAccessUIStringId.JoinEditorEmptyColumnText');
        this.column = column;
        this.queryName = queryName;
        this._setColumn = (resultColumn) => {
            this.column(external_ko_namespaceObject.unwrap(resultColumn.name));
        };
    }
}

;// ./dist/query-builder/widgets/masterdetaileditor/_masterDetailRelationSurface.js


class MasterDetailRelationSurface {
    constructor(relation, parent) {
        this.isEditable = external_ko_namespaceObject.observable(false);
        this.relationName = relation.name;
        this.keyColumns = external_ko_namespaceObject.pureComputed(() => {
            return relation.keyColumns().map((item) => {
                return {
                    master: new KeyColumnSurface(item.masterColumn, relation.masterQuery(), true),
                    detail: new KeyColumnSurface(item.detailColumn, relation.detailQuery(), false)
                };
            });
        });
        this.create = () => {
            relation.createKeyColumn();
        };
        this.remove = (data) => {
            relation.keyColumns.remove(item => item.masterColumn === data.master.column && item.detailColumn === data.detail.column);
            if (relation.keyColumns().length === 0)
                parent.remove(this);
        };
    }
}

;// ./dist/query-builder/widgets/masterdetaileditor/_masterQuerySurface.js





class MasterQuerySurface {
    constructor(masterQueryName, relations) {
        this.relations = external_ko_namespaceObject.observableArray();
        this.queryName = masterQueryName;
        this.add = (relation) => {
            this.relations.push(new MasterDetailRelationSurface(relation, this));
        };
        this.create = (detailQueryItem) => {
            const newRelation = new MasterDetailRelation({ '@Master': this.queryName, '@Detail': detailQueryItem.name });
            if ((0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.relations(), 'relationName', newRelation.name())) {
                newRelation.name((0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueName)(this.relations().map(item => item.relationName()), newRelation.name() + '_'));
            }
            newRelation.createKeyColumn();
            this.add(newRelation);
            relations.push(newRelation);
        };
        this.remove = (relationSurface) => {
            this.relations.remove(relationSurface);
            relations.remove(item => item.name === relationSurface.relationName);
        };
    }
}

;// ./dist/query-builder/widgets/masterdetaileditor/_masterDetailEditor.js









class MasterDetailEditor extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.PopupEditorBase {
    constructor(relations, resultSet, saveCallBack) {
        super();
        this.isValid = external_ko_namespaceObject.observable(true);
        this.loadPanelVisible = external_ko_namespaceObject.observable(false);
        this.masterQueries = external_ko_namespaceObject.observableArray();
        this.popupService = new external_DevExpress_Analytics_Internal_namespaceObject.PopupService();
        this.save = () => {
            const emptyFieldsExist = relations().some((relation) => {
                return !relation.detailQuery() || !relation.masterQuery() ||
                    relation.keyColumns().some(column => (!column.detailColumn() || !column.masterColumn()));
            });
            if (emptyFieldsExist) {
                (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Some fields are empty. Please fill all empty fields or remove the corresponding conditions to proceed.', 'DataAccessUIStringId.JoinEditorFillAllFieldsException'));
            }
            else {
                saveCallBack().done(() => { this.popupVisible(false); });
            }
        };
        this.createRelation = (target) => {
            const popupItems = resultSet.tables()
                .filter(table => table.tableName() !== target.queryName)
                .map(table => { return { name: table.tableName() }; });
            return {
                data: new MasterDetailEditorPopupManager(target, this.popupService, 'create', popupItems),
                templateName: 'dx-filtereditor-create'
            };
        };
        this.setColumn = (target) => {
            const table = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(resultSet.tables(), 'tableName', target.queryName);
            return {
                data: new MasterDetailEditorPopupManager(target, this.popupService, '_setColumn', table ? table.columns() : []),
                templateName: 'dx-masterdetail-editor-setColumn'
            };
        };
        this._createMainPopupButtons();
        const masterQueries = {};
        resultSet.tables().forEach((table) => {
            masterQueries[table.tableName()] = new MasterQuerySurface(table.tableName(), relations);
        });
        relations().forEach((relation) => {
            masterQueries[relation.masterQuery()] = masterQueries[relation.masterQuery()] || new MasterQuerySurface(relation.masterQuery(), relations);
            masterQueries[relation.masterQuery()].add(relation);
        });
        this.masterQueries(Object.keys(masterQueries).map(key => masterQueries[key]));
    }
    title() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Master-Detail Relation Editor', external_DevExpress_Analytics_Internal_namespaceObject.StringId.MasterDetailRelationsEditor);
    }
}

;// ./dist/query-builder/wizard/pages/multiQueryWizard/masterDetailRelationshipsPageBase.js







class MasterDetailRelationshipsPageBase extends WizardPageBase {
    _getResultSet(dataSource) {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        if (dataSource.resultSet) {
            deferred.resolve((dataSource.resultSet));
        }
        else {
            this._getResultSchema(dataSource).done(((result) => {
                deferred.resolve(new ResultSet(JSON.parse(result.resultSchemaJSON)));
            })).fail(result => {
                deferred.reject(result);
            });
        }
        return deferred.promise();
    }
    _dataSource() {
        return null;
    }
    _restoreDataSource(state) {
    }
    _updateRelations() {
        const relations = this._relations();
        relations.forEach((relation, index) => {
            const detailTable = this._resultSet.tables().filter(table => table.tableName() === relation.detailQuery())[0];
            const masterTable = this._resultSet.tables().filter(table => table.tableName() === relation.masterQuery())[0];
            if (!detailTable || !masterTable) {
                relations.splice(index, 1);
                return;
            }
            const keyColumns = relation.keyColumns();
            keyColumns.forEach((keyColumn, index) => {
                if (detailTable.columns().every(x => x.name() !== keyColumn.detailColumn()) ||
                    masterTable.columns().every(x => x.name() !== keyColumn.masterColumn()))
                    keyColumns.splice(index, 1);
            });
            if (keyColumns.length === 0)
                relations.splice(index, 1);
        });
        this._relations.valueHasMutated();
    }
    constructor(_getResultSchema) {
        super();
        this._getResultSchema = _getResultSchema;
        this._relations = external_ko_namespaceObject.observableArray([]);
        this._customResetOptions = () => void (0);
        this._relationsEditor = external_ko_namespaceObject.observable(null);
        const callback = () => this._onChange();
        this._disposables.push(subscribeArray(this._relations, (relation) => {
            relation._disposables.push(...subscribeProperties([relation.detailQuery, relation.name, relation.masterQuery], callback));
            relation._disposables.push(subscribeArray(relation.keyColumns, (column) => {
                relation._disposables.push(...subscribeProperties([column.detailColumn, column.masterColumn], callback));
            }, callback));
        }, callback));
    }
    canNext() {
        return false;
    }
    canFinish() {
        return this._relations().every(relation => relation.keyColumns()
            .every(keyColumn => !!keyColumn.detailColumn() && !!keyColumn.masterColumn()));
    }
    initialize(state) {
        this.relationsSubscription && this.relationsSubscription.dispose();
        this._restoreDataSource(state);
        this._disposables.push(this.relationsSubscription = this._relations.subscribe((changes) => {
            const isRelationsChanged = changes.some(change => {
                return !change['moved'] && change['moved'] !== 0;
            });
            if (isRelationsChanged) {
                this._customResetOptions();
            }
        }, null, 'arrayChange'));
        return this._getResultSet(this._dataSource())
            .done((result) => {
            this._resultSet = result;
            this._updateRelations();
            this._relationsEditor(new MasterDetailEditor(this._relations, this._resultSet, () => void (0)));
        })
            .fail(result => {
            if ((0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(result))
                (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(result));
        });
    }
    dispose() {
        super.dispose();
        this.disposeObservableArray(this._relations);
    }
}

;// ./dist/query-builder/wizard/pages/multiQueryWizard/configureMasterDetailRelationshipsPage.js







class ConfigureMasterDetailRelationshipsPage extends MasterDetailRelationshipsPageBase {
    constructor() {
        super(...arguments);
        this._customResetOptions = () => void (0);
        this._relationsEditor = external_ko_namespaceObject.observable(null);
    }
    _restoreDataSource(state) {
        this._sqlDataSourceWrapper = _restoreSqlDataSourceFromState(state);
    }
    _dataSource() {
        return this._sqlDataSourceWrapper.sqlDataSource;
    }
    commit() {
        this.relationsSubscription.dispose();
        this._sqlDataSourceWrapper.sqlDataSource.relations(this._relations());
        this._sqlDataSourceWrapper.sqlDataSource.resultSet = this._resultSet;
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            sqlDataSourceJSON: this._sqlDataSourceWrapper.sqlDataSourceJSON,
            customQueries: this._sqlDataSourceWrapper.saveCustomQueries(),
            relations: this._relations().map(x => JSON.stringify(serializer.serialize(x))),
        }).promise();
    }
}
function _registerConfigureMasterDetailRelationshipsPage(factory, sqlDataSourceResultSchema) {
    factory.registerMetadata(DataSourceWizardPageId.ConfigureMasterDetailRelationshipsPage, {
        create: () => {
            return new ConfigureMasterDetailRelationshipsPage(sqlDataSourceResultSchema);
        },
        setState: (data, state) => {
            state.relations = data.relations;
        },
        getState: (state) => {
            return state.sqlDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.relations = defaultState.relations;
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure master-detail relationships.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMasterDetailRelations'),
        template: 'dxrd-wizard-configure-relations-page'
    });
}

;// ./dist/query-builder/elements/allColumnsModelMeta.js

const allColumnsSerializationInfo = [query_builder_metadata_name, selected];

;// ./dist/query-builder/elements/allColumnsModel.js







class AllColumnsViewModel extends QueryElementBaseViewModel {
    constructor(parent, serializer) {
        super({ '@ControlType': 'Column' }, parent, serializer);
        const query = parent.parentModel();
        const targetColumn = external_ko_namespaceObject.pureComputed(() => {
            return (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(query.columns(), item => {
                return parent.actualName() === item.table() && ColumnType.AllColumns === item.itemType();
            });
        });
        this.selected = external_ko_namespaceObject.pureComputed({
            read: () => !!targetColumn(),
            write: (value) => {
                if (!!targetColumn() === value)
                    return;
                if (value) {
                    query.columns.push(new ColumnExpression({ '@ItemType': ColumnType.AllColumns, '@Table': parent.actualName() }, query, serializer));
                }
                else {
                    query.columns.remove(item => parent.actualName() === item.table() && ColumnType.AllColumns === item.itemType());
                }
            }
        });
        this.name = external_ko_namespaceObject.pureComputed(AllColumnsViewModel.DisplayName);
        this._disposables.push(this.name);
    }
    getInfo() {
        return allColumnsSerializationInfo;
    }
}
AllColumnsViewModel.DisplayName = () => {
    const name = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('(All Columns)', 'DataAccessStringId.QueryBuilder_AllColumns');
    return name.charAt(0) === '*' ? name : '* ' + name;
};

;// ./dist/query-builder/elements/parameterModelMeta.js
const ParametersMode = {
    ReadWrite: 'ReadWrite',
    Read: 'Read',
    Disabled: 'Disabled'
};

;// ./dist/query-builder/widgets/filterEditor/_queryBuilderObjectsProvider.js




function isAggregatedExpression(object) {
    return object.aggregate() !== AggregationType.None;
}
class QueryBuilderObjectsProvider {
    constructor(query, objectFilter) {
        this.getItems = (pathRequest) => {
            const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            let items = [];
            if (pathRequest.fullPath === '') {
                items = objectFilter.filterTables(query().tables())
                    .map(QueryBuilderObjectsProvider._createTableInfo);
            }
            else {
                const table = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(query().tables(), table => table.actualName() === pathRequest.fullPath);
                items = objectFilter.filterColumns(table && table.columns() || [])
                    .map(column => QueryBuilderObjectsProvider._createColumnInfo(column, objectFilter));
            }
            result.resolve(items);
            return result.promise();
        };
        this.hasParameter = (name) => {
            return query().parameters().filter((parameter) => { return parameter.name() === name; }).length > 0;
        };
        this.getColumnInfo = (propertyName) => {
            if (propertyName) {
                const table = (0,external_DevExpress_Analytics_Internal_namespaceObject.find)(query().tables(), t => propertyName.indexOf(t.actualName() + '.') === 0);
                if (table) {
                    const column = (0,external_DevExpress_Analytics_Internal_namespaceObject.find)(objectFilter.filterColumns(table.columns() || []), c => propertyName === (table.actualName() + '.' + objectFilter.getColumnName(c)));
                    return column ? QueryBuilderObjectsProvider._createColumnInfo(column, objectFilter) : null;
                }
            }
            return null;
        };
    }
    static _createTableInfo(table) {
        return {
            displayName: table.actualName(),
            name: table.actualName(),
            isList: true,
            specifics: 'Default',
            collapsed: external_ko_namespaceObject.observable(true)
        };
    }
    static _createColumnInfo(column, objectFilter) {
        return {
            displayName: objectFilter.getColumnName(column),
            isList: false,
            specifics: objectFilter.getSpecifics(column),
            dataType: objectFilter.getDataType(column),
            name: objectFilter.getColumnName(column)
        };
    }
}
QueryBuilderObjectsProvider.whereClauseObjectsFilter = {
    filterColumns: columns => columns,
    filterTables: tables => tables,
    getColumnName: column => column.name.peek(),
    getSpecifics: column => column.specifics,
    getDataType: column => column.dataType.peek()
};
QueryBuilderObjectsProvider.groupByObjectsFilter = {
    filterColumns: columns => columns.filter(isAggregatedExpression),
    filterTables: tables => tables.filter(table => table.columns().some(isAggregatedExpression)),
    getColumnName: column => column.actualName.peek(),
    getSpecifics: (column) => {
        switch (column.aggregate()) {
            case AggregationType.Avg:
            case AggregationType.AvgDistinct:
                return 'Float';
            case AggregationType.Count:
            case AggregationType.CountDistinct:
                return 'Integer';
            default:
                return column.specifics;
        }
    },
    getDataType: column => null
};

;// ./dist/query-builder/widgets/filterEditor/_groupFilterEditorSerializer.js






class GroupFilterEditorSerializer extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.FilterEditorSerializer {
    _columnDisplayName(column) {
        return column.table() + '.' + column.actualName();
    }
    _findAggregatedColumn(predicate) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.find)(this._columns(), c => isAggregatedExpression(c) && predicate(c));
    }
    constructor(_columns) {
        super();
        this._columns = _columns;
        this._aggregatePropertyName = (operand) => {
            const itemColumnAggregate = this._findAggregatedColumn(c => c.actualName() === operand.propertyName);
            return itemColumnAggregate ? this._columnDisplayName(itemColumnAggregate) : operand.propertyName;
        };
    }
    serializeOperandProperty(operand) {
        const column = this._findAggregatedColumn(c => operand.propertyName === this._columnDisplayName(c));
        return '[' + (column ? column.actualName() : operand.propertyName) + ']';
    }
    deserialize(stringCriteria) {
        const operand = external_DevExpress_Analytics_Criteria_Utils_namespaceObject.CriteriaOperatorStateMachine.parse(stringCriteria);
        if (operand) {
            (0,external_DevExpress_Analytics_Criteria_Utils_namespaceObject.criteriaForEach)(operand, operator => {
                if (operator instanceof external_DevExpress_Analytics_Criteria_namespaceObject.OperandProperty) {
                    operator['propertyName'] = this._aggregatePropertyName(operator);
                }
            });
        }
        return super.deserializeOperand(operand);
    }
}

;// external "DevExpress.Analytics.Widgets.Filtering"
const external_DevExpress_Analytics_Widgets_Filtering_namespaceObject = DevExpress.Analytics.Widgets.Filtering;
;// ./dist/query-builder/elements/parameterModel.js


class ParameterViewModel extends DataSourceParameter {
    getEditorType(type) {
        if ((0,external_DevExpress_Analytics_Internal_namespaceObject.IsDataAccessExpression)(type))
            return { header: 'dxqb-expressionstring' };
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getEditorType)(type);
    }
}

;// ./dist/query-builder/widgets/filterEditor/_operandParameterQBSurface.js





class OperandParameterQBSurface extends external_DevExpress_Analytics_Widgets_Filtering_namespaceObject.OperandParameterSurface {
    get _parameterType() {
        return this.fieldsOptions() && this.fieldsOptions().selected() && this.fieldsOptions().selected()['dataType'] || external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString;
    }
    constructor(operator, parent, fieldListProvider, path) {
        super(operator, parent, fieldListProvider, path);
        this.createParameter = () => {
            if (this.canCreateParameters) {
                this.model.parameterName = this.parameterName();
                this._createParameter(this.parameterName(), this._parameterType);
                this.helper.onChange();
            }
        };
        this._parameterName = external_ko_namespaceObject.observable('');
        this.isEditable = external_ko_namespaceObject.observable(false);
        this.defaultDisplay = OperandParameterQBSurface.defaultDisplay;
        this._parameterName(external_ko_namespaceObject.unwrap(operator.parameterName));
        this.canCreateParameters = this.helper.canCreateParameters;
        this.fieldsOptions = parent.leftPart.fieldsOptions;
        this.parameterName = external_ko_namespaceObject.pureComputed({
            read: () => {
                return this._parameterName() || (this.canCreateParameters && OperandParameterQBSurface.defaultDisplay() || '');
            },
            write: (newVal) => {
                if (newVal !== OperandParameterQBSurface.defaultDisplay() && newVal) {
                    this.model.parameterName = external_ko_namespaceObject.unwrap(newVal);
                    this._parameterName(this.model.parameterName);
                }
            }
        });
        if (this.canCreateParameters && !this.isDefaultTextDisplayed() && !external_ko_namespaceObject.unwrap(this.fieldListProvider).hasParameter(operator.parameterName)) {
            this.createParameter();
        }
    }
    _createParameter(name, dataType) {
        if (name !== '' && name !== OperandParameterQBSurface.defaultDisplay() && this.helper.parameters().filter((parameter) => { return parameter.name() === name; }).length === 0) {
            const parameter = new ParameterViewModel({ '@Name': name, '@Type': dataType });
            this.helper.newParameters.push(parameter);
        }
    }
    isDefaultTextDisplayed() {
        return this.parameterName() === OperandParameterQBSurface.defaultDisplay();
    }
}
OperandParameterQBSurface.defaultDisplay = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Create new parameter', 'AnalyticsCoreStringId.FilterEditor_Operand_CreateNewParameter');

;// ./dist/query-builder/widgets/filterEditor/_operandPropertyQBSurface.js


class OperandPropertyQBSurface extends external_DevExpress_Analytics_Widgets_Filtering_namespaceObject.OperandPropertySurface {
    _updateSpecifics() {
        OperandPropertyQBSurface.updateSpecifics(this);
    }
    constructor(operator, parent, fieldListProvider, path) {
        super(operator, parent, fieldListProvider, path);
    }
    static updateSpecifics(propertySurface) {
        const fieldList = external_ko_namespaceObject.unwrap(propertySurface.fieldListProvider);
        if (fieldList && fieldList.getColumnInfo) {
            const item = fieldList.getColumnInfo(propertySurface.propertyName());
            if (item) {
                propertySurface.specifics(item.specifics.toLowerCase());
                propertySurface.dataType(item.dataType);
                const _fieldsOptions = external_ko_namespaceObject.unwrap(propertySurface.fieldsOptions);
                if (_fieldsOptions)
                    _fieldsOptions.selected(item);
            }
        }
    }
}

;// ./dist/query-builder/widgets/filterEditor/_qbFilterEditorHelper.js









class QBFilterEditorHelper extends external_DevExpress_Analytics_Widgets_namespaceObject.FilterEditorHelper {
    constructor(parametersMode) {
        super();
        this.handlers.changeParameter = (criteria, popupService) => {
            return {
                data: new external_DevExpress_Analytics_Widgets_Internal_namespaceObject.FilterEditorAddOn(criteria, popupService, 'changeParameter', 'items', 'dxqb-filtereditor-parameterspopup'),
                templateName: 'dxqb-filtereditor-changeparameter'
            };
        };
        this.mapper.parameter = OperandParameterQBSurface;
        this.mapper.property = OperandPropertyQBSurface;
        if (parametersMode === ParametersMode.ReadWrite) {
            this.canCreateParameters = true;
            this.newParameters = external_ko_namespaceObject.observableArray([]);
            this.onEditorFocusOut = (criteria) => {
                if (!criteria)
                    return;
                const parameters = this.newParameters();
                const usesParameters = [];
                (0,external_DevExpress_Analytics_Criteria_Utils_namespaceObject.criteriaForEach)(criteria, (child) => {
                    if (child instanceof external_DevExpress_Analytics_Criteria_namespaceObject.OperandParameter) {
                        const parameter = parameters.filter(x => x.name() === child.parameterName)[0];
                        if (parameter)
                            usesParameters.push(parameter);
                    }
                });
                if (usesParameters.length === 0) {
                    this.newParameters.splice(0);
                    return;
                }
                const uselessParameters = parameters.filter(x => usesParameters.indexOf(x) === -1);
                for (let i = 0; i < uselessParameters.length; i++) {
                    const parameterIndex = parameters.indexOf(uselessParameters[i]);
                    if (parameterIndex !== -1)
                        parameters.splice(parameterIndex, 1);
                }
                this.newParameters.valueHasMutated();
            };
            this.onClosing = () => {
                this.newParameters([]);
            };
        }
        this.canSelectLists = false;
        this.getDisplayPropertyName = () => new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve('').promise();
    }
}
let QBFilterEditorHelperDefault = QBFilterEditorHelper;
function _setQBFilterEditorHelperDefault(helperType) {
    QBFilterEditorHelperDefault = helperType;
}

;// ./dist/query-builder/widgets/filterEditor/_qbFilterStringOptions.js





class QBFilterStringOptions extends external_DevExpress_Analytics_Widgets_namespaceObject.FilterStringOptions {
    constructor(filterString, dataMember, disabled, title) {
        super(filterString, dataMember, disabled, title);
    }
    initializeFilterStringHelper(parameters, parametersMode, serializer) {
        const helper = new QBFilterEditorHelperDefault(parametersMode);
        helper.canChoiceParameters = parametersMode !== ParametersMode.Disabled;
        if (serializer) {
            helper.serializer = serializer;
        }
        if (parametersMode === ParametersMode.ReadWrite) {
            helper.parameters = external_ko_namespaceObject.computed(() => [].concat(parameters(), helper.newParameters()));
            helper.onSave = (operandProperty) => {
                const newParameters = helper.newParameters();
                parameters.push.apply(parameters, newParameters);
                helper.newParameters([]);
            };
        }
        else {
            if (parametersMode === ParametersMode.Read)
                helper.parameters = parameters;
            const oldCheckRightPart = helper.criteriaTreeValidator._checkRightPart;
            helper.criteriaTreeValidator._checkRightPart = (criteriaOperator) => {
                if (!(criteriaOperator instanceof external_DevExpress_Analytics_Criteria_namespaceObject.OperandParameter))
                    return oldCheckRightPart.apply(this, [criteriaOperator]);
                if (parametersMode === ParametersMode.Disabled) {
                    return false;
                }
                else if (parametersMode === ParametersMode.Read) {
                    const parameterName = criteriaOperator.parameterName;
                    return parameters.peek().filter(x => x.name() === parameterName).length !== 0;
                }
            };
        }
        this.helper = helper;
    }
}

;// ./dist/query-builder/elements/queryModelMeta.js


const querySerializationsInfo = [
    {
        propertyName: '_tablesObject', modelName: 'Tables', info: [
            { propertyName: 'tables', modelName: 'SelectedTables', array: true },
            { propertyName: 'relations', modelName: 'Relations', array: true }
        ]
    },
    { propertyName: 'parameters', modelName: 'Parameters', array: true },
    { propertyName: 'type', modelName: '@Type' },
    { propertyName: 'name', modelName: '@Name' },
    { propertyName: 'editableName', displayName: 'Name', localizationId: 'DevExpress.DataAccess.Sql.SqlQuery.Name', editor: editorTemplates.getEditor('text') },
    { propertyName: '_filterString', modelName: 'Filter', defaultVal: '' },
    { propertyName: 'filterString', defaultVal: '', displayName: 'Filter', localizationId: 'DataAccessUIStringId.FiltersView_Filter', editor: editorTemplates.getEditor('filterEditor') },
    { propertyName: '_groupFilterString', modelName: 'GroupFilter', defaultVal: '' },
    { propertyName: 'groupFilterString', defaultVal: '', displayName: 'Group Filter', localizationId: 'DataAccessUIStringId.FiltersView_GroupFilter', editor: editorTemplates.getEditor('filterGroupEditor') },
    { propertyName: 'columns', modelName: 'Columns', array: true },
    { propertyName: 'sorting', modelName: 'Sorting', array: true },
    { propertyName: 'grouping', modelName: 'Grouping', array: true },
    { propertyName: 'itemType', modelName: '@ItemType' },
    { propertyName: 'allColumnsInTablesSelected', displayName: 'Select All (*)', localizationId: 'AnalyticsCoreStringId.QueryBuilder_SelectAll', editor: editorTemplates.getEditor('bool') },
    { propertyName: 'top', modelName: '@Top', displayName: 'Select Top', defaultVal: 0, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel, localizationId: 'AnalyticsCoreStringId.QueryBuilder_SelectTop', editor: editorTemplates.getEditor('numeric'), editorOptions: { format: '#0', min: 0 } },
    { propertyName: 'skip', modelName: '@Skip', displayName: 'Offset', defaultVal: 0, from: external_DevExpress_Analytics_Utils_namespaceObject.floatFromModel, localizationId: 'AnalyticsCoreStringId.QueryBuilder_Offset', editor: editorTemplates.getEditor('numeric'), editorOptions: { format: '#0', min: 0 } },
    { propertyName: 'distinct', modelName: '@Distinct', defaultVal: false, from: external_DevExpress_Analytics_Utils_namespaceObject.parseBool, displayName: 'Select distinct', localizationId: 'AnalyticsCoreStringId.QueryBuilder_SelectDistinct', editor: editorTemplates.getEditor('bool') }
];

;// ./dist/query-builder/elements/columnModel.js







class ColumnViewModel extends QueryElementBaseViewModel {
    _isAliasAutoGenerated(addedTableName) {
        if (addedTableName && this.alias() && this.alias().indexOf(addedTableName + '_') === 0) {
            if (this.alias().substring(addedTableName.length + 1) === this.name())
                return true;
        }
        if (!this.alias() || this.alias().indexOf(this.name() + '_') !== 0)
            return false;
        let funcName = this.alias().substring(this.name().length + 1);
        if (funcName.match(new RegExp('_[0-9]+$')))
            funcName = funcName.substring(0, funcName.indexOf('_'));
        return Object.keys(AggregationType).indexOf(funcName) > 0;
    }
    getInfo() {
        return columnSerializationInfo;
    }
    constructor(model, dbColumn, parent, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'Column' }, model), parent, serializer);
        this.isNotAvailable = external_ko_namespaceObject.observable(false);
        this._type = external_ko_namespaceObject.observable(DBColumnType.Unknown);
        this._size = external_ko_namespaceObject.observable('');
        if (dbColumn.size)
            this._size(dbColumn.size);
        if (dbColumn.type)
            this._type(dbColumn.type);
        this.displayType = external_ko_namespaceObject.pureComputed(() => {
            return DBColumnType[this._type()] + (this._size() ? '(' + this._size() + ')' : '');
        });
        this.dataType = external_ko_namespaceObject.pureComputed(() => {
            return DBColumn.GetType(this._type());
        });
        this.actualName = external_ko_namespaceObject.pureComputed(() => {
            return this.alias() || this.name();
        });
        const points = parent.getColumnConnectionPoints(this);
        this.rightConnectionPoint = {
            side: external_ko_namespaceObject.observable(PointSide.East),
            location: points.right
        };
        this.leftConnectionPoint = {
            side: external_ko_namespaceObject.observable(PointSide.West),
            location: points.left
        };
        const query = parent.parentModel();
        const targetColumn = external_ko_namespaceObject.pureComputed(() => ColumnExpressionCollectionHelper.find(query.columns, parent.actualName(), this.name()));
        this.aggregate = external_ko_namespaceObject.pureComputed({
            read: () => targetColumn() ? targetColumn().aggregate() : AggregationType.None,
            write: (value) => {
                targetColumn() && targetColumn().aggregate(value);
            }
        });
        this.alias = external_ko_namespaceObject.pureComputed({
            read: () => targetColumn() ? targetColumn().alias() : '',
            write: (value) => {
                targetColumn() && targetColumn().alias(value || null);
            }
        });
        const orderByItem = external_ko_namespaceObject.pureComputed(() => ColumnExpressionCollectionHelper.find(query.sorting, parent.actualName(), this.name()));
        this.sortingType = external_ko_namespaceObject.computed({
            read: () => {
                if (!orderByItem())
                    return 'Unsorted';
                return orderByItem().descending() ? 'Descending' : 'Ascending';
            },
            write: (newValue) => {
                if (newValue !== 'Unsorted') {
                    if (orderByItem()) {
                        orderByItem().descending(newValue === 'Descending');
                    }
                    else {
                        ColumnExpressionCollectionHelper.addNew(query, query.sorting, parent.actualName(), this.name())
                            .descending(newValue === 'Descending');
                    }
                }
                else if (orderByItem()) {
                    ColumnExpressionCollectionHelper.remove(query.sorting, parent.actualName(), this.name());
                }
            }
        });
        this.sortOrder = external_ko_namespaceObject.computed({
            read: () => {
                const index = query.sorting().indexOf(orderByItem());
                return index < 0 ? undefined : index + 1;
            },
            write: (newValue) => {
                if (!orderByItem())
                    return;
                newValue = Math.min(newValue, query.sorting().length);
                newValue = Math.max(newValue, 1);
                const oldValue = query.sorting().indexOf(orderByItem());
                const item = query.sorting.splice(oldValue, 1);
                query.sorting.splice(newValue - 1, 0, item[0]);
            }
        });
        const groupByItem = external_ko_namespaceObject.computed(() => ColumnExpressionCollectionHelper.find(query.grouping, parent.actualName(), this.name()));
        this.aggregate.subscribe((value) => {
            const parentTable = this.parentModel();
            if (value !== AggregationType.None) {
                this.groupBy(false);
                if (!this.alias() || this._isAliasAutoGenerated(parentTable.actualName())) {
                    const aggregateAlias = this.name() + '_' + value;
                    this.alias(ColumnExpressionCollectionHelper.setUniqueAlias(query.columns, aggregateAlias));
                }
            }
            else if (this._isAliasAutoGenerated(parentTable.actualName())) {
                this.alias(null);
            }
        });
        this.groupBy = external_ko_namespaceObject.computed({
            read: () => !!groupByItem(),
            write: (value) => {
                if (value) {
                    ColumnExpressionCollectionHelper.addNew(query, query.grouping, parent.actualName(), this.name());
                    this.aggregate(AggregationType.None);
                }
                else {
                    ColumnExpressionCollectionHelper.remove(query.grouping, parent.actualName(), this.name());
                }
            }
        });
        this.toggleSelected = (value, lazy = false) => {
            if (!!targetColumn() === value)
                return;
            if (value) {
                ColumnExpressionCollectionHelper.addNew(query, query.columns, parent.actualName(), this.name(), lazy);
            }
            else {
                ColumnExpressionCollectionHelper.remove(query.columns, parent.actualName(), this.name(), lazy);
                this.groupBy(false);
            }
        };
        this.selected = external_ko_namespaceObject.pureComputed({
            read: () => !!targetColumn(),
            write: (value) => this.toggleSelected(value)
        });
    }
    isPropertyDisabled(name) {
        if (this.isNotAvailable())
            return true;
        if (name === 'sortOrder') {
            return this.sortingType() === 'Unsorted';
        }
        else if (name === 'aggregate' || name === 'alias') {
            return !this.selected();
        }
        else if (name === 'groupBy') {
            const query = this.root;
            return this.aggregate() && query.aggregatedColumnsCount() === 1;
        }
        else
            return false;
    }
    get specifics() {
        return DBColumn.GetSpecific(this.dataType());
    }
}

;// ./dist/query-builder/elements/tableModelMeta.js

const tableSerializationInfo = [
    query_builder_metadata_name, alias, { propertyName: 'controlType', modelName: '@ControlType' }, { propertyName: 'itemType', modelName: '@ItemType' }
];

;// ./dist/query-builder/elements/tableModel.js








class TableViewModel extends QueryElementBaseViewModel {
    constructor(model, parent, serializer) {
        super(model, parent, serializer);
        this.serializer = serializer;
        this._columnsConnectionPointLeftX = external_ko_namespaceObject.pureComputed(() => { return this.location.x(); });
        this._columnsConnectionPointRightX = external_ko_namespaceObject.pureComputed(() => { return this.location.x() + this.size.width(); });
        this._columns = external_ko_namespaceObject.observableArray();
        this._initialized = external_ko_namespaceObject.observable(false);
        this.tableOffset = external_ko_namespaceObject.observable(0);
        this.size = new external_DevExpress_Analytics_Elements_namespaceObject.Size(199, 123);
        this.location = new external_DevExpress_Analytics_Elements_namespaceObject.Point(0, 0);
        this.isReady = external_ko_namespaceObject.observable(false);
        this.allColumnsSelected = external_ko_namespaceObject.computed({
            read: () => {
                const selectedColumns = this.columns().filter(item => item.selected());
                if (selectedColumns.length === 0) {
                    return false;
                }
                if (selectedColumns.length === this._columns.peek().length) {
                    return true;
                }
                return false;
            },
            deferEvaluation: true
        });
        this.isInitialized = external_ko_namespaceObject.pureComputed(() => this._initialized());
        this.itemType = 'Table';
        this.controlType = 'Table';
        this._disposables.push(this.size.height = external_ko_namespaceObject.pureComputed({
            read: () => {
                if (this._columns().length === 0) {
                    return TableViewModel.TABLE_DEFAULT_HEIGHT + this.tableOffset();
                }
                return TableViewModel.COLUMNS_OFFSET + (TableViewModel.COLUMN_HEIGHT + TableViewModel.COLUMN_MARGIN) * (this._columns().length + 1) + this.tableOffset();
            },
            write: () => {
            }
        }));
        this.asterisk = new AllColumnsViewModel(this, this.serializer);
        this._disposables.push(this.actualName = external_ko_namespaceObject.pureComputed(() => this.alias() || this.name()));
    }
    columns() {
        return this._columns();
    }
    toggleSelectedColumns() {
        const value = !this.allColumnsSelected.peek();
        const query = (this.parentModel());
        this._columns.peek().forEach((column) => column.toggleSelected(value, true));
        query.columns.valueHasMutated();
    }
    getColumnConnectionPoints(column) {
        const y = external_ko_namespaceObject.pureComputed({
            read: () => {
                const index = this._columns.indexOf(column) + 1;
                return this.location.y() + TableViewModel.COLUMNS_OFFSET + TableViewModel.COLUMN_MARGIN * index + TableViewModel.COLUMN_HEIGHT * (index + 0.5) + this.tableOffset();
            },
            deferEvaluation: true
        });
        this._disposables.push(y);
        return {
            left: { x: this._columnsConnectionPointLeftX, y: y },
            right: { x: this._columnsConnectionPointRightX, y: y }
        };
    }
    getInfo() {
        return tableSerializationInfo;
    }
    getInvalidColumns() {
        return this.columns().filter(x => x.isNotAvailable() && x.selected());
    }
    getColumn(name) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this._columns(), 'name', name);
    }
    _initColumns(columns, update = false) {
        const result = [];
        columns.forEach((item) => {
            result.push(this.createChildColumn(item));
        });
        this._columns(update ? [].concat([], this.columns(), result) : result);
    }
    createChildColumn(item) {
        return new ColumnViewModel({ '@Name': item.name }, item, this, this.serializer);
    }
    createColumns(dbTable) {
        let notAvailableColumnsCount = 0;
        this.columns().forEach((column) => {
            const expectedColumn = dbTable.columns.filter(x => x.name === column.name())[0];
            if (expectedColumn) {
                column._type(expectedColumn.type);
                column._size(expectedColumn.size);
            }
            else {
                column.isNotAvailable(dbTable.columns.every(x => x.name !== column.name()));
                notAvailableColumnsCount++;
            }
        });
        if (this.columns().length - notAvailableColumnsCount < dbTable.columns.length) {
            this._initColumns(dbTable.columns.filter(x => this.columns().every(c => c.name() !== x.name)), true);
        }
        this._initialized(true);
    }
}
TableViewModel.COLUMNS_OFFSET = 37;
TableViewModel.COLUMN_HEIGHT = 32;
TableViewModel.COLUMN_MARGIN = 1;
TableViewModel.TABLE_MIN_WIDTH = 80;
TableViewModel.TABLE_DEFAULT_HEIGHT = 136;

;// ./dist/query-builder/elements/queryModel.js























class QueryViewModelBase extends QueryElementBaseViewModel {
    _initializeTable(table) {
        this.dbSchemaProvider.getDbTable(table.name())
            .done((dbTable) => {
            table.createColumns(dbTable);
        });
    }
    _addColumnsToTable(table, columns) {
        table._initColumns(columns.map(x => {
            return new DBColumn({
                Name: x.column()
            });
        }));
    }
    constructor(querySource, dbSchemaProvider, parametersMode = ParametersMode.ReadWrite, beforeSaveCallback, serializer) {
        super(querySource, null, serializer);
        this.topOffset = 65;
        this._findAncestorsRelations = (table) => {
            const result = { inner: 0, outer: 0, relations: [] };
            this.relations().forEach((item) => {
                if (item.nestedTable() === table) {
                    result.relations.push(item);
                    item.joinType() === 'LeftOuter' ? result.outer++ : result.inner++;
                    const parentResult = this._findAncestorsRelations(item.parentTable());
                    result.inner += parentResult.inner;
                    result.outer += parentResult.outer;
                    result.relations.push.apply(result.relations, parentResult.relations);
                }
            });
            return result;
        };
        this.aggregatedColumnsCount = external_ko_namespaceObject.observable(0);
        this.defaultPageHeight = 500;
        this.defaultPageWidth = 500;
        this.dbSchemaProvider = dbSchemaProvider;
        this.onSave = beforeSaveCallback;
        this.editableName = external_ko_namespaceObject.observable(this.name());
        this._disposables.push(this.name = external_ko_namespaceObject.pureComputed({
            read: this.editableName,
            write: val => { }
        }));
        const _pageWidth = external_ko_namespaceObject.observable(0);
        this._disposables.push(this.pageWidth = external_ko_namespaceObject.pureComputed({
            read: () => {
                let result = this.defaultPageWidth;
                this.tables().forEach((table) => {
                    const right = table.location.x() + table.size.width();
                    if (right > result) {
                        result = right;
                    }
                });
                return Math.max(_pageWidth(), result);
            },
            write: (value) => {
                _pageWidth(value);
            }
        }));
        this._disposables.push(this.pageHeight = external_ko_namespaceObject.pureComputed(() => {
            let result = this.defaultPageHeight;
            this.tables().forEach((table) => {
                const bottom = table.location.y() + table.size.height();
                if (bottom > result) {
                    result = bottom + QueryViewModel.pageMargin;
                }
            });
            return result;
        }));
        this.margins = external_DevExpress_Analytics_Elements_namespaceObject.Margins.fromString();
        this._disposables.push(this.isValid = external_ko_namespaceObject.pureComputed(() => this._validate()));
        const isAllColumnsAllTablesExpression = column => !column.table() && column.itemType() === ColumnType.AllColumns;
        this._disposables.push(this.allColumnsInTablesSelected = external_ko_namespaceObject.pureComputed({
            read: () => this.columns().some(isAllColumnsAllTablesExpression),
            write: (value) => {
                if (value) {
                    this.columns.push(new ColumnExpression({ '@ItemType': 'AllColumns' }, this, serializer));
                }
                else {
                    this.columns.remove(isAllColumnsAllTablesExpression);
                }
            }
        }));
        if (!this.sorting)
            this.sorting = external_ko_namespaceObject.observableArray([]);
        if (!this.grouping)
            this.grouping = external_ko_namespaceObject.observableArray([]);
    }
    dispose() {
        super.dispose();
        this.onSave = null;
    }
    addChild(control) {
        if (control instanceof RelationViewModel) {
            if (this.relations.indexOf(control) > -1)
                return;
            control.parentModel(this);
            this.relations.push(control);
        }
        else if (control instanceof TableViewModel) {
            if (this.tables.indexOf(control) > -1)
                return;
            control.parentModel(this);
            if ((0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.tables(), 'actualName', control.name()) !== null) {
                control.alias((0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueName)(this.tables().map((table) => { return table.actualName(); }), control.name() + '_'));
            }
            this.tables.push(control);
        }
        else {
            (0,external_DevExpress_Analytics_Internal_namespaceObject.NotifyAboutWarning)('Attempt to add wrong child control.');
        }
    }
    removeChild(control) {
        if (control instanceof RelationViewModel) {
            if (this.relations().length < 1)
                return;
            const relation = control;
            const indexRelation = this.relations().indexOf(relation);
            while (relation.conditions().length > 0)
                relation.conditions.pop();
            this.relations.splice(indexRelation, 1);
        }
        else if (control instanceof TableViewModel) {
            if (this.tables().length < 1)
                return;
            this.tables.splice(this.tables().indexOf(control), 1);
            const relations = this.relations();
            for (let i = relations.length - 1; i > -1; i--) {
                if (relations[i].parentTable() === control || relations[i].nestedTable() === control) {
                    this.removeChild(relations[i]);
                }
            }
            this.sorting.remove((item) => item.isDepended(control.actualName()));
            this.grouping.remove((item) => item.isDepended(control.actualName()));
            this.columns.remove((item) => item.isDepended(control.actualName()));
        }
        else {
            (0,external_DevExpress_Analytics_Internal_namespaceObject.NotifyAboutWarning)('Attempt to remove wrong child control.');
        }
    }
    validateRelations() {
        const tables = this.tables().map(table => table.actualName());
        this._validateTable(tables, tables[0]);
        return tables.length < 1;
    }
    _validate() {
        if (this.tables().length === 0)
            return false;
        if (!(this.allColumnsInTablesSelected() || this.columns().length > 0))
            return false;
        return this.validateRelations();
    }
    _validateTable(tables, tableName) {
        const index = tables.indexOf(tableName);
        if (index < 0)
            return;
        tables.splice(index, 1);
        const connectedTables = this.relations().map((relation) => {
            if (relation.parentTableName() === tableName)
                return relation.nestedTableName();
            if (relation.nestedTableName() === tableName)
                return relation.parentTableName();
            return null;
        });
        connectedTables.forEach((item) => this._validateTable(tables, item));
    }
    createChild(info, tableViewModel, path) {
        if (!tableViewModel)
            return super.createChild(info);
        this._initializeTable(tableViewModel);
        this.addChild(tableViewModel);
        return tableViewModel;
    }
    init() {
        this.tables().forEach((table) => {
            const columns = [];
            const sorting = this.sorting && this.sorting() || [];
            const grouping = this.grouping && this.grouping() || [];
            [this.columns(), sorting, grouping].forEach((currentColumns) => {
                currentColumns.forEach((column) => {
                    if (table.name() === column.table() && column.itemType() === ColumnType.Column &&
                        columns.every(x => x.column() !== column.column())) {
                        columns.push(column);
                    }
                });
            });
            this._addColumnsToTable(table, columns);
            this._initializeTable(table);
        });
        this.tables().reduce((posX, tableModel) => {
            tableModel.location.x(posX);
            tableModel.location.y(this.topOffset);
            return posX + tableModel.size.width() + tableModel.size.width() / 2;
        }, 30);
        let inProcess = false;
        this._disposables.push(external_ko_namespaceObject.computed(() => {
            if (!inProcess) {
                inProcess = true;
                const allColumns = this.getAllColumns();
                const withoutAggregate = allColumns.filter(x => x.aggregate() === AggregationType.None);
                this.aggregatedColumnsCount(allColumns.length - withoutAggregate.length);
                if (allColumns.length !== withoutAggregate.length) {
                    withoutAggregate.filter(x => x.selected() && !x.groupBy.peek()).forEach(x => x.groupBy(true));
                }
                else {
                    if (!allColumns.every(x => !x.selected.peek() || x.groupBy.peek())) {
                        allColumns.forEach(x => { if (x.groupBy.peek()) {
                            x.groupBy(false);
                        } });
                    }
                }
                inProcess = false;
            }
        }));
    }
    getTable(name) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.tables(), item => item.actualName() === name);
    }
    canSave(showMessage = true) {
        const message = [];
        this.tables().forEach((t) => {
            t.getInvalidColumns().forEach((column) => {
                message.push((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('The schema does not contain the following column: "{0}"."{1}".', 'DataAccessStringId.ColumnNotInSchemaValidationException'), t.actualName(), column.actualName()));
            });
        });
        if (message.length > 0) {
            showMessage && (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)(message.join('\n'), 'error');
            return false;
        }
        return true;
    }
    save() {
        if (!this.canSave())
            return;
        const data = this.serialize(true);
        if (this.onSave) {
            this.onSave(data);
        }
        return data;
    }
    serialize(includeRootTag = false) {
        return includeRootTag ? { 'Query': this.serialize() } : (new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()).serialize(this);
    }
    _findTableInAncestors(child, probablyAncestor) {
        return this.relations().some((relation) => {
            return relation.nestedTable() === child && (relation.parentTable() === probablyAncestor || this._findTableInAncestors(relation.parentTable(), probablyAncestor));
        });
    }
    _findHead(table) {
        let result = null;
        this.relations().some((relation) => {
            if (relation.nestedTable() === table)
                result = relation;
            return !!result;
        });
        return result ? this._findHead(result.parentTable()) : table;
    }
    _isHead(table) {
        return !this.relations().some(relation => relation.nestedTable() === table);
    }
    _reverseRelations(table, relationsToReverse) {
        relationsToReverse.forEach((item) => {
            const tempTable = item.parentTable();
            item.parentTable(item.nestedTable());
            item.nestedTable(tempTable);
            item.conditions().forEach((condition) => {
                const tempColumn = condition.parentColumnName();
                condition.parentColumnName(condition.nestedColumnName());
                condition.nestedColumnName(tempColumn);
            });
        });
    }
    getAllColumns() {
        return [].concat.apply([], this.tables().map(x => x.columns()));
    }
    cerateJoinCondition(parentColumn, nestedColumn) {
        let parentTable = parentColumn.parentModel();
        let nestedTable = nestedColumn.parentModel();
        if (parentTable === nestedTable)
            return null;
        let isColumnsReplaced = false;
        let relation = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.relations(), (relation) => {
            isColumnsReplaced = relation.parentTable() === nestedTable && relation.nestedTable() === parentTable;
            return relation.parentTable() === parentTable && relation.nestedTable() === nestedTable || isColumnsReplaced;
        });
        if (relation) {
        }
        else if (this._findTableInAncestors(parentTable, nestedTable)) {
            isColumnsReplaced = true;
        }
        else if (this._findHead(parentTable) !== this._findHead(nestedTable) && !this._isHead(nestedTable)) {
            const parentRelations = this._findAncestorsRelations(parentTable);
            const nestedRelations = this._findAncestorsRelations(nestedTable);
            if (parentRelations.outer > nestedRelations.outer) {
                this._reverseRelations(nestedTable, nestedRelations.relations);
            }
            else if (parentRelations.outer < nestedRelations.outer) {
                this._reverseRelations(parentTable, parentRelations.relations);
                isColumnsReplaced = true;
            }
            else if (parentRelations.inner >= nestedRelations.inner) {
                this._reverseRelations(nestedTable, nestedRelations.relations);
            }
            else if (parentRelations.inner < nestedRelations.inner) {
                this._reverseRelations(parentTable, parentRelations.relations);
                isColumnsReplaced = true;
            }
        }
        if (isColumnsReplaced) {
            const tempTable = parentTable;
            parentTable = nestedTable;
            nestedTable = tempTable;
            const tempColumn = parentColumn;
            parentColumn = nestedColumn;
            nestedColumn = tempColumn;
        }
        relation = relation || this.createChild({
            '@ControlType': 'Relation',
            '@Parent': parentTable.actualName(),
            '@Nested': nestedTable.actualName(),
            '@Type': 'Inner'
        });
        let joinCondition = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(relation.conditions(), (condition) => {
            return condition.parentColumn() === parentColumn && condition.nestedColumn() === nestedColumn;
        });
        if (!joinCondition) {
            joinCondition = relation.createChild({ '@ControlType': 'JoinCondition', '@Parent': parentColumn.name(), '@Nested': nestedColumn.name() });
        }
        return joinCondition;
    }
    tryToCreateRelationsByFK(sourceTable) { }
}
QueryViewModelBase.pageMargin = 20;
class QueryViewModel extends QueryViewModelBase {
    constructor(querySource, dbSchemaProvider, parametersMode = ParametersMode.ReadWrite, beforeSaveCallback, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, querySource, QueryViewModel.emptyModel, querySource), dbSchemaProvider, parametersMode, beforeSaveCallback, serializer);
        this.controlType = 'Query';
        this['type']('SelectQuery');
        this.tables = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Tables']['SelectedTables'], item => new TableViewModel(item, this, serializer));
        this.columns = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Columns'], item => new ColumnExpression(item, this, serializer));
        this.sorting = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Sorting'], item => new ColumnExpression(item, this, serializer));
        this.grouping = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Grouping'], item => new ColumnExpression(item, this, serializer));
        this.relations = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Tables']['Relations'], item => new RelationViewModel(item, this, serializer));
        this.init();
        this['_tablesObject']['tables'] = this.tables;
        this['_tablesObject']['relations'] = this.relations;
        const parameters = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Parameters'], item => new ParameterViewModel(item, serializer));
        if (parametersMode === ParametersMode.ReadWrite) {
            this.parameters = parameters;
            this._disposables.push(this.parameters.subscribe((changes) => {
                changes.forEach((change) => {
                    if (change.status === 'added' && !change.value.name())
                        change.value.name((0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueName)(this.parameters().filter(x => x !== change.value).map(x => x.name()), 'parameter'));
                });
            }, null, 'arrayChange'));
        }
        else {
            this._disposables.push(this.parameters = external_ko_namespaceObject.computed(() => { return parameters(); }));
        }
        this.filterString = new QBFilterStringOptions(this._filterString, null, external_ko_namespaceObject.pureComputed(() => (this.tables().length === 0) && (this.filterString && this.filterString.value().length === 0)));
        this.filterString.initializeFilterStringHelper(this.parameters, parametersMode);
        this.groupFilterString = new QBFilterStringOptions(this._groupFilterString, null, external_ko_namespaceObject.pureComputed(() => !this.columns().some(isAggregatedExpression) && (this.groupFilterString && (this.groupFilterString.value() || '').length === 0)));
        this.groupFilterString.initializeFilterStringHelper(this.parameters, parametersMode, new GroupFilterEditorSerializer(this.columns));
    }
    isPropertyDisabled(name) {
        if (name === 'skip')
            return this.skip() === 0 && !this.sorting().length;
        return false;
    }
    getInfo() {
        return querySerializationsInfo;
    }
    createChild(info) {
        return super.createChild(info, info['@ControlType'] === 'Table' ? new TableViewModel(info, this) : undefined);
    }
    tryToCreateRelationsByFK(sourceTable) {
        this.dbSchemaProvider.getDbSchema().done((dbSchema) => {
            const dbSourceTable = dbSchema.tables.filter((item) => { return item.name === sourceTable.name(); })[0];
            if (dbSourceTable) {
                dbSourceTable.foreignKeys.forEach((fk) => {
                    const pkTable = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(this.tables.peek(), 'name', fk.primaryKeyTable);
                    if (pkTable) {
                        for (let i = 0; i < fk.primaryKeyColumns.length; i++) {
                            const column1 = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(sourceTable.columns(), 'name', fk.columns[i]);
                            const column2 = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(pkTable.columns(), 'name', fk.primaryKeyColumns[i]);
                            if (column1 && column2) {
                                this.cerateJoinCondition(column2, column1);
                            }
                        }
                    }
                });
            }
            this.tables.peek().forEach((table) => {
                const dbTable = dbSchema.tables.filter((item) => { return item.name === table.name(); })[0];
                if (dbTable) {
                    dbTable.foreignKeys.forEach((fk) => {
                        if (fk.primaryKeyTable === sourceTable.name()) {
                            for (let i = 0; i < fk.primaryKeyColumns.length; i++) {
                                const column1 = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(sourceTable.columns(), 'name', fk.primaryKeyColumns[i]);
                                const column2 = (0,external_DevExpress_Analytics_Internal_namespaceObject.getFirstItemByPropertyValue)(table.columns(), 'name', fk.columns[i]);
                                if (column1 && column2) {
                                    this.cerateJoinCondition(column2, column1);
                                }
                            }
                        }
                    });
                }
            });
        });
    }
}
QueryViewModel.emptyModel = { '@ItemType': 'Query', 'Tables': { 'SelectedTables': {}, 'Relations': {} }, 'Columns': {}, 'Sorting': {}, 'Grouping': {} };

;// ./dist/query-builder/elements/querySurface.js





class QuerySurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(query, zoom = external_ko_namespaceObject.observable(1)) {
        super(query, {
            measureUnit: external_ko_namespaceObject.observable('Pixels'),
            zoom: zoom,
            dpi: external_ko_namespaceObject.observable(100)
        }, QuerySurface._unitProperties);
        this.placeholder = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Drop a table or view here to create a query.', 'AnalyticsCoreStringId.QueryBuilder_SurfacePlaceholder');
        this.tables = external_ko_namespaceObject.observableArray();
        this.relations = external_ko_namespaceObject.observableArray();
        this.allowMultiselect = false;
        this.focused = external_ko_namespaceObject.observable(false);
        this.selected = external_ko_namespaceObject.observable(false);
        this.underCursor = external_ko_namespaceObject.observable(new external_DevExpress_Analytics_Internal_namespaceObject.HoverInfo());
        this.templateName = 'dx-query-surface';
        this.rtl = external_ko_namespaceObject.observable(false);
        this.measureUnit = this._context.measureUnit;
        this.dpi = this._context.dpi;
        this._context = this;
        this.margins = { bottom: this['_bottom'], left: this['_left'], right: this['_right'], top: this['_top'] };
        this.zoom = zoom;
        (0,external_DevExpress_Analytics_Internal_namespaceObject.createObservableArrayMapCollection)(query.tables, this.tables, this._createSurface);
        (0,external_DevExpress_Analytics_Internal_namespaceObject.createObservableArrayMapCollection)(query.relations, this.relations, this._createSurface);
        this._joinedColumns = external_ko_namespaceObject.computed(() => {
            const resultColumns = [];
            this.relations().forEach(relation => {
                relation.conditions().forEach(condition => {
                    const joinModel = condition.getControlModel();
                    joinModel.parentColumn() && resultColumns.push(joinModel.parentColumn());
                    joinModel.nestedColumn() && resultColumns.push(joinModel.nestedColumn());
                });
            });
            return resultColumns;
        });
    }
    checkParent(surfaceParent) { return false; }
    getChildrenCollection() {
        return this.tables;
    }
    isJoined(column) {
        return this._joinedColumns().indexOf(column.getControlModel()) > -1;
    }
}
QuerySurface._unitProperties = {
    _width: (o) => { return o.pageWidth; },
    _height: (o) => { return o.pageWidth; },
    pageWidth: (o) => { return o.pageWidth; },
    pageHeight: (o) => { return o.pageHeight; },
    _bottom: (o) => { return o.margins.bottom; },
    _left: (o) => { return o.margins.left; },
    _right: (o) => { return o.margins.right; },
    _top: (o) => { return o.margins.top; }
};

;// ./dist/query-builder/elements/queryElementSurface.js



class QueryElementBaseSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context, unitProperties) {
        super(control, context, (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, QueryElementBaseSurface._unitProperties, unitProperties));
        this.template = 'dx-diagram-element';
        this.selectiontemplate = 'dx-diagram-element-selection';
        this.contenttemplate = 'dx-diagram-element-content';
        this.margin = external_ko_namespaceObject.observable(0);
    }
}
QueryElementBaseSurface._unitProperties = {
    _height: (o) => {
        return o.size.height;
    },
    _width: (o) => {
        return o.size.width;
    },
    _x: (o) => {
        return o.location.x;
    },
    _y: (o) => {
        return o.location.y;
    }
};

;// ./dist/query-builder/elements/columnSurface.js




class ColumnSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context) {
        super(control, context, null);
        this.template = 'dxqb-table-field';
        this.toggleSelected = () => {
            this.getControlModel().selected(!this.getControlModel().selected());
        };
        this.selectedWrapper = external_ko_namespaceObject.pureComputed(() => {
            return this.getControlModel().selected();
        });
        this.isNotAvailable = external_ko_namespaceObject.pureComputed(() => this._control.isNotAvailable());
        this.isAggregate = external_ko_namespaceObject.pureComputed(() => isAggregatedExpression(this.getControlModel()));
        this.isAscending = external_ko_namespaceObject.pureComputed(() => {
            return this.getControlModel().sortingType() === 'Ascending';
        });
        this.isDescending = external_ko_namespaceObject.pureComputed(() => {
            return this.getControlModel().sortingType() === 'Descending';
        });
        this.cssClasses = (query, columnDragHandler, parent) => {
            if (!this._isJoined) {
                this._isJoined = external_ko_namespaceObject.pureComputed(() => {
                    return query.isJoined(this) || this.getControlModel() === columnDragHandler.getDragColumn();
                });
            }
            if (!this._isHovered) {
                this._isHovered = external_ko_namespaceObject.pureComputed(() => {
                    const isColumnDragStarted = !!columnDragHandler.getDragColumn();
                    const isCurrentTableNotParentForDraggedColumn = isColumnDragStarted && this.getControlModel().parentModel() !== columnDragHandler.getDragColumn().parentModel();
                    return this.underCursor().isOver && (!external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler.started() || isCurrentTableNotParentForDraggedColumn);
                });
            }
            return {
                'dxd-state-invalid': this.isNotAvailable,
                'dxd-state-active': this.selected(),
                'dxd-state-joined': this._isJoined,
                'dxd-state-hovered': this._isHovered,
            };
        };
    }
}

;// ./dist/query-builder/elements/allColumnsSurface.js



class AllColumnsSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context) {
        super(control, context, null);
        this.template = 'dxqb-table-asterisk-field';
        this.toggleSelected = () => {
            this.getControlModel().selected(!this.getControlModel().selected());
        };
        this.selectedWrapper = external_ko_namespaceObject.pureComputed(() => {
            return this.getControlModel().selected();
        });
        this.isOverAsterisk = external_ko_namespaceObject.pureComputed(() => {
            return this.underCursor().isOver && !external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler.started();
        });
        this.cssClasses = () => {
            return {
                'dxd-state-active': this.selected,
                'dxd-state-hovered': this.isOverAsterisk()
            };
        };
    }
}

;// ./dist/query-builder/elements/tableSurface.js







class TableSurface extends QueryElementBaseSurface {
    constructor(control, context) {
        super(control, context, null);
        this.showSourceName = false;
        this.contenttemplate = 'dxqb-table';
        this.titletemplate = 'dxqb-table-title';
        this.template = 'dxqb-table-main';
        this.toggleSelected = () => {
            this.getControlModel().toggleSelectedColumns();
        };
        this.selectedWrapper = external_ko_namespaceObject.pureComputed(() => {
            return this.getControlModel().allColumnsSelected();
        });
        this.isInitialized = control.isInitialized;
        this.columns = external_ko_namespaceObject.pureComputed(() => {
            return control.columns().map(columnVewModel => new ColumnSurface(columnVewModel, context));
        });
        this.asterisk = new AllColumnsSurface(control.asterisk, context);
    }
    resizable(resizeHandler, element) {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({}, resizeHandler, {
            handles: 'e,w',
            $selectedNodes: (0,external_DevExpress_Analytics_Internal_namespaceObject.createJQueryElement)(element),
            minWidth: TableViewModel.TABLE_MIN_WIDTH,
        });
    }
}

;// ./dist/query-builder/elements/_federationColumnModel.js




class FederationColumnViewModel extends ColumnViewModel {
    constructor(model, dbColumn, parent, serializer) {
        super(model, dbColumn, parent, serializer);
    }
    getInfo() {
        return expressionSerializationInfo;
    }
}
class FederationAllColumnsViewModel extends AllColumnsViewModel {
    constructor(parent, serializer) {
        super(parent, serializer);
    }
    getInfo() {
        return [query_builder_metadata_name];
    }
}

;// ./dist/query-builder/elements/_federationQueryModel.js
















const federationQuerySerializationsInfo = [
    { propertyName: 'name', modelName: '@Alias' },
    { propertyName: 'type', modelName: '@Type' },
    { propertyName: 'itemType', modelName: '@ItemType' },
    { propertyName: 'queryType', modelName: '@QueryType' },
    { propertyName: 'rootModel', modelName: 'Root' },
    { propertyName: 'expressions', modelName: 'Expressions', array: true },
    { propertyName: 'subNodes', modelName: 'SubNodes', array: true },
];
class FederationQueryViewModel extends QueryViewModelBase {
    _initializeTable(table) {
        this._dbSchemaProvider.getDbTable(table.name(), table.pathString)
            .done((dbTable) => {
            table.createColumns(dbTable);
        });
    }
    _createTableViewModel(model) {
        const path = this.dataSource.getPathFromQueryName(model['@SourceName']);
        return new FederationTableViewModel(model, this, path, this.serializer);
    }
    constructor(querySource, dataSource, _dbSchemaProvider, parametersMode = ParametersMode.ReadWrite, serializer) {
        super((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, querySource, FederationQueryViewModel.emptyModel, querySource), _dbSchemaProvider, parametersMode, (data) => data, serializer);
        this._dbSchemaProvider = _dbSchemaProvider;
        this.serializer = serializer;
        this.expressions = external_ko_namespaceObject.observableArray();
        this.subNodes = external_ko_namespaceObject.observableArray();
        this.controlType = 'FQuery';
        this.defaultPageHeight = 300;
        this.topOffset = 20;
        this.dataSource = dataSource;
        this.tables = external_ko_namespaceObject.observableArray();
        if (querySource['Root']) {
            this.tables.push(this._createTableViewModel(querySource['Root']));
        }
        const subNodes = querySource['SubNodes'];
        this.tables.push(...Object.values(subNodes).map(subNode => this._createTableViewModel(subNode['Query'])));
        const relations = Object.values(subNodes).reduce((result, serializedSubNode) => {
            const subNode = new SubNode(serializedSubNode, serializer);
            return result.concat(subNode.createRelationModel(this));
        }, []);
        this.columns = (0,external_DevExpress_Analytics_Utils_namespaceObject.deserializeArray)(querySource['Expressions'], item => new ColumnExpression({ '@Name': item['@Name'], '@Alias': item['@Alias'], '@Table': item['@NodeAlias'], '#text': item['@ColumnExpression'], '@ItemType': ColumnExpressionCollectionHelper.federatedTypeToColumn(item['@ExpressionType']) }, this, serializer));
        this.relations = external_ko_namespaceObject.observableArray(relations);
        this.init();
    }
    dispose() {
        super.dispose();
        this.dataSource = null;
    }
    serialize(includeRootTag) {
        const serializer = this.serializer || new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        this.expressions(this.columns().map(column => {
            const model = {
                '@Alias': column.alias(),
                '@ColumnExpression': column.expression(),
                '@Name': column.column(),
                '@NodeAlias': column.table()
            };
            model['@ExpressionType'] = ColumnExpressionCollectionHelper.columnTypeToFederated(column.itemType());
            return new FederatedQueryExpression(model, serializer);
        }));
        const tables = [...this.tables()];
        this.rootModel(tables.shift());
        this.subNodes(tables.map(table => {
            const tableRelations = this.relations().filter(relation => relation.nestedTable() === table);
            return tableRelations.length ? SubNode.deserializeRelationModel(table, tableRelations) : null;
        }).filter(subNode => subNode !== null));
        return super.serialize(includeRootTag);
    }
    createChild(info, model, path) {
        return super.createChild(info, info['@ControlType'] === 'FTable' ? new FederationTableViewModel(info, this, path) : undefined);
    }
    cerateJoinCondition(parentColumn, nestedColumn) {
        const parentTable = parentColumn.parentModel();
        const nestedTable = nestedColumn.parentModel();
        if (this.tables().indexOf(parentTable) > this.tables.indexOf(nestedTable))
            return null;
        return super.cerateJoinCondition(parentColumn, nestedColumn);
    }
    getInfo() {
        return federationQuerySerializationsInfo;
    }
}
FederationQueryViewModel.emptyModel = { '@ItemType': 'Query', 'SubNodes': {}, 'Expressions': {} };
class FederationQuerySurface extends QuerySurface {
}
class FederationTableViewModel extends TableViewModel {
    constructor(model, parent, pathParts, serializer) {
        super(model, parent, serializer);
        this.controlType = 'FTable';
        if (pathParts && !model['@SourceName'])
            this.sourceName(parent.dataSource.getQueryNameFromPath(pathParts));
        if (!this.name()) {
            this.name(this.sourceName());
        }
        !this.alias() && this.alias(this.name());
        this._disposables.push(this.queryType = external_ko_namespaceObject.pureComputed(() => { return FederationQueryType[FederationQueryType.SourceNode]; }));
        this.tableOffset(12);
        const dataSourceId = pathParts.shift();
        const dataSource = parent.dataSource.dataSources().filter(x => {
            return x.id === dataSourceId || x.ref === dataSourceId || x.name === dataSourceId;
        })[0];
        this.path = [dataSourceId, ...pathParts];
        this.displaySourceName = external_ko_namespaceObject.observable(dataSource.name);
        this.asterisk = new FederationAllColumnsViewModel(this, this.serializer);
    }
    get pathString() {
        return this.path?.join('.');
    }
    getInfo() {
        return sourceQuerySerializationsInfo;
    }
    getPath() {
        return this.path;
    }
    createChildColumn(item) {
        return new FederationColumnViewModel({ '@Name': item.name }, item, this, this.serializer);
    }
}
class FederationTableSurface extends TableSurface {
    constructor() {
        super(...arguments);
        this.titletemplate = 'dxqb-federation-table-title';
    }
}

;// ./dist/query-builder/utils/_federationUnionQueryBuilderTreeListController.js

class FederatedUnionQueryBuilderTreeListController extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListController {
    itemsFilter(item, path, model) {
        if (item.specifics === 'none')
            return false;
        return true;
    }
    isDraggable(item) {
        if (!item.data)
            return false;
        if (item.data.isListType || item.data.isSupportQueries === false) {
            let parent = item.parent;
            while (parent && parent.data) {
                if (parent.data.isListType)
                    return false;
                parent = parent.parent;
            }
            return true;
        }
        return false;
    }
    constructor(dragDropHandler, dblClickHandler) {
        super();
        this.dragDropHandler = dragDropHandler;
        this.dblClickHandler = dblClickHandler;
    }
}
class FederatedTransformQueryBuilderTreeListController extends FederatedUnionQueryBuilderTreeListController {
    itemsFilter(item, path, model) {
        return super.itemsFilter(item, path, model) && item.isList;
    }
    hasItems(item) {
        return super.hasItems(item) && (!item.isListType || item.isSupportQueries === false);
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationQueryBuilderPopupBase.js









class FederationQueryBuilderPopupBase extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.PopupEditorBase {
    _aliasValidationCallback(alias, data) {
        return true;
    }
    constructor(onSaveCallback, _dataSource, rtl = false, onCloseCallback) {
        super();
        this.onSaveCallback = onSaveCallback;
        this._dataSource = _dataSource;
        this.rtl = rtl;
        this.onCloseCallback = onCloseCallback;
        this.selectedPath = external_ko_namespaceObject.observable();
        this.title = () => (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Query Builder', 'DataAccessUIStringId.QueryBuilder');
        this.getPopupContainer = external_DevExpress_Analytics_Internal_namespaceObject.getParentContainer;
        this.loaded = external_ko_namespaceObject.observable(false);
        this.maxHeight = '90%';
        this.width = '900px';
        this.height = '710px';
        this.cssClass = 'dxrd-querybuilder-federation-popup-wrapper';
        this.resultGridHeight = external_ko_namespaceObject.observable(250);
        this.dragDropHandler = new external_DevExpress_Analytics_Internal_namespaceObject.WizardDragDropHandler({
            dragHelperContent: new external_DevExpress_Analytics_Internal_namespaceObject.DragHelperContent(null),
            parent: '.dx-designer-viewport .dxrd-querybuilder-federation-popup-wrapper .dxrd-querybuilder-federation-popup',
            containment: 'parent',
            target: this.popupTarget(),
            addHandler: (dropTarget, item, position) => {
                if (dropTarget && dropTarget instanceof FederationQueryBuilderPopupBase || dropTarget instanceof QuerySurface)
                    this.addDataMember(item, position);
            }
        });
        const treeListController = new FederatedUnionQueryBuilderTreeListController(this.dragDropHandler, (item) => treeListController.isDraggable(item) && this.addDataMember(item));
        this.fieldListModel = {
            itemsProvider: this._dataSource.dbSchemaProvider,
            treeListController: treeListController,
            selectedPath: this.selectedPath,
            pageSize: 100,
            expandRootItems: true,
            onItemsChanged: (items) => {
                items.forEach(item => {
                    if (item.parent && treeListController.isDraggable(item.parent) && item.parent.data.isListType)
                        item.parent.collapsed = true;
                });
            },
            rtl: rtl
        };
        this._disposables.push(this.dragDropHandler, treeListController);
        this._aliasValidationRule = {
            type: 'custom',
            validationCallback: (options) => {
                if (!options.value)
                    return true;
                return this._aliasValidationCallback(options.value, options.data);
            },
            get message() { return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias is not unique', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_AliasUniqueMessage'); }
        };
        this.resizeHelper = new external_DevExpress_Analytics_Widgets_Internal_namespaceObject.ResizeHelper();
    }
    dispose() {
        super.dispose();
        this.fieldListModel = null;
        this.onSaveCallback = null;
        this._dataSource = null;
    }
    addDataMember(item, position) { }
    save() {
        this.close();
    }
    close() {
        this.onCloseCallback && this.onCloseCallback();
        this.popupVisible(false);
    }
    popupTarget() {
        return '.dxrd-result-source-grid';
    }
    onHiding() { }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationTablesExpressionFieldListProvider.js



class FederationTablesExpressionFieldListProvider {
    getItems(pathRequest) {
        const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        if (pathRequest.path === '') {
            result.resolve(this.tables().map(table => {
                return { name: table.actualName(), displayName: table.actualName(), isList: true, specifics: 'List', dragData: { noDragable: false } };
            }));
        }
        else {
            const paths = pathRequest.fullPath.split('.');
            paths.shift();
            const table = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.tables(), table => table.actualName() === paths.join('.'));
            if (table) {
                this.provider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(table.pathString)).done(items => result.resolve(items));
            }
        }
        return result.promise();
    }
    constructor(provider, tables) {
        this.provider = provider;
        this.tables = tables;
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationSelectQueryBuilderPopup.js



















class FederationSelectQueryBuilderPopup extends FederationQueryBuilderPopupBase {
    _onContentReady(e) {
        if (!e.component._isFirstLoad) {
            e.component._isFirstLoad = true;
            const element = (0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(e.component.content());
            const contentEl = element.parentElement;
            contentEl.className += '  dx-dropdowneditor-overlay';
        }
    }
    _getQuery() {
        return this.designer() && this.designer().model();
    }
    _afterChangeColumn(colIndex, notifyCallback) {
        this._gridComponent.saveEditData().then(() => {
            this._gridComponent.cancelEditData();
            const event = () => {
                const rows = this._gridComponent.getVisibleRows();
                const index = colIndex != null ? colIndex : rows.length - 1;
                this._gridComponent.editCell(index, 0);
                this._gridComponent.navigateToRow(this.columnsExpressions()[index]);
                this._gridComponent.off('contentReady', event);
            };
            this._gridComponent.on('contentReady', event);
            notifyCallback();
        });
    }
    _changeColumn(columnData, insertIndex) {
        if (columnData) {
            let query = ColumnExpressionCollectionHelper.createNew(this._getQuery(), this._getQuery().columns, columnData.table, columnData.column);
            if (this._isSelectAllItemByKey(columnData.key)) {
                query = columnData.table ?
                    new ColumnExpression({ '@Table': columnData.table, '@ItemType': ColumnType[ColumnType.AllColumns] }, this._getQuery()) :
                    new ColumnExpression({ '@ItemType': ColumnType[ColumnType.AllColumnsQuery] }, this._getQuery());
            }
            const callback = () => insertIndex != null ? this._getQuery().columns.splice(insertIndex, 1, query) : this._getQuery().columns.push(query);
            this._afterChangeColumn(insertIndex, callback);
        }
    }
    _switchEditors(model) {
        let column;
        let callback = () => this._getQuery().columns.notifySubscribers();
        if (model.index != null) {
            column = this._getQuery().columns()[model.index];
            if (model.isExpression()) {
                column.toTable();
            }
            else if (column.itemType() === ColumnType[ColumnType.AllColumns] || column.itemType() === ColumnType[ColumnType.AllColumnsQuery]) {
                column = new ColumnExpression({}, this._getQuery());
                ColumnExpressionCollectionHelper.toExpresson(column, this._getQuery().columns, '');
                this._getQuery().columns.splice(model.index, 1, column);
            }
            else {
                const epressionValue = column.column() ? '[' + column.table() + '.' + column.column() + ']' : '';
                ColumnExpressionCollectionHelper.toExpresson(column, this._getQuery().columns, epressionValue);
            }
        }
        else {
            column = new ColumnExpression({}, this._getQuery());
            ColumnExpressionCollectionHelper.toExpresson(column, this._getQuery().columns, '');
            callback = () => this._getQuery().columns.push(column);
        }
        this._afterChangeColumn(model.index, callback);
    }
    _deleteRow(deleteIndex) {
        this._gridComponent.deleteRow(deleteIndex);
        this._gridComponent.deselectAll();
        this._getQuery().columns.splice(deleteIndex, 1);
    }
    _onRowUpdating(oldData, newData) {
        const column = this._getQuery().columns()[oldData.index];
        if (column) {
            column.alias(newData.alias);
        }
        oldData.alias = newData.alias;
        this._gridComponent.saveEditData();
    }
    _aliasValidationCallback(alias, data) {
        if (alias === data.alias && alias === data.key)
            return true;
        return !ColumnExpressionCollectionHelper.findByName(this._getQuery().columns, alias);
    }
    dispose() {
        super.dispose();
        this._bindingContext = null;
        this._gridComponent = null;
        this._qbOptions = null;
        this.queryBuilderSurfaceCreator = null;
        this.columnsGrid = null;
    }
    _generateKey(tableName, columnName, actualName, alias) {
        return alias === actualName ? actualName : tableName + '_' + columnName;
    }
    _isSelectAllItemByKey(key) {
        return key === FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllColumnsExpression] ||
            key.indexOf(FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllNodeColumnsExpression]) !== -1;
    }
    constructor(onSaveCallback, dataSource, rtl = false, onCloseCallback) {
        super(onSaveCallback, dataSource, rtl, onCloseCallback);
        this.popupContentTemplate = 'dxrd-querybuilder-select-popup-content';
        this._querySource = external_ko_namespaceObject.observable(null);
        this.designer = external_ko_namespaceObject.observable(null);
        this.joinResultCollapsed = external_ko_namespaceObject.observable(false);
        this.width = '90%';
        this.height = '90%';
        this.cssClass = 'dxrd-querybuilder-federation-popup-wrapper dxrd-querybuilder-select-federation-popup';
        this._qbOptions = {
            queryBuilderModel: this.designer,
            dataSource: dataSource,
            dbSchemaProvider: external_ko_namespaceObject.observable(dataSource.dbSchemaProvider),
            showPropertyGridCondition: (model) => model.controlType === 'JoinCondition' || model.controlType === 'FTable' || model.controlType === 'Column',
            querySource: this._querySource,
        };
        this.queryBuilderSurfaceCreator = {
            options: this._qbOptions,
            creator: (options) => {
                return new FederationQueryViewModel(options.querySource(), options.dataSource, options.dbSchemaProvider(), options.parametersMode, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer());
            }
        };
        const rootItems = [{ name: 'DataSource', needPrefix: false, rootPath: 'Root' }];
        this.allColumns = external_ko_namespaceObject.pureComputed(() => {
            const columns = [];
            if (this._getQuery()) {
                let tableName = '';
                columns.push({ column: AllColumnsViewModel.DisplayName(), table: tableName, key: FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllColumnsExpression] });
                const allColumns = this._getQuery().getAllColumns().map(x => {
                    const tableModel = x.parentModel();
                    return {
                        column: x.name(),
                        table: tableModel.actualName(),
                        key: this._generateKey(tableModel.actualName(), x.name(), x.actualName(), x.alias())
                    };
                });
                allColumns.forEach(column => {
                    if (column.table && tableName !== column.table) {
                        columns.push({ column: AllColumnsViewModel.DisplayName(), table: column.table, key: column.table + '_' + FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllNodeColumnsExpression] });
                    }
                    tableName = column.table;
                    columns.push(column);
                });
            }
            return columns;
        });
        this.columnsExpressions = external_ko_namespaceObject.pureComputed(() => this._getQuery() && this._getQuery().columns().map((x, index) => {
            let key;
            let column;
            if (x.actualName()) {
                key = this._generateKey(x.table(), x.column(), x.actualName(), x.alias());
            }
            if (x.itemType() === ColumnType[ColumnType.Column]) {
                column = x.column();
            }
            else if (x.itemType() === ColumnType[ColumnType.Expression]) {
                column = x.expression();
            }
            else if (x.itemType() === ColumnType[ColumnType.AllColumns]) {
                key = x.table() + '_' + FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllNodeColumnsExpression];
                column = AllColumnsViewModel.DisplayName();
            }
            else if (x.itemType() === ColumnType[ColumnType.AllColumnsQuery]) {
                key = FederatedQueryExpressionType[FederatedQueryExpressionType.SelectAllColumnsExpression];
                column = AllColumnsViewModel.DisplayName();
            }
            return {
                alias: x.alias(),
                key: key,
                index: index,
                column: column,
                table: x.table(),
                isExpression: external_ko_namespaceObject.observable(x.itemType() === ColumnType[ColumnType.Expression] ? true : false),
                expression: x.expression
            };
        }));
        this._disposables.push(this.columnsExpressions, this.allColumns);
        const cellTemplateMask = (container, options) => {
            if (options.data.key && this._isSelectAllItemByKey(options.data.key)) {
                return cellTemplate(container, options);
            }
            const templateHtml = (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-querybuilder-column-combobox-masked'), $element = (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(container).append(templateHtml);
            const model = {
                cellInfo: options.data,
                isExpression: options.data.isExpression || external_ko_namespaceObject.observable(false),
                switchEditors: () => null
            };
            external_ko_namespaceObject.applyBindings(model, $element[0]);
        };
        const cellTemplate = (container, options) => {
            let childContext = this._bindingContext.createChildContext({
                allColumns: this.allColumns,
                itemsProvider: new FederationTablesExpressionFieldListProvider(this._dataSource.dbSchemaProvider, this._getQuery().tables),
                cellInfo: options.data,
                expression: { value: options.data.expression, onContentReady: this._onContentReady, path: external_ko_namespaceObject.observable(rootItems[0].name), customizeCategories: () => { }, rootItems, functions: (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.functionDisplay)().filter(cat => cat.category != 'Aggregate') },
                isExpression: options.data.isExpression || external_ko_namespaceObject.observable(false),
                changeColumn: (selectionChangedArgs) => {
                    this._changeColumn(selectionChangedArgs.selectedRowsData[0], selectionChangedArgs.model.cellInfo.index);
                },
                switchEditors: (e) => {
                    this._switchEditors(e.model.cellInfo);
                }
            });
            let child = document.createElement('div');
            container.append(child);
            external_ko_namespaceObject.renderTemplate('dxrd-querybuilder-column-combobox', childContext, {}, child, 'replaceNode');
            (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(container, function () {
                child = null;
                childContext = null;
            });
        };
        this._disposables.push(this.addRowDisabled = external_ko_namespaceObject.pureComputed(() => {
            return !this._getQuery() || this._getQuery().tables().length === 0;
        }));
        this.columnsGrid = {
            rtlEnabled: rtl,
            dataSource: this.columnsExpressions,
            disabled: this.addRowDisabled,
            showRowLines: true,
            height: '100%',
            showBorders: true,
            editing: {
                allowUpdating: (grid) => {
                    return !(grid.row && grid.row.data.key &&
                        this._isSelectAllItemByKey(grid.row.data.key));
                },
                newRowPosition: 'last',
                mode: 'cell',
                texts: {
                    confirmDeleteMessage: '',
                }
            },
            paging: { enabled: false },
            columns: [{
                    dataField: 'column',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Column Name', 'DataAccessUIStringId.QueryBuilderColumns_ColumnName');
                    },
                    cssClass: 'dx-editor-cell',
                    editCellTemplate: cellTemplate,
                    cellTemplate: cellTemplateMask
                }, {
                    dataField: 'table',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Table Name', 'DataAccessUIStringId.QueryBuilderColumns_TableName');
                    },
                    allowEditing: false
                }, {
                    dataField: 'alias',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias', 'DataAccessUIStringId.QueryBuilderColumns_Alias');
                    },
                    disabled: true,
                    validationRules: [this._aliasValidationRule, {
                            type: 'custom',
                            validationCallback: (options) => {
                                if (options.value === '' && options.data && options.data.key) {
                                    const existedColumn = ColumnExpressionCollectionHelper.findByName(this._getQuery().columns, options.data.key);
                                    if (existedColumn && existedColumn.itemType() === ColumnType[ColumnType.Expression])
                                        return false;
                                }
                                return true;
                            },
                            get message() { return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('The column name is not specified.', 'DataAccessStringId.UnnamedColumnValidationException'); }
                        }]
                },
                {
                    type: 'buttons',
                    width: 50,
                    buttons: [
                        {
                            icon: 'delete',
                            onClick: (e) => {
                                this._deleteRow(e.row.data.index != null ? e.row.data.index : this._getQuery().columns().length);
                            },
                            cssClass: 'dxrd-querybuilder-grid-actions dxd-icon-highlighted dxrd-image-recycle-bin',
                            template: (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-svg-operations-recycle_bin'),
                        }
                    ]
                }
            ],
            onInitialized: (e) => {
                this._bindingContext = external_ko_namespaceObject.contextFor((0,external_DevExpress_Analytics_Internal_namespaceObject.$unwrap)(e.element));
                this._gridComponent = e.component;
            },
            onRowUpdating: (event) => {
                this._onRowUpdating(event.oldData, event.newData);
            }
        };
        this.gridResizeHelper = new external_DevExpress_Analytics_Widgets_Internal_namespaceObject.ResizeHelper({
            resultSize: this.resultGridHeight,
            disabled: this.joinResultCollapsed
        });
    }
    save() {
        this.selectQuery.init(this._getQuery().serialize());
        this.onSaveCallback(this.selectQuery);
        this.close();
    }
    addRow() {
        this._gridComponent.addRow();
        this._gridComponent.deselectAll();
    }
    canSave() {
        return this._getQuery() && this._getQuery().isValid();
    }
    addDataMember(item, position) {
        const query = this._getQuery();
        if (query.tables().length > 1 && !query.validateRelations())
            return;
        const name = item.data.displayName || item.data.name;
        const newControl = query.createChild((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({
            '@ControlType': 'FTable',
            '@Name': name
        }, controlsFactory.controlsMap['FTable'].defaultVal), undefined, [...item.pathParts]);
        const controlSurface = external_ko_namespaceObject.unwrap((0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(newControl));
        if (!position) {
            const posX = Math.max.apply(null, query.tables.peek()
                .filter(t => t !== newControl)
                .map((t) => t.location.x.peek() + t.size.width.peek() * 1.25)
                .concat([30]));
            newControl.location.x(posX);
            newControl.location.y(20);
        }
        else {
            const _querySurface = external_ko_namespaceObject.unwrap((0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(this.designer()));
            _querySurface.underCursor().x = position.left - _querySurface['absolutePosition'].x();
            _querySurface.underCursor().y = position.top - _querySurface['absolutePosition'].y();
            controlSurface.rect({ left: _querySurface.underCursor().x, top: _querySurface.underCursor().y, width: 199 });
        }
        this.designer().selection.initialize(controlSurface);
    }
    show(query) {
        this.selectQuery = query;
        this._querySource(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(query));
        this.popupVisible(true);
    }
    popupTarget() {
        return '.dxrd-select-querybuilder-surface .dxqb-main';
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationTransformResultSchemaProvider.js



class TransformResultSchemaProvider {
    constructor(itemsProvider, transformData, currentPath) {
        this.getItems = (path) => {
            const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (!currentPath() || path.fullPath.split('.').length > currentPath().length) {
                return result.resolve([]).promise();
            }
            itemsProvider.getItems(path).done(resultItems => {
                const itemsArray = [];
                const listPath = [];
                resultItems.forEach(item => {
                    const column = transformData().filter(x => x.name == item.name)[0];
                    if ((0,external_DevExpress_Analytics_Internal_namespaceObject.isList)(item) && column && column.transform.value()) {
                        listPath.push(column);
                    }
                    else {
                        item.displayName = (column && column.alias) || item.displayName;
                        itemsArray.push(item);
                    }
                });
                if (listPath.length === 0) {
                    result.resolve(itemsArray);
                }
                else {
                    external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred.when(listPath.map(x => itemsProvider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(path.fullPath + '.' + x.name)).done((subNodes) => {
                        subNodes.forEach(element => {
                            const name = (x.alias || x.column) + '_' + element.displayName;
                            element.displayName = name;
                            element.name = name;
                        });
                        result.resolve(subNodes.concat(itemsArray));
                    })));
                }
            });
            return result.promise();
        };
        this.dispose = () => {
            this.getItems = null;
            itemsProvider = null;
        };
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationTransformQueryBuilderPopup.js











class FederationTransformQueryBuilderPopup extends FederationQueryBuilderPopupBase {
    _updateColumns(columns, path) {
        this.transformSources(columns.map(x => {
            const disabled = !x.isList;
            return {
                columns: columns,
                column: x.displayName,
                name: x.name,
                key: path + '.' + x.name,
                alias: undefined,
                transform: {
                    value: external_ko_namespaceObject.observable(disabled ? undefined : false),
                    disabled: disabled,
                    onValueChanged: () => { this.resultFieldListModel.valueHasMutated(); }
                }
            };
        }));
    }
    _aliasValidationCallback(alias, data) {
        const existedColumn = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.transformSources(), source => source.alias === alias);
        if (existedColumn && existedColumn.key === data.key)
            return true;
        return !existedColumn;
    }
    constructor(onSaveCallback, dataSource, rtl = false, onCloseCallback) {
        super(onSaveCallback, dataSource, rtl, onCloseCallback);
        this.transformGridTitle = external_ko_namespaceObject.observable();
        this.transformResultGridTitle = external_ko_namespaceObject.observable();
        this.transformResultCollapsed = external_ko_namespaceObject.observable(false);
        this.transformSources = external_ko_namespaceObject.observable([]);
        this.currentPath = external_ko_namespaceObject.observable();
        this.transformQuery = external_ko_namespaceObject.observable();
        this.popupContentTemplate = 'dxrd-querybuilder-transform-popup-content';
        this.transformGrid = {
            rtlEnabled: rtl,
            noDataText: () => '',
            dataSource: this.transformSources,
            showRowLines: true,
            paging: { enabled: false },
            scrolling: { mode: 'infinite' },
            editing: {
                allowUpdating: true,
                mode: 'cell',
            },
            height: '100%',
            columns: [{
                    dataField: 'column',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Column Name', 'DataAccessUIStringId.QueryBuilderColumns_ColumnName');
                    },
                    allowEditing: false
                }, {
                    dataField: 'alias',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias', 'DataAccessUIStringId.QueryBuilderColumns_Alias');
                    },
                    validationRules: [this._aliasValidationRule]
                }, {
                    type: 'buttons',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Transform', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_NodeType_Transform');
                    },
                    buttons: [{
                            template: function (element, object) {
                                const child = document.createElement('div');
                                element.append(child);
                                external_ko_namespaceObject.renderTemplate('dxrd-querybuilder-transform-checkbox', object.data, {}, child, 'replaceNode');
                            },
                        }]
                }
            ],
            onRowUpdating: (event) => {
                event.oldData.alias = event.newData.alias;
                this.resultFieldListModel.valueHasMutated();
                this.transformSources.valueHasMutated();
            }
        };
        const transformResultSchemaProvider = new TransformResultSchemaProvider(this._dataSource.dbSchemaProvider, this.transformSources, this.currentPath);
        this.resultFieldListModel = external_ko_namespaceObject.observable({
            itemsProvider: transformResultSchemaProvider,
            path: this.currentPath,
            selectedPath: external_ko_namespaceObject.observable(),
            treeListController: null
        });
        const treeListController = new FederatedTransformQueryBuilderTreeListController(this.dragDropHandler, (item) => treeListController.isDraggable(item) && this.addDataMember(item));
        this.fieldListModel = {
            itemsProvider: this._dataSource.dbSchemaProvider,
            treeListController: treeListController,
            selectedPath: this.selectedPath,
            pageSize: 100,
            expandRootItems: true
        };
        this._disposables.push(this.currentPath.subscribe(path => {
            this.selectedPath(this.currentPathString);
            this.transformGridTitle((0,external_DevExpress_Analytics_Internal_namespaceObject.formatUnicorn)((0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Transformation node root: {0}', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_TransformationNodeRoot'), this._dataSource.getQueryNameFromPath(path)));
        }), treeListController, transformResultSchemaProvider, treeListController);
        this.transformResultGridTitle((0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Transformation result', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_TransformationResult'));
    }
    dispose() {
        super.dispose();
        this.resultFieldListModel = null;
        this.transformGrid = null;
    }
    addDataMember(item) {
        this.currentPath(item.pathParts);
        this.updateColumns();
        this.transformSources.valueHasMutated();
    }
    updateColumns() {
        const path = this.currentPathString;
        this._dataSource.fielListProvider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(path)).done((result) => {
            this._updateColumns(result, path);
        });
    }
    canSave() {
        return this.transformSources().some(source => source.transform.value() || source.alias);
    }
    save() {
        const query = this.transformQuery();
        query.root(new SourceQuery({}, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer(), this._dataSource.getQueryNameFromPath(this.currentPath()), this.currentPath()));
        query.transformationRules([]);
        this.transformSources().forEach(source => {
            if (source.transform.value() || source.alias) {
                query.transformationRules.push(new FederationTransformationRule({
                    '@Name': source.name,
                    '@Alias': source.alias,
                    '@Unfold': source.transform.value(),
                    '@Flatten': source.transform.value()
                }));
            }
        });
        this.onSaveCallback(this.transformQuery());
        this.close();
    }
    onHiding() {
        this.transformQuery(null);
        this.transformResultCollapsed(false);
        this.transformSources([]);
    }
    show(query) {
        this.transformQuery(query);
        if (query.root && query.root()) {
            const path = this._dataSource.getPathFromQueryName(query.root().sourceName());
            this.currentPath(path);
            const currentPathString = this.currentPathString;
            this._dataSource.fielListProvider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(currentPathString)).done((result) => {
                this._updateColumns(result, currentPathString);
                query.transformationRules().forEach(rule => {
                    const column = this.transformSources().filter(x => x.name === rule.name())[0];
                    column.alias = rule.alias();
                    column.transform.value(rule.unfold());
                });
            });
        }
        if (!this.currentPath()) {
            const firstDataSource = this._dataSource.dataSources()[0];
            const firstDataSourceName = firstDataSource.ref || firstDataSource.id;
            if (firstDataSource.isListType) {
                this.currentPath([firstDataSourceName]);
                this.updateColumns();
            }
            else {
                this._dataSource.fielListProvider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(firstDataSourceName)).done(items => {
                    this.currentPath(items[0] && items[0].isListType ? [firstDataSourceName, items[0].name] : [firstDataSourceName]);
                    this.updateColumns();
                });
            }
        }
        this.popupVisible(true);
    }
    get currentPathString() {
        return this.currentPath()?.join('.');
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationUnionQueryBuilderPopup.js







class FederationUnionQueryBuilderPopup extends FederationQueryBuilderPopupBase {
    _aliasValidationCallback(alias, data) {
        const existedColumn = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.columns(), source => source.alias === alias);
        if (existedColumn && existedColumn.key === data.key)
            return true;
        return !existedColumn;
    }
    constructor(onSaveCallback, dataSource, rtl = false, onCloseCallback) {
        super(onSaveCallback, dataSource, rtl, onCloseCallback);
        this.unionQuery = external_ko_namespaceObject.observable();
        this.unionAll = external_ko_namespaceObject.observable(false);
        this.columns = external_ko_namespaceObject.observableArray();
        this.popupContentTemplate = 'dxrd-querybuilder-union-popup-content';
        this.buttonItems.push({
            toolbar: 'bottom', location: 'before', widget: 'dxCheckBox', options: { text: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Union All', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_NodeType_UnionAll'), value: this.unionAll }
        });
        const sources = external_ko_namespaceObject.pureComputed(() => this.unionQuery() && this.unionQuery().queries().map(x => {
            const alias = x.alias.peek();
            return {
                source: alias.split('_').join(' - '),
                name: alias
            };
        }));
        this._disposables.push(sources);
        this.sourcesGrid = {
            rtlEnabled: rtl,
            noDataText: () => (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Drop a table or view here to create a query.', 'AnalyticsCoreStringId.QueryBuilder_SurfacePlaceholder'),
            dataSource: sources,
            showRowLines: true,
            paging: { enabled: false },
            scrolling: { mode: 'infinite' },
            columns: [{
                    dataField: 'source',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Source', 'DataAccessUIStringId.QueryBuilderColumns_Source');
                    },
                }, {
                    type: 'buttons',
                    width: 30,
                    buttons: [{
                            icon: 'delete',
                            onClick: (e) => {
                                this.unionQuery().removeQuery(e.row.data.name);
                                this.unionQuery.valueHasMutated();
                            },
                            cssClass: 'dxrd-image-recycle-bin',
                            template: (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-svg-operations-recycle_bin'),
                        }]
                }
            ]
        };
        this._disposables.push(this.unionQuery.subscribe((unionQuery) => {
            const expressionArrays = [];
            const resultExpressions = [];
            this.columns([]);
            if (!unionQuery || unionQuery.queries().length === 0) {
                return;
            }
            unionQuery.queries().forEach(query => expressionArrays.push(query.expressions()));
            expressionArrays[0] && expressionArrays[0].forEach(expression => {
                if (expressionArrays.every(array => !!array.filter(x => x.name() === expression.name())[0])) {
                    resultExpressions.push({
                        name: expression.name(),
                        alias: expression.alias(),
                        key: expression.table() + '.' + expression.name()
                    });
                }
            });
            this.columns(resultExpressions);
        }));
        this._disposables.push(this.unionAll.subscribe((value) => {
            if (!this.unionQuery())
                return;
            if (value) {
                this.unionQuery().unionType(UnionTypes[UnionTypes.UnionAll]);
            }
            else {
                this.unionQuery().unionType(UnionTypes[UnionTypes.Union]);
            }
        }));
        this.aliasGrid = {
            rtlEnabled: rtl,
            dataSource: this.columns,
            showRowLines: true,
            editing: {
                allowUpdating: true,
                mode: 'cell',
            },
            paging: { enabled: false },
            scrolling: { mode: 'infinite' },
            height: '100%',
            columns: [{
                    dataField: 'name',
                    caption: 'Column Name',
                    allowEditing: false
                }, {
                    dataField: 'alias',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Alias', 'DataAccessUIStringId.QueryBuilderColumns_Alias');
                    },
                    validationRules: [this._aliasValidationRule]
                }],
            onRowUpdating: (event) => {
                event.oldData.alias = event.newData.alias;
            }
        };
    }
    dispose() {
        super.dispose();
        this.sourcesGrid = null;
        this.aliasGrid = null;
    }
    addDataMember(item) {
        this._dataSource.fielListProvider.getItems(new external_DevExpress_Analytics_Utils_namespaceObject.PathRequest(item.path)).done((result) => {
            result.forEach(x => this.unionQuery().addSelectQuery(item.pathParts, x.name));
            this.unionQuery.valueHasMutated();
        });
    }
    canSave() {
        return this.unionQuery() && this.unionQuery().queries().length > 0;
    }
    save() {
        this.unionQuery().queries().forEach(query => {
            query.expressions(query.expressions().filter(expression => {
                const column = this.columns().filter(column => column.name === expression.name())[0];
                if (column) {
                    expression.alias(column.alias);
                    return true;
                }
                return false;
            }));
        });
        this.onSaveCallback(this.unionQuery());
        this.close();
    }
    onHiding() {
        this.unionQuery(null);
        this.unionAll(false);
    }
    show(query) {
        this.unionQuery(query);
        this.popupVisible(true);
        this.unionAll(query.unionType() === UnionTypes[UnionTypes.UnionAll]);
    }
}

;// ./dist/query-builder/widgets/_federatedQueriesHelper.js










class FederatedQueriesHelper extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    _getQuery(creator, name) {
        return name ? (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this.queries(), item => name === item.alias()) : creator();
    }
    _setQuery(query) {
        if (!query)
            return;
        if (this.queries.indexOf(query) === -1) {
            query.alias(query.alias() || query.generateName());
            if (this._dataSource.queries().filter(x => x.alias() === query.alias())[0]) {
                query.alias((0,external_DevExpress_Analytics_Internal_namespaceObject.getUniqueName)(this._dataSource.queries().map(x => x.alias()), query.alias()));
            }
            this.queries.push(query);
            this._afterAddQuery && this._afterAddQuery(query);
        }
    }
    constructor(_dataSource, queries, callbacks, rtl = false) {
        super();
        this._dataSource = _dataSource;
        this.queries = queries;
        this._showSelectQbCallBack = (name = null) => {
            this._popupSelectQueryBuilder.show(this._getQuery(() => new SelectQuery({}), name));
        };
        this._showUnionQbCallBack = (name = null) => {
            this._popupUnionQueryBuilder.show(this._getQuery(() => new UnionQuery({}, this._dataSource.dataSources), name));
        };
        this._showTransformQbCallBack = (name = null) => {
            this._popupTransformQueryBuilder.show(this._getQuery(() => new TransformQuery({}), name));
        };
        this.template = 'dx-querybuilder-federation-popup-templates';
        const saveCallback = (query) => {
            this._setQuery(query);
            queries.valueHasMutated();
            callbacks.onSave && callbacks.onSave();
        };
        this._afterAddQuery = callbacks.afterAddQuery;
        this._disposables.push(queries.subscribe(queries => queries.forEach(query => {
            query.sources().forEach(x => this._dataSource.addSource(x));
        })));
        this._disposables.push(this._popupSelectQueryBuilder = new FederationSelectQueryBuilderPopup(saveCallback, this._dataSource, rtl, callbacks.onClose));
        this._disposables.push(this._popupUnionQueryBuilder = new FederationUnionQueryBuilderPopup(saveCallback, this._dataSource, rtl, callbacks.onClose));
        this._disposables.push(this._popupTransformQueryBuilder = new FederationTransformQueryBuilderPopup(saveCallback, this._dataSource, rtl, callbacks.onClose));
        this.callBacks = {
            joinCallBack: this._showSelectQbCallBack,
            unionCallBack: this._showUnionQbCallBack,
            transformCallBack: this._showTransformQbCallBack
        };
        this.popupItems = [
            { template: 'dxrd-querybuilder-federation-popup', model: this._popupSelectQueryBuilder },
            { template: 'dxrd-querybuilder-federation-popup', model: this._popupUnionQueryBuilder },
            { template: 'dxrd-querybuilder-federation-popup', model: this._popupTransformQueryBuilder }
        ];
    }
    editQuery(type, name) {
        switch (type) {
            case FederationQueryType.SelectNode:
                this.callBacks.joinCallBack(name);
                break;
            case FederationQueryType.UnionNode:
                this.callBacks.unionCallBack(name);
                break;
            case FederationQueryType.TransformationNode:
                this.callBacks.transformCallBack(name);
                break;
        }
    }
    dispose() {
        super.dispose();
        this._dataSource = null;
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationDataSourceItemsExtender.js
class FederationDataSourceItemsExtender {
    constructor(_rootItems) {
        this._rootItems = _rootItems;
    }
    afterItemsFilled(request, items) {
        items.forEach(x => {
            if (x.isListType === undefined) {
                x.isListType = x.specifics === 'List';
            }
        });
    }
    beforeItemsFilled(request, items) {
        if (!request.fullPath) {
            items.push(...this._rootItems().map(item => {
                return {
                    name: item.id || item.ref,
                    displayName: item.name,
                    isList: true,
                    specifics: item.specifics || 'ListSource',
                    isSupportQueries: item.isSupportQueries,
                    isListType: item.isListType
                };
            }));
            return true;
        }
        return false;
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federatedQueriesTreeNode.js


class FederatedQueriesTreeNode extends QueriesTreeNode {
    constructor(name, displayName, specifics, isChecked, callbacks, afterCheckToggled) {
        super(name, displayName, specifics, isChecked, callbacks, afterCheckToggled);
        this.callbacks = callbacks;
        this.addAction = {
            clickAction: () => {
                return this.showPopover();
            },
            imageClassName: 'dxrd-image-add',
            imageTemplateName: 'dxrd-svg-operations-add',
            templateName: 'dx-treelist-federation-action-with-popover',
            text: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Add query', 'AnalyticsCoreStringId.SqlDSWizard_AddQuery')
        };
        this.className = 'dxrd-federation-addqueries-popover';
    }
    getActions(context) {
        const result = [];
        if (context.path.indexOf('queries') === 0) {
            result.push(this.addAction);
        }
        return result;
    }
    popoverListItems() {
        return [
            {
                name: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Join', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_NodeType_Select'),
                addAction: () => this.callbacks().showQbCallBacks.joinCallBack(),
                className: this.className
            },
            {
                name: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Union', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_NodeType_Union'),
                addAction: () => this.callbacks().showQbCallBacks.unionCallBack(),
                className: this.className
            },
            {
                name: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Transform', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_NodeType_Transform'),
                addAction: () => this.callbacks().showQbCallBacks.transformCallBack(),
                className: this.className
            }
        ];
    }
}

;// ./dist/query-builder/wizard/internal/federationDataSource/_federationTreeNodeProvider.js










class FederationTreeNodeProvider extends TreeNodeItemsProvider {
    constructor(fieldListProvider, rootItems, callBacks, customQueries, afterCheckToggled) {
        super(fieldListProvider, rootItems, (item, isChecked, pathParts) => new FederationDataMemberTreeNode(item.name, item.displayName, item.specifics, item.isListType, isChecked, pathParts, afterCheckToggled), (item, isChecked, pathParts) => new FieldTreeNode(item.name, item.displayName, item.specifics, isChecked, pathParts, afterCheckToggled));
        this._callBack = external_ko_namespaceObject.observable({
            deleteAction: (name) => {
                this._customQueries
                    .remove((0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._customQueries(), item => (item.alias() || item.generateName()) === name));
            },
            disableCustomSql: false,
            showQbCallBack: null,
            showQbCallBacks: null
        });
        const getCurrentItems = this.getItems;
        this._customQueries = customQueries;
        this.getItems = (pathRequest) => {
            const result = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
            if (pathRequest.pathParts && pathRequest.pathParts.length > 2) {
                result.resolve([]);
            }
            if (pathRequest.fullPath === 'queries') {
                const queries = customQueries().map(query => {
                    const name = query.alias() || query.generateName();
                    const currentQuery = this._queries.children().filter(q => q.query === query)[0];
                    let callback;
                    if (currentQuery)
                        return currentQuery;
                    if (query instanceof UnionQuery) {
                        callback = external_ko_namespaceObject.observable((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(this._callBack(), { showQbCallBack: callBacks.unionCallBack }));
                    }
                    else if (query instanceof TransformQuery) {
                        callback = external_ko_namespaceObject.observable((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(this._callBack(), { showQbCallBack: callBacks.transformCallBack }));
                    }
                    else {
                        callback = external_ko_namespaceObject.observable((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(this._callBack(), { showQbCallBack: callBacks.joinCallBack }));
                    }
                    const queryNode = new TreeQueryNode(name, name, 'query', !!currentQuery && currentQuery.checked(), external_ko_namespaceObject.observableArray([]), callback, afterCheckToggled, query, ['queries', name]);
                    this._disposables.push(queryNode);
                    return queryNode;
                });
                this._queries.initializeChildren(queries);
                result.resolve(queries);
            }
            else {
                getCurrentItems(pathRequest, true).done(value => result.resolve(value));
            }
            return result.promise();
        };
        this._callBack().showQbCallBacks = callBacks;
        this._disposables.push(this._queries = new FederatedQueriesTreeNode('queries', (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Federated Queries', 'DataAccessUIStringId.FederatedQueryCollectionList_Title'), 'list', false, this._callBack, afterCheckToggled));
        rootItems.valueHasMutated();
        this._rootItems.push(this._queries);
        this.dispose = () => {
            super.dispose();
            this.removeProperties();
        };
    }
    isList(dataMember, parentNode) {
        return !parentNode.isListType && super.isList(dataMember, parentNode);
    }
}
class FederationDataMemberTreeNode extends DataMemberTreeNode {
    constructor(name, displayName, specifics, isListType, isChecked, pathParts, afterCheckToggled) {
        super(name, displayName, specifics, isChecked, pathParts, afterCheckToggled);
        this.isListType = isListType;
    }
}

;// ./dist/query-builder/wizard/pages/federationDataSourceWizard/federatedQueryConfigurePage.js
















class FederatedQueryConfigurePage extends WizardPageBase {
    _wrapFieldListCallback(itemsCallback, useCache) {
        return (pathRequest) => {
            const _dataSource = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._dataSources(), x => {
                return x.id != null && pathRequest.id != null && x.id === pathRequest.id ||
                    x.ref != null && pathRequest.id != null && x.ref === pathRequest.ref;
            });
            if (_dataSource)
                return itemsCallback(pathRequest, _dataSource, useCache);
            else
                return itemsCallback(pathRequest, undefined, useCache);
        };
    }
    constructor(_options) {
        super();
        this._options = _options;
        this._selectedPath = external_ko_namespaceObject.observable(null);
        this._itemsProvider = external_ko_namespaceObject.observable();
        this._customQueries = external_ko_namespaceObject.observableArray([]);
        this._setQueryChecked = (query) => {
            const rootItems = this._itemsProvider().getRootItems();
            this._selectedPath('queries.' + query.alias());
            const queries = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(rootItems, (data) => data.name === 'queries');
            const children = queries.children();
            const child = children[children.length - 1];
            child.setChecked(true);
            child['_afterCheckToggled'] && child['_afterCheckToggled'](child);
        };
        this._dataSources = external_ko_namespaceObject.observableArray();
        this._scrollViewHeight = 'calc(100% - 37px)';
        this._isDataLoadingInProcess = external_ko_namespaceObject.observable(false);
        this._customizeDBSchemaTreeListActions = null;
        this._dataSources(this._options.dataSources());
        this._disposables.push(this._fieldListProvider = new external_DevExpress_Analytics_Internal_namespaceObject.FieldListProvider(this._wrapFieldListCallback(this._options.callbacks.fieldListsCallback, false), this._dataSources, [
            new FederationDataSourceItemsExtender(this._dataSources)
        ], true));
        this._disposables.push(this._dataSource = new FederationDataSource({}, this._dataSources, this._fieldListProvider, new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer()));
        this._disposables.push(this._queriesPopupHelper = new FederatedQueriesHelper(this._dataSource, this._customQueries, { afterAddQuery: this._setQueryChecked }, this._options.rtl));
        this._fieldListModel = external_ko_namespaceObject.observable(null);
        this._getItemsAfterCheck = (node) => {
            const pathParts = [...node.pathParts];
            if (node.specifics == 'query') {
                const query = node['query'] || (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._customQueries(), (data) => (data.alias() || query.generateName()) === node.name);
                query.alias(node.name);
                if (node.checked.peek()) {
                    this._dataSource.queries.indexOf(query) === -1 && this._dataSource.queries.push(query);
                }
                else {
                    this._dataSource.queries.remove(query);
                }
            }
            else if (!node.isList || pathParts.length > 2) {
                const columnName = node.name;
                pathParts.pop();
                node.checked.peek() ? this._dataSource.addSelectQuery(pathParts, columnName) : this._dataSource.removeExpression(columnName, pathParts);
            }
            else {
                node.children().forEach(x => this._getItemsAfterCheck(x));
            }
            this._onChange();
            this[__loadingStateFunctionName] && this[__loadingStateFunctionName](false);
        };
    }
    canNext() {
        return this._dataSource.queries().length > 1;
    }
    canFinish() {
        return this._dataSource.queries().length > 0;
    }
    _createTreeListFactory() {
        return new MultiQueryTreeListItemFactory();
    }
    _loadPanelViewModel(element) {
        return PopupWizard._getLoadPanelViewModel(element, this._isDataLoadingInProcess);
    }
    commit() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            federationDataSourceJSON: JSON.stringify(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(this._dataSource)),
            federatedQueries: []
        }).promise();
    }
    initialize(state) {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        const rootItems = external_ko_namespaceObject.observableArray((this._dataSources() || []).map((item) => {
            return {
                name: item.id || item.ref,
                displayName: item.name,
                isList: true,
                specifics: item.specifics || 'ListSource',
                isListType: item.isListType,
                dragData: { noDragable: false }
            };
        }));
        const itemsProvider = new FederationTreeNodeProvider(this._fieldListProvider, rootItems, this._queriesPopupHelper.callBacks, this._customQueries, this._getItemsAfterCheck);
        this._disposables.push(itemsProvider);
        this._itemsProvider(itemsProvider);
        this._fieldListModel({
            factory: this._createTreeListFactory(),
            itemsProvider: this._itemsProvider(),
            selectedPath: this._selectedPath,
            treeListController: new DBSchemaTreeListController(this._customizeDBSchemaTreeListActions),
            templateName: 'dxrd-treelist-with-checkbox'
        });
        return deferred.resolve().promise();
    }
}
function _registerFederatedQueryConfigurePage(factory, wizardOptions) {
    factory.registerMetadata(FederationDataSourceWizardPageId.FederatedQueryConfigurePage, {
        create: () => {
            return new FederatedQueryConfigurePage(wizardOptions);
        },
        setState: (data, state) => {
            state.federationDataSourceJSON = data.federationDataSourceJSON;
            state.federatedQueries = data.federatedQueries;
        },
        getState: (state) => {
            return state.federationDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.federationDataSourceJSON = defaultState.federationDataSourceJSON;
            state.federatedQueries = defaultState.federatedQueries;
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Columns selected from specific tables and/or views will be automatically included into a separate query.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMultiQuery'),
        template: 'dxrd-wizard-add-federated-queries-page'
    });
}

;// ./dist/query-builder/wizard/pages/federationDataSourceWizard/federatedMasterDetailRelationshipsPage.js








class FederatedMasterDetailRelationshipsPage extends MasterDetailRelationshipsPageBase {
    _restoreDataSource(state) {
        this._federationDataSource = _restoreFederationDataSourceFromState(state, this._options.dataSources);
    }
    _dataSource() {
        return this._federationDataSource;
    }
    constructor(federationDataSourceResultSchema, _options) {
        super(federationDataSourceResultSchema);
        this._options = _options;
        this._customResetOptions = () => void (0);
        this._relationsEditor = external_ko_namespaceObject.observable(null);
    }
    commit() {
        this.relationsSubscription.dispose();
        const relations = this._relations().map(x => FederationMasterDetailRelation.create(x));
        this._federationDataSource.relations(relations);
        this._federationDataSource.resultSet = this._resultSet;
        const serializer = new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer();
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            federationDataSourceJSON: JSON.stringify(new external_DevExpress_Analytics_Utils_namespaceObject.ModelSerializer().serialize(this._federationDataSource)),
            relations: relations.map(x => JSON.stringify(serializer.serialize(x))),
        }).promise();
    }
}
function _registerFederatedMasterDetailRelationshipsPage(factory, federationDataSourceResultSchema, wizardOptions) {
    factory.registerMetadata(FederationDataSourceWizardPageId.FederatedMasterDetailRelationshipsPage, {
        create: () => {
            return new FederatedMasterDetailRelationshipsPage(federationDataSourceResultSchema, wizardOptions);
        },
        setState: (data, state) => {
            state.relations = data.relations;
        },
        getState: (state) => {
            return state.federationDataSourceWizard;
        },
        resetState: (state, defaultState) => {
            state.relations = defaultState.relations;
        },
        description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure master-detail relationships.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMasterDetailRelations'),
        template: 'dxrd-wizard-configure-relations-page'
    });
}

;// ./dist/query-builder/wizard/pageFactory.js
class PageFactory {
    constructor() {
        this.metadata = {};
    }
    registerMetadata(pageId, metadata) {
        if (!metadata.canFinish)
            metadata.canFinish = (page) => page.canFinish();
        if (!metadata.canNext)
            metadata.canNext = (page) => page.canNext();
        this.metadata[pageId] = metadata;
    }
    getMetadata(pageId) {
        return this.metadata[pageId];
    }
    unregisterMetadata(pageId) {
        delete this.metadata[pageId];
    }
    reset() {
        this.metadata = {};
    }
}

;// ./dist/query-builder/wizard/dataSourceWizard.js



















class _DataSourceWizardOptionsBase {
    constructor() {
        this.connectionStrings = {
            json: external_ko_namespaceObject.observableArray([]),
            mongodb: external_ko_namespaceObject.observableArray([]),
            sql: external_ko_namespaceObject.observableArray([])
        };
        this.callbacks = {};
        this.rtl = false;
        this.disableCustomSql = false;
        this.wizardSettings = new DataSourceWizardSettings().createDefault();
        this.allowCreateNewJsonConnection = false;
        this.predefinedDataSources = external_ko_namespaceObject.observable([]);
    }
    get jsonDataSourceAvailable() {
        return this.wizardSettings.enableJsonDataSource && (this.allowCreateNewJsonConnection || (external_ko_namespaceObject.unwrap(this.connectionStrings.json) || []).length > 0);
    }
    get mongoDBDataSourceAvailable() {
        return this.wizardSettings.enableMongoDBDataSource && (external_ko_namespaceObject.unwrap(this.connectionStrings.mongodb) || []).length > 0;
    }
    get sqlDataSourceAvailable() {
        return this.wizardSettings.enableSqlDataSource && (external_ko_namespaceObject.unwrap(this.connectionStrings.sql) || []).length > 0;
    }
    get objectDataSourceAvailable() {
        return this.wizardSettings.enableObjectDataSource;
    }
    get canCreateDataSource() {
        return this.jsonDataSourceAvailable || this.sqlDataSourceAvailable || this.objectDataSourceAvailable || this.mongoDBDataSourceAvailable;
    }
    get canRunWizard() {
        return this.canCreateDataSource || external_ko_namespaceObject.unwrap(this.predefinedDataSources).length > 0;
    }
    get federationDataSourceAvailable() {
        return this.wizardSettings.enableFederationDataSource && this.dataSources && this.dataSources() && this.dataSources().length > 0;
    }
}
class _DataSourceWizardOptions extends _DataSourceWizardOptionsBase {
}
class DataSourceWizardSettings {
    createDefault(settings) {
        return {
            enableJsonDataSource: settings?.enableJsonDataSource ?? true,
            enableMongoDBDataSource: settings?.enableMongoDBDataSource || true,
            enableSqlDataSource: settings?.enableSqlDataSource ?? true,
            enableObjectDataSource: settings?.enableObjectDataSource ?? true,
            enableFederationDataSource: settings?.enableFederationDataSource ?? true
        };
    }
}
class DataSourceWizardPageIterator extends PageIterator {
    constructor(pageFactory, stateManager, _dataSourceWizardOptions) {
        super(pageFactory, stateManager);
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
    }
    getNextPageId(pageId) {
        if (!pageId && _isMoreThanOneDataSourceTypeAvailable(this._dataSourceWizardOptions)) {
            return DataSourceWizardPageId.ChooseDataSourceTypePage;
        }
        else if (!pageId) {
            return this.getNextPageId(DataSourceWizardPageId.ChooseDataSourceTypePage);
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Sql) {
            return SqlDataSourceWizardPageId.ChooseConnectionPage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Federation) {
            return FederationDataSourceWizardPageId.FederatedQueryConfigurePage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Json && this._dataSourceWizardOptions.connectionStrings.json().length > 0) {
            return JsonDataSourceWizardPageId.ChooseConnectionPage;
        }
        else if (this._dataSourceWizardOptions.allowCreateNewJsonConnection && pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Json) {
            return JsonDataSourceWizardPageId.ChooseJsonSourcePage;
        }
        else if (pageId === JsonDataSourceWizardPageId.ChooseConnectionPage && this._getCurrentState().jsonDataSourceWizard.connectionName) {
            return JsonDataSourceWizardPageId.ChooseJsonSchemaPage;
        }
        else if (this._dataSourceWizardOptions.allowCreateNewJsonConnection && pageId === JsonDataSourceWizardPageId.ChooseConnectionPage && !this._getCurrentState().jsonDataSourceWizard.connectionName) {
            return JsonDataSourceWizardPageId.ChooseJsonSourcePage;
        }
        else if (pageId === JsonDataSourceWizardPageId.ChooseJsonSourcePage) {
            return JsonDataSourceWizardPageId.ChooseJsonSchemaPage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.MongoDB) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage;
        }
        else if (pageId === MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage;
        }
        else if (pageId === MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBSchemaPage;
        }
        else if (pageId === SqlDataSourceWizardPageId.ChooseConnectionPage && this._getCurrentState().sqlDataSourceWizard.name) {
            return SqlDataSourceWizardPageId.ConfigureQueryPage;
        }
        else if (pageId === SqlDataSourceWizardPageId.ConfigureQueryPage && this._getCurrentState().sqlDataSourceWizard.sqlDataSourceJSON) {
            return SqlDataSourceWizardPageId.ConfigureParametersPage;
        }
    }
}
class DataSourceWizard extends PopupWizard {
    constructor(pageFactory, _wizardOptions) {
        super(pageFactory, _wizardOptions.callbacks.finishCallback);
        this._wizardOptions = _wizardOptions;
        this._extendCssClass = 'dxrd-sqldatasource-wizard';
        this.title = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Data Source Wizard', 'AnalyticsCoreStringId.DSWizard_Title');
    }
    initialize(state, createIterator = (pageFactory, stateManager) => new DataSourceWizardPageIterator(pageFactory, stateManager, this._wizardOptions)) {
        if (this._wizardOptions.sqlDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.sqlDataSourceWizard)) {
            state.dataSourceType = DataSourceType.Sql;
        }
        else if (this._wizardOptions.jsonDataSourceAvailable || state.jsonDataSourceWizard.jsonSource) {
            state.dataSourceType = DataSourceType.Json;
        }
        else if (this._wizardOptions.mongoDBDataSourceAvailable) {
            state.dataSourceType = DataSourceType.MongoDB;
        }
        else if (this._wizardOptions.federationDataSourceAvailable) {
            state.dataSourceType = DataSourceType.Federation;
        }
        super.initialize(state, createIterator);
    }
    canRunWizard() {
        return this._wizardOptions.jsonDataSourceAvailable || this._wizardOptions.sqlDataSourceAvailable || this._wizardOptions.mongoDBDataSourceAvailable;
    }
}
function _registerDataSourceWizardPages(factory = new PageFactory(), dataSourceWizardOptions) {
    _registerChooseDataSourceTypePage(factory, dataSourceWizardOptions);
    _registerChooseJsonSourcePage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.callbacks.getItemsProviderCallback);
    _registerChooseJsonConnectionPage(factory, dataSourceWizardOptions);
    _registerChooseJsonSchemaPage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.callbacks);
    _registerChooseMongoDBDataConnectionPage(factory, dataSourceWizardOptions);
    _registerChooseMongoDBCollectionPage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.rtl);
    _registerChooseMongoDBSchemaPage(factory, dataSourceWizardOptions.requestWrapper);
    _registerConfigureObjectDataSourceParametersPage(factory, dataSourceWizardOptions.callbacks.getItemsProviderCallback);
    _registerConfigureQueryPage(factory, dataSourceWizardOptions);
    _registerChooseSqlConnectionPage(factory, dataSourceWizardOptions.connectionStrings.sql);
    _registerConfigureParametersPage(factory, dataSourceWizardOptions.requestWrapper);
    return factory;
}
function _createDataSourceWizard(factory = new PageFactory(), dataSourceWizardOptions) {
    factory = _registerDataSourceWizardPages(factory, dataSourceWizardOptions);
    return new DataSourceWizard(factory, dataSourceWizardOptions);
}

;// ./dist/query-builder/wizard/fullscreen/pages/specifyObjectDataSourceSettingsPage.js












class SpecifyObjectDataSourceSettingsPage extends WizardPageBase {
    _initSections() {
        this._sections = [
            {
                data: this._chooseObjectType,
                disabled: () => false,
                description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose the type and its constructor.', 'AnalyticsCoreStringId.ObjectDSWizard_ChooseType_Description'),
                position: getSectionStyle(this._dataSourceWizardOptions.rtl ? WizardSectionPosition.Right : WizardSectionPosition.Left),
                template: 'dx-objectdatasource-types-section'
            },
            {
                data: this._chooseObjectDataMember,
                disabled: () => !this._chooseObjectDataMember.dataMembers().length,
                description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose the entire object or a data member to bind.', 'AnalyticsCoreStringId.ObjectDSWizard_ChooseDataMember_Description'),
                position: getSectionStyle(this._dataSourceWizardOptions.rtl ? WizardSectionPosition.TopLeft : WizardSectionPosition.TopRight),
                disabledText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To select a data member, choose a type that contains at least one data member.', 'AnalyticsCoreStringId.ObjectDSWizard_ChooseDataMember_Disabled_Description'),
                template: 'dx-objectdatasource-datamembers-section'
            },
            {
                data: this._chooseObjectParameters,
                disabled: () => !this._chooseObjectParameters.hasParameters(),
                position: getSectionStyle(this._dataSourceWizardOptions.rtl ? WizardSectionPosition.BottomLeft : WizardSectionPosition.BottomRight),
                description: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure constructor parameters and/or method parameters.', 'AnalyticsCoreStringId.ObjectDSWizard_ConfigureParameters_Description'),
                disabledText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To specify parameters, select a parameterized constructor or method.', 'AnalyticsCoreStringId.ObjectDSWizard_ConfigureParameters_Disabled_Description'),
                template: 'dx-objectdatasource-parameters-section'
            }
        ];
    }
    showDescription(index, text) {
        return [index + 1, text].join('. ');
    }
    constructor(_dataSourceWizardOptions) {
        super();
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
        this._types = external_ko_namespaceObject.observableArray([]);
        this._objectDataSource = new ObjectDataSource();
        const getItemsProviderCallback = this._dataSourceWizardOptions.callbacks.getItemsProviderCallback;
        this._disposables.push(this._provider = new ObjectSchemaProvider(this._dataSourceWizardOptions.requestWrapper), this._chooseObjectType = new ChooseObjectTypes(this._types, this._provider), this._chooseObjectDataMember = new ChooseObjectDataMembers(this._chooseObjectType.selectedType, this._chooseObjectType.selectedCtor), this._chooseObjectParameters = new ChooseObjectParameters(this._chooseObjectType.selectedCtor, this._chooseObjectDataMember.selectedDataMembers, getItemsProviderCallback && getItemsProviderCallback()));
        this._initSections();
    }
    canNext() {
        return false;
    }
    canFinish() {
        return !!this._chooseObjectDataMember.selectedDataMembers().length;
    }
    commit() {
        const selectedDataMember = this._chooseObjectDataMember.selectedDataMembers()[0];
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve({
            selectedType: this._chooseObjectType.selectedType().name,
            ctor: this._chooseObjectType.selectedCtor(),
            dataSourceName: this._objectDataSource.name(),
            context: this._context,
            dataMember: !selectedDataMember || selectedDataMember.isEntireObject() ? undefined : selectedDataMember,
        }).promise();
    }
    initialize(state) {
        this._context = state.objectDataSourceWizard.context;
        this._objectDataSource.setState(state.objectDataSourceWizard);
        this._chooseObjectType.selectedCtor(this._objectDataSource.ctor);
        this._chooseObjectDataMember.selectedDataMembers([this._objectDataSource.dataMember]);
        this._provider.getObjectTypeDescriptions(state.objectDataSourceWizard.context).done((schema) => {
            schema.types.forEach(type => type.members.splice(0, 0, ObjectDataMember.empty()));
            this._types(schema.types || []);
            if (schema.types.length > 0) {
                this._chooseObjectType.selectedPath(schema.types[0].name);
            }
        });
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
}
function _registerSpecifyObjectDataSourceSettingsPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SpecifyObjectDataSourceSettingsPage, {
        setState: (data, state) => {
            state.objectDataSourceWizard.ctor = data.ctor;
            state.objectDataSourceWizard.dataMember = data.dataMember;
            state.objectDataSourceWizard.selectedType = data.selectedType;
            state.objectDataSourceWizard.dataSourceName = data.dataSourceName;
        },
        getState: (state) => {
            return state;
        },
        resetState: (state, defaultState) => {
            state.objectDataSourceWizard.ctor = defaultState.objectDataSourceWizard.ctor;
            state.objectDataSourceWizard.dataMember = defaultState.objectDataSourceWizard.dataMember;
            state.objectDataSourceWizard.selectedType = defaultState.objectDataSourceWizard.selectedType;
            state.objectDataSourceWizard.dataSourceName = defaultState.objectDataSourceWizard.dataSourceName;
        },
        create: () => {
            return new SpecifyObjectDataSourceSettingsPage(dataSourceWizardOptions);
        },
        template: 'dxrd-page-objectsource',
        navigationPanelText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Specify Data Source Settings', 'AnalyticsCoreStringId.Wizard_SpecifyDataSourceSettingsPage')
    });
}

;// ./dist/query-builder/wizard/multiQueryDataSourceWizard.js




























class _MultiQueryDataSourceWizardOptions extends _DataSourceWizardOptionsBase {
}
class MultiQueryDataSourceWizard extends PopupWizard {
    constructor(pageFactory, _wizardOptions) {
        super(pageFactory, _wizardOptions.callbacks.finishCallback);
        this._wizardOptions = _wizardOptions;
        this.title = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Data Source Wizard', 'AnalyticsCoreStringId.SqlDSWizard_Title');
        this._extendCssClass = 'dxrd-multiqueries-sqldatasource-wizard';
    }
    canRunWizard() {
        return this._wizardOptions.canRunWizard;
    }
    initialize(state, createIterator = (pageFactory, stateManager) => new MultiQueryDataSourceWizardPageIterator(pageFactory, stateManager, this._wizardOptions)) {
        if (this._wizardOptions.sqlDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.sqlDataSourceWizard)) {
            state.dataSourceType = DataSourceType.Sql;
        }
        else if (this._wizardOptions.jsonDataSourceAvailable || state.jsonDataSourceWizard.jsonSource) {
            state.dataSourceType = DataSourceType.Json;
        }
        else if (this._wizardOptions.mongoDBDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.mongoDBDataSourceWizard)) {
            state.dataSourceType = DataSourceType.MongoDB;
        }
        else if (this._wizardOptions.objectDataSourceAvailable) {
            state.dataSourceType = DataSourceType.Object;
        }
        super.initialize(state, createIterator);
    }
}
class MultiQueryDataSourceWizardPageIterator extends PageIterator {
    constructor(pagesFactory, stateManager, _wizardOptions) {
        super(pagesFactory, stateManager);
        this._wizardOptions = _wizardOptions;
    }
    getNextPageId(pageId) {
        if (!pageId && this._wizardOptions.predefinedDataSources().length > 0) {
            return DataSourceWizardPageId.ChoosePredefinedDataSourcePage;
        }
        else if ((!pageId || pageId === DataSourceWizardPageId.ChoosePredefinedDataSourcePage) && _isMoreThanOneDataSourceTypeAvailable(this._wizardOptions)) {
            return DataSourceWizardPageId.ChooseDataSourceTypePage;
        }
        else if (!pageId || pageId === DataSourceWizardPageId.ChoosePredefinedDataSourcePage) {
            return this.getNextPageId(DataSourceWizardPageId.ChooseDataSourceTypePage);
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Sql) {
            return SqlDataSourceWizardPageId.ChooseConnectionPage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Federation) {
            return FederationDataSourceWizardPageId.FederatedQueryConfigurePage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Object) {
            return ObjectDataSourceWizardPageId.ChooseTypesPage;
        }
        else if (pageId === ObjectDataSourceWizardPageId.ChooseTypesPage && this._getCurrentState().objectDataSourceWizard.selectedType) {
            return ObjectDataSourceWizardPageId.ChooseDataMembersPage;
        }
        else if (pageId === ObjectDataSourceWizardPageId.ChooseDataMembersPage) {
            return ObjectDataSourceWizardPageId.ConfigureParametersPage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Json && this._wizardOptions.connectionStrings.json().length > 0) {
            return JsonDataSourceWizardPageId.ChooseConnectionPage;
        }
        else if (this._wizardOptions.allowCreateNewJsonConnection && pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.Json) {
            return JsonDataSourceWizardPageId.ChooseJsonSourcePage;
        }
        else if (pageId === JsonDataSourceWizardPageId.ChooseConnectionPage && this._getCurrentState().jsonDataSourceWizard.connectionName) {
            return JsonDataSourceWizardPageId.ChooseJsonSchemaPage;
        }
        else if (this._wizardOptions.allowCreateNewJsonConnection && pageId === JsonDataSourceWizardPageId.ChooseConnectionPage && !this._getCurrentState().jsonDataSourceWizard.connectionName) {
            return JsonDataSourceWizardPageId.ChooseJsonSourcePage;
        }
        else if (pageId === JsonDataSourceWizardPageId.ChooseJsonSourcePage) {
            return JsonDataSourceWizardPageId.ChooseJsonSchemaPage;
        }
        else if (pageId === DataSourceWizardPageId.ChooseDataSourceTypePage && this._getCurrentState().dataSourceType === DataSourceType.MongoDB) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage;
        }
        else if (pageId === MongoDBDataSourceWizardPageId.ChooseMongoDBDataConnectionPage) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage;
        }
        else if (pageId === MongoDBDataSourceWizardPageId.ChooseMongoDBCollectionPage) {
            return MongoDBDataSourceWizardPageId.ChooseMongoDBSchemaPage;
        }
        else if (pageId === SqlDataSourceWizardPageId.ChooseConnectionPage && this._getCurrentState().sqlDataSourceWizard.name) {
            return SqlDataSourceWizardPageId.MultiQueryConfigurePage;
        }
        else if (this._getCurrentState().sqlDataSourceWizard.sqlDataSourceJSON && pageId === SqlDataSourceWizardPageId.MultiQueryConfigurePage && this._getCurrentState().sqlDataSourceWizard.customQueries.length > 0) {
            return SqlDataSourceWizardPageId.MultiQueryConfigureParametersPage;
        }
        else if (pageId === SqlDataSourceWizardPageId.MultiQueryConfigurePage || pageId === SqlDataSourceWizardPageId.MultiQueryConfigureParametersPage) {
            const sqlDataSourceWrapped = _restoreSqlDataSourceFromState(this._getCurrentState().sqlDataSourceWizard);
            if (sqlDataSourceWrapped.sqlDataSource.queries().length > 1) {
                return DataSourceWizardPageId.ConfigureMasterDetailRelationshipsPage;
            }
        }
        else if (pageId === FederationDataSourceWizardPageId.FederatedQueryConfigurePage) {
            const federationDataSource = _restoreFederationDataSourceFromState(this._getCurrentState().federationDataSourceWizard, this._wizardOptions.dataSources);
            if (federationDataSource.queries().length > 1) {
                return FederationDataSourceWizardPageId.FederatedMasterDetailRelationshipsPage;
            }
        }
    }
}
function _registerMultiQueryDataSourcePages(factory = new PageFactory(), dataSourceWizardOptions) {
    _registerChooseAvailableDataSourcePage(factory, dataSourceWizardOptions);
    _registerChooseDataSourceTypePage(factory, dataSourceWizardOptions);
    _registerChooseJsonConnectionPage(factory, dataSourceWizardOptions);
    _registerChooseJsonSourcePage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.callbacks.getItemsProviderCallback);
    _registerChooseJsonSchemaPage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.callbacks);
    _registerChooseMongoDBDataConnectionPage(factory, dataSourceWizardOptions);
    _registerChooseMongoDBCollectionPage(factory, dataSourceWizardOptions.requestWrapper, dataSourceWizardOptions.rtl);
    _registerChooseMongoDBSchemaPage(factory, dataSourceWizardOptions.requestWrapper);
    _registerChooseSqlConnectionPage(factory, dataSourceWizardOptions.connectionStrings.sql, dataSourceWizardOptions.getSqlConnectionStrings);
    _registerSpecifyObjectDataSourceSettingsPage(factory, dataSourceWizardOptions);
    _registerFederatedQueryConfigurePage(factory, dataSourceWizardOptions);
    _registerFederatedMasterDetailRelationshipsPage(factory, wrapGetFederationdResultSchema(dataSourceWizardOptions.callbacks.federationDataSourceResultSchema), dataSourceWizardOptions);
    _registerMultiQueryConfigurePage(factory, dataSourceWizardOptions);
    _registerMultiQueryConfigureParametersPage(factory, dataSourceWizardOptions.requestWrapper);
    _registerConfigureMasterDetailRelationshipsPage(factory, wrapRebuildResultSchema(dataSourceWizardOptions.callbacks.sqlDataSourceResultSchema));
    _registerChooseObjectDataSourceTypesPage(factory, dataSourceWizardOptions);
    _registerChooseObjectDataSourceDataMembersPage(factory, dataSourceWizardOptions);
    _registerConfigureObjectDataSourceParametersPage(factory, dataSourceWizardOptions.callbacks.getItemsProviderCallback);
    return factory;
}
function _createMultiQueryDataSourceWizard(factory = new PageFactory(), dataSourceWizardOptions) {
    _registerMultiQueryDataSourcePages(factory, dataSourceWizardOptions);
    return new MultiQueryDataSourceWizard(factory, dataSourceWizardOptions);
}

;// ./dist/query-builder/wizard/fullscreen/fullscreenWizardPageFactory.js

class FullscreenWizardPageFactory extends PageFactory {
    constructor() {
        super(...arguments);
        this.metadata = {};
    }
    registerMetadata(pageId, metadata) {
        super.registerMetadata(pageId, metadata);
    }
    getMetadata(key) {
        return super.getMetadata(key);
    }
}

;// ./dist/query-builder/wizard/internal/_wizardPageSectionFactory.js

class WizardPageSectionFactory extends PageFactory {
    registerMetadata(pageId, metadata) {
        super.registerMetadata(pageId, metadata);
        if (metadata.position === undefined)
            metadata.position = Object.keys(this.metadata).length - 1;
        if (metadata.recreate === undefined)
            metadata.recreate = false;
        if (metadata.disabledText === undefined)
            metadata.disabledText = '';
    }
}

;// ./dist/query-builder/wizard/internal/_wrappedWizardPageSection.js

class WrappedWizardPageSection extends _WrappedWizardPage {
    constructor(pageId, page, metadata) {
        super(pageId, page, metadata.template, metadata.description);
        this.pageId = pageId;
        this.page = page;
        if (page.onChange) {
            this.onChange = (callback) => {
                page.onChange(() => {
                    callback();
                    metadata.onChange && metadata.onChange();
                });
            };
        }
    }
}

;// ./dist/query-builder/wizard/internal/_wizardPageSectionIterator.js



class WizardPageSection {
    resetPage() {
        this.page() && this.page().dispose();
        this.page(null);
    }
    setPage(page) {
        if (this.page() !== page)
            this.page(page);
    }
    constructor(pageId, metadata) {
        this.pageId = pageId;
        this.metadata = metadata;
        this.page = external_ko_namespaceObject.observable(null);
    }
}
class WizardPageSectionIterator {
    _resetPages(fromIndex, resetPage = (pageId) => this._resetPage(pageId)) {
        if (fromIndex < this._pagesIds.length) {
            for (let index = this._pagesIds.length - 1; index >= fromIndex; index--) {
                this._pagesIds[index].forEach((pageId) => resetPage(pageId));
                this._pagesIds.splice(index, 1);
            }
        }
    }
    _tryResetPageByMetadata(pageId) {
        if (this.pageFactory.getMetadata(pageId)['recreate']) {
            this._resetPage(pageId);
            return true;
        }
        return false;
    }
    _resetPage(pageId) {
        this.stateManager.resetPageState(pageId);
        this._resetPageCallback(pageId);
        const page = this._getPage(pageId);
        if (page) {
            page.dispose();
            this._pages.splice(this._pages.indexOf(page), 1);
        }
    }
    _createNewPage(nextPageId) {
        const pageMetadata = this.pageFactory.getMetadata(nextPageId);
        return new WrappedWizardPageSection(nextPageId, pageMetadata.create(), pageMetadata);
    }
    _getPage(pageId) {
        return this._pages.filter(x => x.pageId === pageId)[0];
    }
    _getNextPage(nextPageIds) {
        return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().resolve(nextPageIds.map((nextPageId) => {
            let page = this._getPage(nextPageId);
            if (!page || this._tryResetPageByMetadata(nextPageId)) {
                page = this._createNewPage(nextPageId);
                this._pages.push(page);
            }
            return page;
        })).promise();
    }
    _getPageIndex(pageId) {
        return this._pagesIds.indexOf(this._pagesIds.filter(x => x.some(y => y === pageId))[0]);
    }
    resetNextPages(pageId) {
        this._resetPages(this._getPageIndex(pageId) + 1);
    }
    constructor(pageFactory, stateManager, _resetPageCallback) {
        this.pageFactory = pageFactory;
        this.stateManager = stateManager;
        this._resetPageCallback = _resetPageCallback;
        this._pagesIds = [];
        this._pages = [];
    }
    getStartPage() {
        const startPageId = this.getNextPageId();
        const pageMetadata = this.pageFactory.getMetadata(startPageId);
        const page = new WrappedWizardPageSection(startPageId, pageMetadata.create(), pageMetadata);
        this._pagesIds.push([startPageId]);
        return page;
    }
    getNextPage(currentPageId) {
        const index = this._getPageIndex(currentPageId);
        let nextPageIds = this.getNextPageId(currentPageId);
        if (!nextPageIds) {
            this.resetNextPages(currentPageId);
            return new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred().reject().promise();
        }
        if (!nextPageIds['push']) {
            nextPageIds = [nextPageIds];
        }
        if (JSON.stringify(this._pagesIds[index + 1]) !== JSON.stringify(nextPageIds)) {
            this._resetPages(index + 1, (pageId) => {
                if (nextPageIds.indexOf(pageId) === -1) {
                    this._resetPage(pageId);
                }
            });
            this._pagesIds.push(nextPageIds);
        }
        return this._getNextPage(nextPageIds);
    }
    getCurrentState() {
        return this.stateManager.getCurrentState();
    }
    getNextPageId(pageId) {
        return '';
    }
}

;// ./dist/query-builder/wizard/internal/_wizardPageProcessor.js








class WizardPageProcessor extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    dispose() {
        super.dispose();
        this.sections.forEach(x => x.resetPage());
        this.sections = [];
    }
    _createLoadingState(page) {
        if (!page[WizardPageProcessor.__loadingStateFunctionName]) {
            page[WizardPageProcessor.__loadingStateFunctionName] = (newVal) => this._loadingState(newVal);
        }
    }
    _createNextAction(page) {
        if (!page[__nextActionFunctionName])
            page[__nextActionFunctionName] = () => this._extendedNextAction();
    }
    _loadingState(active) {
        if (active) {
            this._loadingTimeout && clearTimeout(this._loadingTimeout);
            this._loadingTimeout = setTimeout(() => {
                this.isLoading(true);
            }, 100);
        }
        else {
            this._loadingTimeout && clearTimeout(this._loadingTimeout);
            this.isLoading(false);
        }
    }
    _extendedNextAction() { }
    constructor(pageFactory, _loadingState, _nextAction) {
        super();
        this.pageFactory = pageFactory;
        this.events = new external_DevExpress_Analytics_Utils_namespaceObject.EventManager();
        this._loadingTimeout = null;
        this._changeTimeout = null;
        this.sections = [];
        this.isLoading = external_ko_namespaceObject.observable(false);
        if (_loadingState)
            this._loadingState = _loadingState;
        if (_nextAction)
            this._extendedNextAction = _nextAction;
        this._disposables.push(this.events);
    }
    _resetPageById(pageId) {
        const page = this.getPageById(pageId);
        page.resetPage();
    }
    initialize(state, createIterator = (pageFactory, stateManager) => new WizardPageSectionIterator(pageFactory, stateManager, (pageId) => this._resetPageById(pageId))) {
        this.events.call('beforeInitialize', { wizard: this, state });
        this.stateManager = new StateManager(state, this.pageFactory);
        this.iterator = createIterator(this.pageFactory, this.stateManager);
        this.sections = [];
        Object.keys(this.pageFactory.metadata).forEach((key) => {
            this.sections.push(new WizardPageSection(key, this.pageFactory.metadata[key]));
        });
        this.sections = this.sections.sort((a, b) => a.metadata.position - b.metadata.position);
        this.events.call('afterInitialize', { wizard: this });
    }
    _canNext(currentPage) {
        const pageMetadata = !this.isLoading() && currentPage && currentPage.page && this.pageFactory.getMetadata(currentPage.pageId);
        return pageMetadata && pageMetadata.canNext(currentPage.page);
    }
    _canFinish(currentPage) {
        return !this.isLoading() && currentPage && this.pageFactory.getMetadata(currentPage.pageId).canFinish(currentPage.page);
    }
    _initPage(page, force = false, stateChanged = false) {
        this._createNextAction(page.page);
        if (page.onChange) {
            page.onChange(() => {
                this._changeTimeout && clearTimeout(this._changeTimeout);
                this._changeTimeout = setTimeout(() => {
                    this._nextAction(page);
                }, 100);
            });
        }
        else
            throw Error('Page with id ' + page.pageId + ' cannot be used in AutoNavigation, because it does not have method OnChange');
        return page.initialize(this.stateManager.getPageState(page.pageId), force, stateChanged).always(() => this._loadingState(false));
    }
    getPageById(pageId) {
        return this.sections.filter(x => x.pageId === pageId)[0];
    }
    start() {
        this.events.call('beforeStart', { wizard: this });
        this._loadingState(true);
        const page = this.iterator.getStartPage();
        this.events.call('beforePageInitialize', _createBeforeInitializePageEventArgs(page, this));
        this._initPage(page).done(() => {
            const pageSection = this.getPageById(page.pageId);
            if (pageSection) {
                pageSection.setPage(page);
                this.events.call('afterPageInitialize', _createPageEventArgs(page, this));
                this._nextAction(page);
            }
            else {
                throw Error('Page Section with id ' + page.pageId + ' is not found.');
            }
        });
    }
    finishAction() {
        const deferred = new external_DevExpress_Analytics_Internal_namespaceObject.DxDeferred();
        let resolved = false;
        for (let i = this.sections.length - 1; i >= 0; i--) {
            if (this.sections[i].page()) {
                this.sections[i].page().commit().done((result) => {
                    if (this.sections[i].page().isChanged) {
                        this.events.call('beforeFinish', { wizardModel: this, state: this.stateManager.getCurrentState() });
                        this.stateManager.setPageState(this.sections[i].page().pageId, result);
                        this.events.call('afterFinish', { wizardResult: this, state: this.stateManager.getCurrentState() });
                    }
                }).always(() => deferred.resolve());
                resolved = true;
                break;
            }
        }
        if (!resolved)
            deferred.resolve();
        return deferred.promise();
    }
    _nextAction(currentPage, parentIsChanged = false) {
        if (!this._canNext(currentPage) && !this._canFinish(currentPage)) {
            currentPage.resetCommitedState();
            return this.iterator.resetNextPages(currentPage.pageId);
        }
        this._loadingState(true);
        currentPage.commit().done((result) => {
            if (currentPage.isChanged || parentIsChanged || currentPage.page && currentPage.page.changeAlways) {
                this.stateManager.setPageState(currentPage.pageId, result);
                this.iterator.getNextPage(currentPage.pageId).done(pages => {
                    if (pages && pages.length > 0) {
                        pages.forEach((_page) => {
                            const containedPage = this.getPageById(_page.pageId);
                            const page = containedPage && containedPage.page() || _page;
                            this.events.call('beforePageInitialize', _createBeforeInitializePageEventArgs(page, this));
                            this._initPage(page, !!containedPage.page(), currentPage.isChanged || parentIsChanged).done(() => {
                                this.getPageById(page.pageId).setPage(page);
                                this.events.call('afterPageInitialize', _createPageEventArgs(page, this));
                                this._nextAction(page, currentPage.isChanged);
                            });
                        });
                    }
                    else
                        this._loadingState(false);
                    currentPage.isChanged = false;
                }).fail(() => this._loadingState(false));
            }
            else
                this._loadingState(false);
        }).fail(() => this._loadingState(false));
    }
}
WizardPageProcessor.__loadingStateFunctionName = '__loadingState';

;// ./dist/query-builder/wizard/fullscreen/pages/fullscreenWizardPage.js









class FullscreenWizardPage extends WizardPageBase {
    dispose() {
        this._sectionsProcessor.dispose();
        this._factory.reset();
        this._sections = [];
    }
    _patchOnChange() {
        Object.keys(this._factory.metadata).forEach(key => {
            this._factory.metadata[key].onChange = () => this._onChange();
        });
    }
    _getPageStyle(position, isVisible = true) {
        return getSectionStyle(position, this._defaultMargin, isVisible);
    }
    _applyCustomizations() {
        this._sectionsToUnregister.forEach(x => x());
        this._sectionsToUnregister = [];
        this._sectionPositions.forEach(x => x());
        this._sectionPositions = [];
        this._sectionsToRegister.forEach(x => x());
        this._sectionsToRegister = [];
    }
    _setSectionPosition(pageId, position) {
        this._pageCss[pageId] = external_ko_namespaceObject.observable(this._getPageStyle(position));
    }
    constructor() {
        super();
        this._sectionsToUnregister = [];
        this._sectionsToRegister = [];
        this._sectionPositions = [];
        this._initInProgress = external_ko_namespaceObject.observable(false);
        this._defaultMargin = 24;
        this._parentMarginOffset = this._defaultMargin + this._defaultMargin / 2;
        this._className = '';
        this._pageCss = {};
        this.events = new external_DevExpress_Analytics_Utils_namespaceObject.EventManager();
        this._factory = new WizardPageSectionFactory();
        this._sectionsProcessor = new WizardPageProcessor(this._factory, (state) => {
            this[__loadingStateFunctionName] && this[__loadingStateFunctionName](state);
        }, () => this[__nextActionFunctionName] && this[__nextActionFunctionName]());
        this._sectionsProcessor.events.addHandler('beforeStart', () => {
            this._sections = this._sectionsProcessor.sections;
            this._beforeStart();
            this._patchOnChange();
        });
        this._sectionsProcessor.events.addHandler('beforePageInitialize', (args) => {
            this.events.call('beforeSectionInitialize', {
                section: args.page,
                sectionId: args.pageId,
                page: this,
                state: args.state
            });
        });
        this._sectionsProcessor.events.addHandler('afterPageInitialize', (args) => {
            this.events.call('afterSectionInitialize', {
                section: args.page,
                sectionId: args.pageId,
                page: this
            });
        });
    }
    registerSections() { }
    canNext() {
        return this._sectionsProcessor.sections.every(this._sectionCondition);
    }
    _sectionCondition(section) {
        if (section.metadata.required)
            return section.page() && section.metadata.canNext(section.page().page);
        else {
            return !section.page() || section.metadata.canNext(section.page().page);
        }
    }
    canFinish() {
        for (let i = this._sections.length - 1; i >= 0; i--) {
            if (this._sections[i].page() && this._sections[i].metadata.canFinish(this._sections[i].page().page))
                return true;
        }
        return false;
    }
    setSectionPosition(sectionId, position) {
        this._sectionPositions.push(() => {
            this._setSectionPosition(sectionId, position);
        });
    }
    registerSection(sectionId, metadata) {
        this._sectionsToRegister.push(() => {
            this._factory.registerMetadata(sectionId, metadata);
        });
    }
    unregisterSection(sectionId) {
        this._sectionsToUnregister.push(() => this._factory.unregisterMetadata(sectionId));
    }
    _loadPanelViewModel(element, isLoading) {
        return {};
    }
    getNextSectionId(sectionId) { return undefined; }
    initialize(state, stateChanged) {
        this.registerSections();
        this._applyCustomizations();
        this._sectionsProcessor.initialize((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, state));
        this._stateManager = this._sectionsProcessor.stateManager;
        this._sectionsProcessor.iterator.getNextPageId = (pageId) => this.getNextSectionId(pageId);
        this._sectionsProcessor.start();
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)().resolve().promise();
    }
    _beforeStart() { }
    commit() {
        const deferred = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDeferred)();
        this._sectionsProcessor.finishAction().done(() => {
            deferred.resolve((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, this._stateManager.getCurrentState()));
        });
        return deferred.promise();
    }
    _getPageDescription(index, page) {
        return (index + 1) + '. ' + page.metadata.description;
    }
    _showPageDescription(page) {
        return page?.metadata?.alwaysShowTitle || this._sections.length > 1;
    }
}

;// ./dist/query-builder/wizard/fullscreen/pages/selectDataSourcePage.js






class SelectDataSourcePage extends FullscreenWizardPage {
    constructor(wizardOptions, dataSources) {
        super();
        this.wizardOptions = wizardOptions;
        this.dataSources = dataSources;
        this.disabledSectionText = (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)("To specify a data source, select \"No, I'd like to create a new data source\".", 'AnalyticsCoreStringId.Wizard_SelectDataSourceType_Placeholder');
    }
    registerSections() {
        if (this.showPredefinedDataSourceSection()) {
            _registerChooseAvailableDataSourcePage(this._factory, this.wizardOptions);
            this._setSectionPosition(DataSourceWizardPageId.ChoosePredefinedDataSourcePage);
        }
        if (this.showChooseDataSourceTypeSection()) {
            _registerChooseDataSourceTypePage(this._factory, this.wizardOptions);
            this._setSectionPosition(DataSourceWizardPageId.ChooseDataSourceTypePage);
            const meta = this._factory.getMetadata(DataSourceWizardPageId.ChooseDataSourceTypePage);
            meta['disabledText'] = this.disabledSectionText;
        }
        if (this.showPredefinedDataSourceSection() && this.showChooseDataSourceTypeSection()) {
            this._setSectionPosition(DataSourceWizardPageId.ChoosePredefinedDataSourcePage, WizardSectionPosition.Top);
            this._setSectionPosition(DataSourceWizardPageId.ChooseDataSourceTypePage, WizardSectionPosition.Bottom);
        }
    }
    showPredefinedDataSourceSection() {
        return this.dataSources.length > 0;
    }
    showChooseDataSourceTypeSection() {
        return this.wizardOptions.canCreateDataSource && _isMoreThanOneDataSourceTypeAvailable(this.wizardOptions);
    }
    getNextSectionId(sectionId) {
        if (!sectionId && this.showPredefinedDataSourceSection())
            return DataSourceWizardPageId.ChoosePredefinedDataSourcePage;
        else if (!sectionId && this.showChooseDataSourceTypeSection())
            return DataSourceWizardPageId.ChooseDataSourceTypePage;
        else if (sectionId === DataSourceWizardPageId.ChoosePredefinedDataSourcePage && !this._stateManager.getCurrentState().predefinedDataSourceName && this.showChooseDataSourceTypeSection())
            return DataSourceWizardPageId.ChooseDataSourceTypePage;
    }
}
function _registerSelectDataSourcePage(factory, wizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SelectDataSourcePage, {
        setState: (data, state) => {
            state.dataSourceType = data.dataSourceType;
            state.predefinedDataSourceName = data.predefinedDataSourceName;
        },
        getState: (state) => {
            return state;
        },
        resetState: (state, defaultState) => {
            state.dataSource = defaultState.dataSource;
            state.predefinedDataSourceName = null;
        },
        create: () => {
            return new SelectDataSourcePage(wizardOptions, wizardOptions.predefinedDataSources());
        },
        navigationPanelText: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Select Data Source', 'ASPxReportsStringId.ReportDesigner_Wizard_SelectDataSource'),
        template: 'dx-wizard-fullscreen-page'
    });
}

;// ./dist/query-builder/wizard/fullscreen/pages/specifyJsonDataSourceSettingsPage.js







class SpecifyJsonDataSourceSettingsPage extends FullscreenWizardPage {
    constructor(_dataSourceWizardOptions) {
        super();
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
    }
    registerSections() {
        if (this._dataSourceWizardOptions.connectionStrings.json().length > 0) {
            _registerSpecifyJsonConnectionPage(this._factory, this._dataSourceWizardOptions.connectionStrings.json, this._dataSourceWizardOptions.allowCreateNewJsonConnection, this._dataSourceWizardOptions.callbacks.getItemsProviderCallback, this._dataSourceWizardOptions.getJsonConnectionStrings);
            this._setSectionPosition(FullscreenDataSourceWizardSectionId.SpecifyJsonConnectionPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.Right : WizardSectionPosition.Left);
        }
        if (this._dataSourceWizardOptions.allowCreateNewJsonConnection) {
            if (this._dataSourceWizardOptions.connectionStrings.json().length === 0) {
                _registerChooseJsonSourcePage(this._factory, this._dataSourceWizardOptions.requestWrapper, this._dataSourceWizardOptions.callbacks.getItemsProviderCallback);
                this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseJsonSourcePage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.Right : WizardSectionPosition.Left);
                const meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ChooseJsonSourcePage);
                meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)("To create a data connection, select \"No, I'd like to create a new data connection\".", 'AnalyticsCoreStringId.JsonDSWizard_CreateNewConnectionPage_Placeholder');
            }
        }
        _registerChooseJsonSchemaPage(this._factory, this._dataSourceWizardOptions.requestWrapper, this._dataSourceWizardOptions.callbacks);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.Left : WizardSectionPosition.Right);
        const meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage);
        meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To select data fields, choose or create a data connection.', 'AnalyticsCoreStringId.JsonDSWizard_ChooseJsonSchemaPage_Placeholder');
    }
    getNextSectionId(sectionId) {
        if (!sectionId && this._dataSourceWizardOptions.connectionStrings.json().length > 0) {
            return FullscreenDataSourceWizardSectionId.SpecifyJsonConnectionPage;
        }
        else if (!sectionId) {
            return FullscreenDataSourceWizardSectionId.ChooseJsonSourcePage;
        }
        else if (this._dataSourceWizardOptions.allowCreateNewJsonConnection) {
            if (sectionId === FullscreenDataSourceWizardSectionId.SpecifyJsonConnectionPage)
                return FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage;
            else if (sectionId === FullscreenDataSourceWizardSectionId.ChooseJsonSourcePage)
                return FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage;
        }
        else {
            if (sectionId === FullscreenDataSourceWizardSectionId.SpecifyJsonConnectionPage)
                return FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage;
        }
    }
    canNext() {
        const section = this._sectionsProcessor.getPageById(FullscreenDataSourceWizardSectionId.ChooseJsonSchemaPage);
        return section && section.page() && section.metadata.canNext(section.page().page);
    }
}
function _registerSpecifyJsonDataSourceSettingsPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SpecifyJsonDataSourceSettingsPage, {
        setState: (data, state) => {
            state.jsonDataSourceWizard.connectionName = data.jsonDataSourceWizard.connectionName;
            state.jsonDataSourceWizard.newConnectionName = data.jsonDataSourceWizard.newConnectionName;
            state.jsonDataSourceWizard.jsonSource = data.jsonDataSourceWizard.jsonSource;
            state.jsonDataSourceWizard.dataSourceName = data.jsonDataSourceWizard.dataSourceName;
            state.jsonDataSourceWizard.jsonScheme = data.jsonDataSourceWizard.jsonScheme;
            state.jsonDataSourceWizard.rootElement = data.jsonDataSourceWizard.rootElement;
        },
        getState: (state) => state,
        resetState: (state, defaultState) => {
            state.jsonDataSourceWizard.connectionName = defaultState.jsonDataSourceWizard.connectionName;
            state.jsonDataSourceWizard.jsonSource = defaultState.jsonDataSourceWizard.jsonSource;
            state.jsonDataSourceWizard.dataSourceName = defaultState.jsonDataSourceWizard.dataSourceName;
            state.jsonDataSourceWizard.newConnectionName = defaultState.jsonDataSourceWizard.newConnectionName;
            state.jsonDataSourceWizard.jsonScheme = defaultState.jsonDataSourceWizard.jsonScheme;
            state.jsonDataSourceWizard.rootElement = defaultState.jsonDataSourceWizard.rootElement;
        },
        create: () => {
            return new SpecifyJsonDataSourceSettingsPage(dataSourceWizardOptions);
        },
        navigationPanelText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Specify Data Source Settings', 'AnalyticsCoreStringId.Wizard_SpecifyDataSourceSettingsPage'),
        template: 'dx-wizard-fullscreen-page'
    });
}

;// ./dist/query-builder/wizard/fullscreen/pages/specifyMongoDBDataSourceSettingsPage.js







class SpecifyMongoDBDataSourceSettingsPage extends FullscreenWizardPage {
    constructor(_dataSourceWizardOptions) {
        super();
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
        this._requestWrapper = _dataSourceWizardOptions.requestWrapper;
    }
    registerSections() {
        _registerChooseMongoDBDataConnectionPage(this._factory, this._dataSourceWizardOptions);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseMongoDBDataConnectionPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.TopRight : WizardSectionPosition.TopLeft);
        _registerChooseMongoDBCollectionPage(this._factory, this._requestWrapper, this._dataSourceWizardOptions.rtl);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseMongoDBCollectionsPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.BottomRight : WizardSectionPosition.BottomLeft);
        let meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ChooseMongoDBCollectionsPage);
        _registerChooseMongoDBSchemaPage(this._factory, this._requestWrapper);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseMongoDBSchemaPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.Left : WizardSectionPosition.Right);
        meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ChooseMongoDBSchemaPage);
        meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To select data fields, choose or create a data connection.', 'AnalyticsCoreStringId.JsonDSWizard_ChooseJsonSchemaPage_Placeholder');
    }
    getNextSectionId(sectionId) {
        if (!sectionId) {
            return FullscreenDataSourceWizardSectionId.ChooseMongoDBDataConnectionPage;
        }
        else if (sectionId === FullscreenDataSourceWizardSectionId.ChooseMongoDBDataConnectionPage) {
            return FullscreenDataSourceWizardSectionId.ChooseMongoDBCollectionsPage;
        }
        else if (sectionId === FullscreenDataSourceWizardSectionId.ChooseMongoDBCollectionsPage) {
            return FullscreenDataSourceWizardSectionId.ChooseMongoDBSchemaPage;
        }
    }
    canNext() {
        const section = this._sectionsProcessor.getPageById(FullscreenDataSourceWizardSectionId.ChooseMongoDBSchemaPage);
        return section && section.page() && section.metadata.canNext(section.page().page);
    }
}
function _registerSpecifyMongoDBDataSourceSettingsPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SpecifyMongoDBDataSourceSettingsPage, {
        setState: (data, state) => {
            state.mongoDBDataSourceWizard.queries = data.mongoDBDataSourceWizard.queries;
            state.mongoDBDataSourceWizard.connectionJson = data.mongoDBDataSourceWizard.connectionJson;
        },
        getState: (state) => state,
        resetState: (state, defaultState) => {
            state.mongoDBDataSourceWizard.queries = defaultState.mongoDBDataSourceWizard.queries;
            state.mongoDBDataSourceWizard.connectionJson = defaultState.mongoDBDataSourceWizard.connectionJson;
        },
        create: () => {
            return new SpecifyMongoDBDataSourceSettingsPage(dataSourceWizardOptions);
        },
        navigationPanelText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Specify Data Source Settings', 'AnalyticsCoreStringId.Wizard_SpecifyDataSourceSettingsPage'),
        template: 'dx-wizard-fullscreen-page'
    });
}

;// ./dist/query-builder/wizard/fullscreen/pages/specifySqlDataSourceSettingsPage.js











class SpecifySqlDataSourceSettingsPage extends FullscreenWizardPage {
    constructor(_dataSourceWizardOptions) {
        super();
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
    }
    getNextSectionId(sectionId) {
        if (!sectionId)
            return FullscreenDataSourceWizardSectionId.ChooseSqlConnectionPage;
        else if (sectionId === FullscreenDataSourceWizardSectionId.ChooseSqlConnectionPage && this._stateManager.getCurrentState().sqlDataSourceWizard.name) {
            return FullscreenDataSourceWizardSectionId.ConfigureQueryPage;
        }
        else if (sectionId === FullscreenDataSourceWizardSectionId.ConfigureQueryPage) {
            const sections = [];
            const sqlDataSourceWizardState = this._stateManager.getCurrentState().sqlDataSourceWizard;
            if ((sqlDataSourceWizardState.customQueries || []).length > 0) {
                sections.push(FullscreenDataSourceWizardSectionId.ConfigureQueryParametersPage);
            }
            const sqlDataSourceWrapped = _restoreSqlDataSourceFromState(sqlDataSourceWizardState);
            if (sqlDataSourceWrapped.sqlDataSource.queries().length > 1) {
                sections.push(FullscreenDataSourceWizardSectionId.ConfigureMasterDetailRelationshipsPage);
            }
            return sections;
        }
    }
    registerSections() {
        _registerChooseSqlConnectionPage(this._factory, this._dataSourceWizardOptions.connectionStrings.sql, this._dataSourceWizardOptions.getSqlConnectionStrings);
        _registerMultiQueryConfigurePage(this._factory, this._dataSourceWizardOptions);
        _registerConfigureMasterDetailRelationshipsPage(this._factory, wrapRebuildResultSchema(this._dataSourceWizardOptions.callbacks.sqlDataSourceResultSchema));
        _registerMultiQueryConfigureParametersPage(this._factory, this._dataSourceWizardOptions.requestWrapper);
        let meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ChooseSqlConnectionPage);
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose a data connection.', 'AnalyticsCoreStringId.SqlDSWizard_PageChooseConnection');
        meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ConfigureQueryPage);
        meta['recreate'] = true;
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Choose predefined queries and/or create custom queries.', 'AnalyticsCoreStringId.Wizard_Queries_Description');
        meta['required'] = true;
        meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ConfigureMasterDetailRelationshipsPage);
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure master-detail relationships.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMasterDetailRelations');
        meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To create a master-detail relationship, select two or more queries.', 'AnalyticsCoreStringId.Wizard_MasterDetailRelationship_Placeholder');
        meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ConfigureQueryParametersPage);
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure query parameters.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureParameters');
        meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To specify query parameters, select a parameterized stored procedure or create a custom query.', 'AnalyticsCoreStringId.Wizard_ConfigureQueryParameters_Placeholder');
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ChooseSqlConnectionPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.TopRight : WizardSectionPosition.TopLeft);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ConfigureQueryPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.TopLeft : WizardSectionPosition.TopRight);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ConfigureQueryParametersPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.BottomLeft : WizardSectionPosition.BottomRight);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ConfigureMasterDetailRelationshipsPage, this._dataSourceWizardOptions.rtl ? WizardSectionPosition.BottomRight : WizardSectionPosition.BottomLeft);
    }
}
function _registerSpecifySqlDataSourceSettingsPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SpecifySqlDataSourceSettingsPage, {
        setState: (data, state) => {
            state.sqlDataSourceWizard.customQueries = data.sqlDataSourceWizard.customQueries;
            state.sqlDataSourceWizard.name = data.sqlDataSourceWizard.name;
            state.sqlDataSourceWizard.queryName = data.sqlDataSourceWizard.name;
            state.sqlDataSourceWizard.sqlDataSourceJSON = data.sqlDataSourceWizard.sqlDataSourceJSON;
            state.sqlDataSourceWizard.relations = data.sqlDataSourceWizard.relations;
        },
        getState: (state) => state,
        resetState: (state, defaulState) => {
            state.sqlDataSourceWizard = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, defaulState);
        },
        create: () => {
            return new SpecifySqlDataSourceSettingsPage(dataSourceWizardOptions);
        },
        navigationPanelText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Specify Data Source Settings', 'AnalyticsCoreStringId.Wizard_SpecifyDataSourceSettingsPage'),
        template: 'dx-wizard-fullscreen-page'
    });
}

;// ./dist/query-builder/wizard/fullscreen/pages/specifyFederationDataSourceSettingsPage.js









class SpecifyFederationDataSourceSettingsPage extends FullscreenWizardPage {
    constructor(_dataSourceWizardOptions) {
        super();
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
    }
    getNextSectionId(sectionId) {
        if (!sectionId)
            return FullscreenDataSourceWizardSectionId.ConfigureFederatedQueriesPage;
        else if (sectionId === FullscreenDataSourceWizardSectionId.ConfigureFederatedQueriesPage) {
            const federationDataSourceWizard = _restoreFederationDataSourceFromState(this._stateManager.getCurrentState().federationDataSourceWizard, this._dataSourceWizardOptions.dataSources);
            if (federationDataSourceWizard.queries().length > 1) {
                return FullscreenDataSourceWizardSectionId.ConfigureFederatedMasterDetailRelationshipsPage;
            }
        }
    }
    _showPageDescription() {
        return true;
    }
    registerSections() {
        _registerFederatedQueryConfigurePage(this._factory, this._dataSourceWizardOptions);
        _registerFederatedMasterDetailRelationshipsPage(this._factory, wrapGetFederationdResultSchema(this._dataSourceWizardOptions.callbacks.federationDataSourceResultSchema), this._dataSourceWizardOptions);
        let meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ConfigureFederatedQueriesPage);
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Create a federated query.', 'DataAccessUIStringId.WizardPageConfigureFederatedQueryPage');
        meta = this._factory.getMetadata(FullscreenDataSourceWizardSectionId.ConfigureFederatedMasterDetailRelationshipsPage);
        meta.description = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Configure master-detail relationships.', 'AnalyticsCoreStringId.SqlDSWizard_PageConfigureMasterDetailRelations');
        meta['disabledText'] = (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('To create a master-detail relationship, select two or more queries.', 'AnalyticsCoreStringId.Wizard_MasterDetailRelationship_Placeholder');
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ConfigureFederatedQueriesPage, WizardSectionPosition.Top);
        this._setSectionPosition(FullscreenDataSourceWizardSectionId.ConfigureFederatedMasterDetailRelationshipsPage, WizardSectionPosition.Bottom);
    }
}
function _registerSpecifyFederationDataSourceSettingsPage(factory, dataSourceWizardOptions) {
    factory.registerMetadata(FullscreenDataSourceWizardPageId.SpecifyFederationDataSourceSettingsPage, {
        setState: (data, state) => {
            state.federationDataSourceWizard.federationDataSourceJSON = data.federationDataSourceWizard.federationDataSourceJSON;
            state.federationDataSourceWizard.federatedQueries = data.federationDataSourceWizard.federatedQueries;
            state.federationDataSourceWizard.relations = data.federationDataSourceWizard.relations;
            state.federationDataSourceWizard.name = data.federationDataSourceWizard.name;
        },
        getState: (state) => state,
        resetState: (state, defaulState) => {
            state.federationDataSourceWizard = (0,external_DevExpress_Analytics_Internal_namespaceObject.extend)(true, {}, defaulState);
        },
        create: () => {
            return new SpecifyFederationDataSourceSettingsPage(dataSourceWizardOptions);
        },
        navigationPanelText: (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Specify Data Source Settings', 'AnalyticsCoreStringId.Wizard_SpecifyDataSourceSettingsPage'),
        template: 'dx-wizard-fullscreen-page'
    });
}

;// ./dist/query-builder/wizard/fullscreen/fullscreenWizardNavigation.js


class WizardNavigationPanel extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(wizard) {
        super();
        this._steps = [];
        this._disposables.push(wizard._currentPage.subscribe((newPage) => {
            const currentStep = this._steps.filter(step => step.pageIds.some(x => x === newPage.pageId))[0];
            if (currentStep) {
                currentStep.currentPageId = newPage.pageId;
                currentStep.disabled(false);
                this._setStepVisible(currentStep.stepIndex);
            }
        }));
        Object.keys(wizard.pageFactory.metadata).forEach((pageId) => {
            const item = wizard.pageFactory.metadata[pageId];
            const navigationItem = this._steps.filter(x => x.text === item.navigationPanelText)[0];
            if (navigationItem) {
                navigationItem.pageIds.push(pageId);
            }
            else {
                const navigationStep = {
                    text: item.navigationPanelText,
                    pageIds: [pageId],
                    currentPageId: null,
                    stepIndex: this._steps.length,
                    disabled: external_ko_namespaceObject.observable(true),
                    visible: external_ko_namespaceObject.observable(true)
                };
                this._disposables.push(navigationStep.isActive = external_ko_namespaceObject.computed(() => {
                    return wizard._currentPage() && navigationStep.currentPageId === wizard._currentPage().pageId;
                }));
                navigationStep.clickAction = () => {
                    if (!navigationStep.isActive())
                        wizard.goToPage(navigationStep.currentPageId);
                };
                this._steps.push(navigationStep);
            }
        });
        this._disposables.push(this.isVisible = external_ko_namespaceObject.computed(() => {
            return this._steps.filter(step => step.visible()).length > 1;
        }));
    }
    resetAll() {
        this._steps.forEach(step => {
            step.disabled(true);
        });
    }
    _currentStep(pageId) {
        return this._steps.filter(x => x.currentPageId === pageId)[0];
    }
    _reset(pageId) {
        const currentStep = this._currentStep(pageId);
        currentStep && currentStep.disabled(true);
    }
    _resetNextPages(pageId) {
        const currentStep = this._currentStep(pageId);
        if (!currentStep)
            return;
        for (let i = currentStep.stepIndex + 1; i < this._steps.length; i++) {
            this._steps[i].disabled(true);
        }
    }
    _setStepVisible(currentPageIndex) {
        const previousSteps = this._steps.filter((_, index) => index < currentPageIndex);
        if (previousSteps.length > 0 && !previousSteps.some(step => !step.disabled())) {
            previousSteps.forEach(step => step.visible(false));
        }
    }
}

;// ./dist/query-builder/wizard/fullscreen/fullscreenWizard.js



class FullscreenWizard extends PopupWizard {
    constructor(pageFactory, finishCallback) {
        super(pageFactory, finishCallback);
        this._extendCssClass = 'dx-wizard-fullscreen';
        this.navigationPanel = external_ko_namespaceObject.observable(null);
        this.isVisible.subscribe(newValue => {
            if (!newValue) {
                this.navigationPanel() && this.navigationPanel().resetAll();
                this.navigationPanel() && this.navigationPanel().dispose();
                this._onCloseCallback && this._onCloseCallback();
            }
        });
    }
    _onClose(callback) {
        this._onCloseCallback = callback;
    }
    onFinish() {
        this.navigationPanel().dispose();
        super.onFinish();
    }
    _initPage(page) {
        if (page.onChange)
            page.onChange(() => this.navigationPanel()._resetNextPages(page.pageId));
        return super._initPage(page);
    }
    _onResetPage(page) {
        this.navigationPanel()._reset(page.pageId);
    }
    start(finishCallback) {
        if (finishCallback)
            this['_finishCallback'] = finishCallback;
        this.navigationPanel() && this.navigationPanel().resetAll();
        this.navigationPanel() && this.navigationPanel().dispose();
        this.navigationPanel(new WizardNavigationPanel(this));
        super.start();
    }
    _pageDescription() {
        const currentStep = this.navigationPanel()._steps.filter(x => x.isActive())[0];
        if (currentStep) {
            return currentStep.text;
        }
        else {
            return this.pageFactory.getMetadata(this._currentPage().pageId).description;
        }
    }
    _description() {
        return '';
    }
}

;// ./dist/query-builder/wizard/fullscreen/fullscreenMultiQueryDataSourceWizard.js














class FullscreenDataSourceWizard extends FullscreenWizard {
    constructor(factory, _dataSourceWizardOptions) {
        super(factory, _dataSourceWizardOptions.callbacks.finishCallback);
        this._dataSourceWizardOptions = _dataSourceWizardOptions;
    }
    initialize(state, createIterator = (pageFactory, stateManager) => new FullscreenDataSourceWizardPageIterator(pageFactory, stateManager, this._dataSourceWizardOptions, (page) => this._onResetPage(page))) {
        if (this._dataSourceWizardOptions.sqlDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.sqlDataSourceWizard)) {
            state.dataSourceType = DataSourceType.Sql;
        }
        else if (this._dataSourceWizardOptions.jsonDataSourceAvailable || state.jsonDataSourceWizard.jsonSource) {
            state.dataSourceType = DataSourceType.Json;
        }
        else if (this._dataSourceWizardOptions.objectDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.objectDataSourceWizard)) {
            state.dataSourceType = DataSourceType.Object;
        }
        else if (this._dataSourceWizardOptions.mongoDBDataSourceAvailable || !(0,external_DevExpress_Analytics_Internal_namespaceObject.isEmptyObject)(state.mongoDBDataSourceWizard)) {
            state.dataSourceType = DataSourceType.MongoDB;
        }
        super.initialize(state, createIterator);
    }
    canRunWizard() {
        return this._dataSourceWizardOptions.canRunWizard;
    }
    _description() {
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Data Source Wizard', 'AnalyticsCoreStringId.DSWizard_Title');
    }
}
class FullscreenDataSourceWizardPageIterator extends PageIterator {
    constructor(factory, stateManager, _dataSourceOptions, onResetPage) {
        super(factory, stateManager, onResetPage);
        this._dataSourceOptions = _dataSourceOptions;
    }
    _shouldSelectDataSource() {
        return _isMoreThanOneDataSourceTypeAvailable(this._dataSourceOptions) || this._dataSourceOptions.predefinedDataSources().length > 0;
    }
    getNextPageId(pageId) {
        if (!pageId && this._shouldSelectDataSource()) {
            return FullscreenDataSourceWizardPageId.SelectDataSourcePage;
        }
        else if (!pageId && this._dataSourceOptions.sqlDataSourceAvailable) {
            return FullscreenDataSourceWizardPageId.SpecifySqlDataSourceSettingsPage;
        }
        else if (!pageId && this._dataSourceOptions.jsonDataSourceAvailable) {
            return FullscreenDataSourceWizardPageId.SpecifyJsonDataSourceSettingsPage;
        }
        else if (!pageId && this._dataSourceOptions.objectDataSourceAvailable) {
            return FullscreenDataSourceWizardPageId.SpecifyObjectDataSourceSettingsPage;
        }
        else if (!pageId && this._dataSourceOptions.mongoDBDataSourceAvailable) {
            return FullscreenDataSourceWizardPageId.SpecifyMongoDBDataSourceSettingsPage;
        }
        else if (pageId === FullscreenDataSourceWizardPageId.SelectDataSourcePage && this._getCurrentState().dataSourceType === DataSourceType.Json) {
            return FullscreenDataSourceWizardPageId.SpecifyJsonDataSourceSettingsPage;
        }
        else if (pageId === FullscreenDataSourceWizardPageId.SelectDataSourcePage && this._getCurrentState().dataSourceType === DataSourceType.MongoDB) {
            return FullscreenDataSourceWizardPageId.SpecifyMongoDBDataSourceSettingsPage;
        }
        else if (pageId === FullscreenDataSourceWizardPageId.SelectDataSourcePage && this._getCurrentState().dataSourceType === DataSourceType.Sql) {
            return FullscreenDataSourceWizardPageId.SpecifySqlDataSourceSettingsPage;
        }
        else if (pageId === FullscreenDataSourceWizardPageId.SelectDataSourcePage && this._getCurrentState().dataSourceType === DataSourceType.Object) {
            return FullscreenDataSourceWizardPageId.SpecifyObjectDataSourceSettingsPage;
        }
        else if (pageId === FullscreenDataSourceWizardPageId.SelectDataSourcePage && this._getCurrentState().dataSourceType === DataSourceType.Federation) {
            return FullscreenDataSourceWizardPageId.SpecifyFederationDataSourceSettingsPage;
        }
    }
}
function _createDataSourceFullscreenWizard(dataSourceWizardOptions) {
    const factory = new FullscreenWizardPageFactory();
    const wizard = new FullscreenDataSourceWizard(factory, dataSourceWizardOptions);
    _registerSelectDataSourcePage(factory, dataSourceWizardOptions);
    _registerSpecifySqlDataSourceSettingsPage(factory, dataSourceWizardOptions);
    _registerSpecifyFederationDataSourceSettingsPage(factory, dataSourceWizardOptions);
    _registerSpecifyJsonDataSourceSettingsPage(factory, dataSourceWizardOptions);
    _registerSpecifyMongoDBDataSourceSettingsPage(factory, dataSourceWizardOptions);
    _registerSpecifyObjectDataSourceSettingsPage(factory, dataSourceWizardOptions);
    return wizard;
}

;// ./dist/analytics-wizard.js











































































































;// ./dist/query-builder/wizard/internal/_wizardAction.js

class WizardAction {
    constructor(handler, text) {
        this.handler = handler;
        this.isVisible = external_ko_namespaceObject.observable(true);
        this.isDisabled = external_ko_namespaceObject.observable(false);
        this.text = text;
    }
}

;// ./dist/analytics-wizard-internal.js































































;// ./dist/queryBuilder-widgets.js



;// ./dist/query-builder/widgets/_undoEditor.js


class UndoEditor extends external_DevExpress_Analytics_Widgets_namespaceObject.Editor {
    constructor(info, level, parentDisabled, textToSearch) {
        super(info, level, parentDisabled, textToSearch);
    }
    generateValue(undoEngine) {
        if (!this.undoValue) {
            this._disposables.push(this.undoValue = external_ko_namespaceObject.computed({
                read: () => {
                    return this.value();
                },
                write: (val) => {
                    undoEngine().start();
                    this.value(val);
                    undoEngine().end();
                }
            }));
        }
        return this.undoValue;
    }
}

;// ./dist/query-builder/widgets/_manageFederatedQueriesEditor.js








class ManageFederatedQueriesEditor extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.PopupEditorBase {
    _createAddQueryButton() {
        const addQueryTemplate = 'dxrd-managequeries-selectbox';
        const queriesTreeNode = new FederatedQueriesTreeNode('queries', (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Federated Queries', 'DataAccessUIStringId.FederatedQueryCollectionList_Title'), 'list', false, external_ko_namespaceObject.observable({ showQbCallBacks: this._queriesPopupHelper.callBacks }));
        const popoverListItems = queriesTreeNode.popoverListItems();
        const addAction = {
            text: (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Add query', 'AnalyticsCoreStringId.SqlDSWizard_AddQuery'),
            items: popoverListItems.map(x => {
                return {
                    text: x.name,
                    onClick: x.addAction
                };
            }),
            dropDownOptions: {
                width: 150,
                container: '.dx-designer-viewport',
            },
            useItemTextAsTitle: false
        };
        this.buttonItems.push({ toolbar: 'bottom', location: 'before', template: function () { return (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)(addQueryTemplate); }, options: addAction });
    }
    constructor(_dataSource, _callBack, rtl = false) {
        super();
        this._dataSource = _dataSource;
        this._callBack = _callBack;
        this.rtl = rtl;
        this.className = 'dxrd-federated-manageQueries-editor';
        this._disposables.push(this._queriesPopupHelper = new FederatedQueriesHelper(this._dataSource, this._dataSource.queries, {}, rtl));
        this._createAddQueryButton();
        this.queriesStoreData = external_ko_namespaceObject.pureComputed(() => this._dataSource && this._dataSource.queries().map(x => {
            return {
                name: x.alias(),
                id: x.alias(),
                type: FederationQueryType[x.queryType()]
            };
        }));
        this._disposables.push(this.queriesStoreData);
        this.queriesGrid = {
            rtlEnabled: rtl,
            dataSource: this.queriesStoreData,
            showColumnLines: false,
            hoverStateEnabled: true,
            scrolling: { mode: 'infinite' },
            height: '100%',
            editing: {
                mode: 'cell',
                allowUpdating: true,
            },
            onRowUpdating: (event) => {
                event.oldData.alias = event.newData.name;
            },
            columns: [{
                    dataField: 'name',
                    get caption() {
                        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Name', 'AnalyticsCoreStringId.CollectionEditor_Name_Placeholder');
                    },
                }, {
                    type: 'buttons',
                    width: 100,
                    buttons: [
                        {
                            icon: 'edit',
                            onClick: (e) => {
                                this._queriesPopupHelper.editQuery(e.row.data.type, e.row.data.id);
                            },
                            cssClass: 'dxrd-manage-queries-actions dxd-icon-highlighted dxrd-image-operations-edit-query',
                            template: (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-svg-operations-edit'),
                        },
                        {
                            icon: 'delete',
                            onClick: (e) => {
                                this._dataSource.removeQuery(e.row.data.id);
                            },
                            cssClass: 'dxrd-manage-queries-actions dxd-icon-highlighted dxrd-image-recycle-bin',
                            template: (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-svg-operations-recycle_bin'),
                        }
                    ]
                }
            ]
        };
    }
    save() {
        this.queriesStoreData().forEach(data => {
            const query = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(this._dataSource.queries(), query => query.alias() === data.id);
            query.sources().forEach(x => this._dataSource.addSource(x));
            if (query && data.id !== data.name) {
                query.alias(data.name);
            }
        });
        this._callBack();
        super.save();
        this.dispose();
    }
    canSave() {
        return !!this.queriesStoreData().length;
    }
    close() {
        super.close();
        this.dispose();
    }
    dispose() {
        super.dispose();
        this.queriesGrid = null;
        this._dataSource = null;
        this._callBack = null;
    }
    title() {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.getLocalization)('Manage Queries', 'DataAccessUIStringId.FederatedQueryCollectionEditorForm_Title');
    }
}

;// ./dist/query-builder/widgets/_rightPanelSwitcher.js


class RightPanelSwitcher extends external_DevExpress_Analytics_Utils_namespaceObject.Disposable {
    constructor(collapsed, editableObj, visibleCondition = (_) => true, getDisplayName) {
        super();
        this.editableObj = editableObj;
        this.getDisplayName = getDisplayName;
        this._collapsed = collapsed;
        this._disposables.push(this.visible = external_ko_namespaceObject.pureComputed(() => {
            return !this.disabled() && !this._collapsed();
        }));
        this._disposables.push(this.disabled = external_ko_namespaceObject.pureComputed(() => {
            const disabled = !editableObj() || !visibleCondition(editableObj());
            if (disabled && !this._collapsed())
                this._collapsed(true);
            return disabled;
        }));
    }
    dispose() {
        super.dispose();
        this._collapsed = null;
    }
    toogle() {
        this._collapsed(!this._collapsed());
    }
    get title() {
        return this.getDisplayName(this.editableObj());
    }
}

;// ./dist/query-builder/widgets/registerEditorTemplates.js


function registerEditorTemplates() {
    editorTemplates.registerEditors({
        bool: { header: 'dx-boolean-select', custom: 'dxqb-property-editor' },
        combobox: { header: 'dx-combobox', custom: 'dxqb-property-editor' },
        comboboxUndo: { header: 'dx-combobox-undo', custom: 'dxqb-property-editor', editorType: UndoEditor },
        text: { header: 'dx-text', custom: 'dxqb-property-editor' },
        filterEditor: { header: 'dxrd-filterstring', custom: 'dxqb-property-editor' },
        filterGroupEditor: { header: 'dxrd-filterstringgroup', custom: 'dxqb-property-editor' },
        numeric: { header: 'dx-numeric', custom: 'dxqb-property-editor' }
    });
}

;// ./dist/queryBuilder-widgets-internal.js




















registerEditorTemplates();




















;// ./dist/queryBuilder-metadata.js



;// ./dist/queryBuilder-utils.js









;// ./dist/dx-query-builder-bindings.generated.js

(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { x1: startPoint().relativeX, y1: startPoint().relativeY, x2: endPoint().relativeX, y2: endPoint().relativeY }', function ($context, $element) { return { 'attr': function () { return { 'x1': $context.$data.startPoint().relativeX, 'y1': $context.$data.startPoint().relativeY, 'x2': $context.$data.endPoint().relativeX, 'y2': $context.$data.endPoint().relativeY }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, trackCursor: underCursor, style: { \'marginLeft\': -3 * _context.zoom() + \'px\', \'marginTop\': -3 * _context.zoom() + \'px\' }, draggable: $root.connectingPointDragHandler', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'trackCursor': function () { return $context.$data.underCursor; }, 'style': function () { return { 'marginLeft': -3 * $context.$data._context.zoom() + 'px', 'marginTop': -3 * $context.$data._context.zoom() + 'px' }; }, 'draggable': function () { return $context.$root.connectingPointDragHandler; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('draggable: $root.connectionPointDragHandler, styleunit: { top: relativeY, left: relativeX }', function ($context, $element) { return { 'draggable': function () { return $context.$root.connectionPointDragHandler; }, 'styleunit': function () { return { 'top': $context.$data.relativeY, 'left': $context.$data.relativeX }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, trackCursor: underCursor', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'trackCursor': function () { return $context.$data.underCursor; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, dxclick: function() {}, draggable: $root.dragHandler', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'dxclick': function () { return function () { }; }, 'draggable': function () { return $context.$root.dragHandler; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxdd-connection-line\' }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxdd-connection-line' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: startPoint', function ($context, $element) { return { 'with': function () { return $context.$data.startPoint; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxdd-connection-point-selection\' }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxdd-connection-point-selection' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: endPoint', function ($context, $element) { return { 'with': function () { return $context.$data.endPoint; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { id: \'dxqb-arrow_end\' + connectorID() }', function ($context, $element) { return { 'attr': function () { return { 'id': 'dxqb-arrow_end' + $context.$data.connectorID() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { id: \'dxqb-arrow_start\' + connectorID() }', function ($context, $element) { return { 'attr': function () { return { 'id': 'dxqb-arrow_start' + $context.$data.connectorID() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { points: routePointsSet, \'marker-end\': showArrow() ? \'url(#dxqb-arrow_end\' + connectorID() + \')\' : \'\', \'marker-start\': showRightArrow() ? \'url(#dxqb-arrow_start\' + connectorID() + \')\' : \'\' }', function ($context, $element) { return { 'attr': function () { return { 'points': $context.$data.routePointsSet, 'marker-end': $context.$data.showArrow() ? 'url(#dxqb-arrow_end' + $context.$data.connectorID() + ')' : '', 'marker-start': $context.$data.showRightArrow() ? 'url(#dxqb-arrow_start' + $context.$data.connectorID() + ')' : '' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: showArrow', function ($context, $element) { return { 'if': function () { return $context.$data.showArrow; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: showRightArrow', function ($context, $element) { return { 'if': function () { return $context.$data.showRightArrow; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, visible: isVisible', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'visible': function () { return $context.$data.isVisible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('trackCursor: underCursor, click: $root.selectItemProperties', function ($context, $element) { return { 'trackCursor': function () { return $context.$data.underCursor; }, 'click': function () { return $context.$root.selectItemProperties; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, dxclick: function() {}, visible: isVisible', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'dxclick': function () { return function () { }; }, 'visible': function () { return $context.$data.isVisible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: position, style: { cursor: isVerticalLine ? \'ew-resize\' : \'ns-resize\' }, routeLineDraggable: { starting: $root.resizeHandler.starting, stopped: function() { resizeStopped(); $root.resizeHandler.stopped(); }, forceResize: resizeHandler }', function ($context, $element) { return { 'styleunit': function () { return $context.$data.position; }, 'style': function () { return { 'cursor': $context.$data.isVerticalLine ? 'ew-resize' : 'ns-resize' }; }, 'routeLineDraggable': function () { return { 'starting': $context.$root.resizeHandler.starting, 'stopped': function () { $context.$data.resizeStopped(); $context.$root.resizeHandler.stopped(); }, 'forceResize': $context.$data.resizeHandler }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxdd-routed-connection-line\' }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxdd-routed-connection-line' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: routeLineWrappers', function ($context, $element) { return { 'foreach': function () { return $context.$data.routeLineWrappers; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: $parent', function ($context, $element) { return { 'with': function () { return $context.$parent; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: isLocked', function ($context, $element) { return { 'ifnot': function () { return $context.$data.isLocked; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: isLocked', function ($context, $element) { return { 'if': function () { return $context.$data.isLocked; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { lineHeight: positionLineHeightWithoutMargins }, style: css', function ($context, $element) { return { 'styleunit': function () { return { 'lineHeight': $context.$data.positionLineHeightWithoutMargins }; }, 'style': function () { return $context.$data.css; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: contenttemplate, styleunit: { \'height\': positionLineHeightWithoutMargins, \'width\': positionWidthWithoutMargins }', function ($context, $element) { return { 'template': function () { return $context.$data.contenttemplate; }, 'styleunit': function () { return { 'height': $context.$data.positionLineHeightWithoutMargins, 'width': $context.$data.positionWidthWithoutMargins }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: { dblclick: $root.inlineTextEdit.show($element) }, css: {\'dxrd-selected\': selected, \'dxrd-focused\': focused }, resizable: $root.resizeHandler, draggable: $root.dragHandler, styleunit: position, trackCursor: underCursor', function ($context, $element) { return { 'event': function () { return { 'dblclick': $context.$root.inlineTextEdit.show($element) }; }, 'css': function () { return { 'dxrd-selected': $context.$data.selected, 'dxrd-focused': $context.$data.focused }; }, 'resizable': function () { return $context.$root.resizeHandler; }, 'draggable': function () { return $context.$root.dragHandler; }, 'styleunit': function () { return $context.$data.position; }, 'trackCursor': function () { return $context.$data.underCursor; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: contenttemplate, styleunit: { \'height\': positionLineHeightWithoutMargins, \'width\': positionWidthWithoutMargins}', function ($context, $element) { return { 'template': function () { return $context.$data.contenttemplate; }, 'styleunit': function () { return { 'height': $context.$data.positionLineHeightWithoutMargins, 'width': $context.$data.positionWidthWithoutMargins }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextArea: { value: text, onKeyUp: keypressAction, valueChangeEvent: \'keyup\' }', function ($context, $element) { return { 'dxTextArea': function () { return { 'value': $context.$data.text, 'onKeyUp': $context.$data.keypressAction, 'valueChangeEvent': 'keyup' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: getControlModel().text', function ($context, $element) { return { 'text': function () { return $context.$data.getControlModel().text; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !$root.inlineTextEdit.visible()', function ($context, $element) { return { 'if': function () { return !$context.$root.inlineTextEdit.visible(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $root.inlineTextEdit.visible', function ($context, $element) { return { 'if': function () { return $context.$root.inlineTextEdit.visible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: $root.inlineTextEdit', function ($context, $element) { return { 'with': function () { return $context.$root.inlineTextEdit; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: getControlModel().type() === \'Ellipse\'', function ($context, $element) { return { 'if': function () { return $context.$data.getControlModel().type() === 'Ellipse'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: getControlModel().type() === \'Condition\'', function ($context, $element) { return { 'if': function () { return $context.$data.getControlModel().type() === 'Condition'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !getControlModel().type()', function ($context, $element) { return { 'if': function () { return !$context.$data.getControlModel().type(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: connectingPoints', function ($context, $element) { return { 'foreach': function () { return $context.$data.connectingPoints; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxdd-connecting-point\'', function ($context, $element) { return { 'template': function () { return 'dxdd-connecting-point'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { \'width\': Math.min(pageWidth(), $root.surfaceSize()) }, click: function(_, e) { $root.selection.clickHandler(null, e); e.stopPropagation(); }, keyDownActions: $root.actionLists.getViewModel()', function ($context, $element) { return { 'styleunit': function () { return { 'width': Math.min($context.$data.pageWidth(), $context.$root.surfaceSize()) }; }, 'click': function () { return function (_, e) { $context.$root.selection.clickHandler(null, e); e.stopPropagation(); }; }, 'keyDownActions': function () { return $context.$root.actionLists.getViewModel(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { minWidth: Math.min(pageWidth(), $root.surfaceSize()), maxWidth: pageWidth(),  maxHeight: pageHeight() + 20 }', function ($context, $element) { return { 'styleunit': function () { return { 'minWidth': Math.min($context.$data.pageWidth(), $context.$root.surfaceSize()), 'maxWidth': $context.$data.pageWidth(), 'maxHeight': $context.$data.pageHeight() + 20 }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('selectable: { selection: $root.selection, zoom: zoom }', function ($context, $element) { return { 'selectable': function () { return { 'selection': $context.$root.selection, 'zoom': $context.$data.zoom }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { \'width\': pageWidth(), \'height\': pageHeight() }, trackCursor: underCursor', function ($context, $element) { return { 'styleunit': function () { return { 'width': $context.$data.pageWidth(), 'height': $context.$data.pageHeight() }; }, 'trackCursor': function () { return $context.$data.underCursor; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: controls', function ($context, $element) { return { 'foreach': function () { return $context.$data.controls; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: isSelected() ? selectiontemplate : template }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.isSelected() ? $context.$data.selectiontemplate : $context.$data.template }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { x1: startPoint.x, y1: startPoint.y, x2: endPoint.x, y2: endPoint.y }', function ($context, $element) { return { 'attr': function () { return { 'x1': $context.$data.startPoint.x, 'y1': $context.$data.startPoint.y, 'x2': $context.$data.endPoint.x, 'y2': $context.$data.endPoint.y }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { \'width\': $root.surfaceSize }, click: function(_, e) { $root.selection.clickHandler(null, e); e.stopPropagation(); }, keyDownActions: $root.actionLists.getViewModel()', function ($context, $element) { return { 'styleunit': function () { return { 'width': $context.$root.surfaceSize }; }, 'click': function () { return function (_, e) { $context.$root.selection.clickHandler(null, e); e.stopPropagation(); }; }, 'keyDownActions': function () { return $context.$root.actionLists.getViewModel(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { direction: \'both\', showScrollbar: \'always\', scrollByContent: false, scrollByThumb: true, bounceEnabled: false, useNative: true, height: \'100%\' }', function ($context, $element) { return { 'dxScrollView': function () { return { 'direction': 'both', 'showScrollbar': 'always', 'scrollByContent': false, 'scrollByThumb': true, 'bounceEnabled': false, 'useNative': true, 'height': '100%' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { \'minWidth\': pageWidth, \'minHeight\': pageHeight }, trackCursor: { recalculateBounds: true, underCursor: underCursor }', function ($context, $element) { return { 'styleunit': function () { return { 'minWidth': $context.$data.pageWidth, 'minHeight': $context.$data.pageHeight }; }, 'trackCursor': function () { return { 'recalculateBounds': true, 'underCursor': $context.$data.underCursor }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('trackCursor: { recalculateBounds: true, underCursor: underCursor }, style: { \'z-index\' : $parent.dragDropStarted() ? 2 : null }', function ($context, $element) { return { 'trackCursor': function () { return { 'recalculateBounds': true, 'underCursor': $context.$data.underCursor }; }, 'style': function () { return { 'z-index': $context.$parent.dragDropStarted() ? 2 : null }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: placeholder()', function ($context, $element) { return { 'text': function () { return $context.$data.placeholder(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { top: position.top(), left: position.left() }', function ($context, $element) { return { 'styleunit': function () { return { 'top': $context.$data.position.top(), 'left': $context.$data.position.left() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: {x1 : $data.x, y1: $data.y, x2: $parent.routePoints()[$index()+1].x, y2: $parent.routePoints()[$index()+1].y }', function ($context, $element) { return { 'attr': function () { return { 'x1': $context.$data.x, 'y1': $context.$data.y, 'x2': $context.$parent.routePoints()[$context.$index() + 1].x, 'y2': $context.$parent.routePoints()[$context.$index() + 1].y }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: tables().length === 0', function ($context, $element) { return { 'if': function () { return $context.$data.tables().length === 0; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: relations', function ($context, $element) { return { 'foreach': function () { return $context.$data.relations; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: conditions', function ($context, $element) { return { 'foreach': function () { return $context.$data.conditions; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !isSelected()', function ($context, $element) { return { 'if': function () { return !$context.$data.isSelected(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: tables', function ($context, $element) { return { 'foreach': function () { return $context.$data.tables; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: isSelected()', function ($context, $element) { return { 'if': function () { return $context.$data.isSelected(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: selectiontemplate', function ($context, $element) { return { 'template': function () { return $context.$data.selectiontemplate; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: $root.columnDragHandler.dragDropConnector', function ($context, $element) { return { 'with': function () { return $context.$root.columnDragHandler.dragDropConnector; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: routePoints', function ($context, $element) { return { 'foreach': function () { return $context.$data.routePoints; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $index() < ($parent.routePoints().length - 1)', function ($context, $element) { return { 'if': function () { return $context.$index() < ($context.$parent.routePoints().length - 1); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('trackCursor: { recalculateBounds: true, underCursor: underCursor }, draggable: $root.columnDragHandler, css: cssClasses($root.surface(), $root.columnDragHandler, $parent), click: $root.selectItemProperties', function ($context, $element) { return { 'trackCursor': function () { return { 'recalculateBounds': true, 'underCursor': $context.$data.underCursor }; }, 'draggable': function () { return $context.$root.columnDragHandler; }, 'css': function () { return $context.$data.cssClasses($context.$root.surface(), $context.$root.columnDragHandler, $context.$parent); }, 'click': function () { return $context.$root.selectItemProperties; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCheckBox: { value: selectedWrapper }, click: function(surface, e) { surface.toggleSelected(); e.stopPropagation(); return true;  }', function ($context, $element) { return { 'dxCheckBox': function () { return { 'value': $context.$data.selectedWrapper }; }, 'click': function () { return function (surface, e) { surface.toggleSelected(); e.stopPropagation(); return true; }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { title: getControlModel().actualName }', function ($context, $element) { return { 'attr': function () { return { 'title': $context.$data.getControlModel().actualName }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: getControlModel().actualName', function ($context, $element) { return { 'text': function () { return $context.$data.getControlModel().actualName; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('trackCursor: underCursor, css: cssClasses(), click: $root.selectItemProperties', function ($context, $element) { return { 'trackCursor': function () { return $context.$data.underCursor; }, 'css': function () { return $context.$data.cssClasses(); }, 'click': function () { return $context.$root.selectItemProperties; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { title: getControlModel().name }', function ($context, $element) { return { 'attr': function () { return { 'title': $context.$data.getControlModel().name }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: getControlModel().name', function ($context, $element) { return { 'text': function () { return $context.$data.getControlModel().name; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if:  $data.isAscending()', function ($context, $element) { return { 'if': function () { return $context.$data.isAscending(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-queryBuilder-sorting_asc\'', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-queryBuilder-sorting_asc'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if:  $data.isDescending()', function ($context, $element) { return { 'if': function () { return $context.$data.isDescending(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-queryBuilder-sorting_desc\'', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-queryBuilder-sorting_desc'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.getControlModel().groupBy', function ($context, $element) { return { 'if': function () { return $context.$data.getControlModel().groupBy; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-queryBuilder-group_by\'', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-queryBuilder-group_by'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.isAggregate()', function ($context, $element) { return { 'if': function () { return $context.$data.isAggregate(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-queryBuilder-aggregate\'', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-queryBuilder-aggregate'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('resizable: resizable($root.resizeHandler, $element), styleunit: position', function ($context, $element) { return { 'resizable': function () { return $context.$data.resizable($context.$root.resizeHandler, $element); }, 'styleunit': function () { return $context.$data.position; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: {\'dxrd-selected\': selected, \'dxrd-focused\': focused }, draggable: $root.dragHandler, styleunit: position, trackCursor: underCursor, click: $root.selectItemProperties', function ($context, $element) { return { 'css': function () { return { 'dxrd-selected': $context.$data.selected, 'dxrd-focused': $context.$data.focused }; }, 'draggable': function () { return $context.$root.dragHandler; }, 'styleunit': function () { return $context.$data.position; }, 'trackCursor': function () { return $context.$data.underCursor; }, 'click': function () { return $context.$root.selectItemProperties; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { lineHeight: position.lineHeight }, style: css', function ($context, $element) { return { 'styleunit': function () { return { 'lineHeight': $context.$data.position.lineHeight }; }, 'style': function () { return $context.$data.css; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: contenttemplate, styleunit: { \'height\': position.lineHeight, \'width\': position.width }', function ($context, $element) { return { 'template': function () { return $context.$data.contenttemplate; }, 'styleunit': function () { return { 'height': $context.$data.position.lineHeight, 'width': $context.$data.position.width }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCheckBox: { value: selectedWrapper }, click: function(surface, e) { $root.undoEngine().start(); surface.toggleSelected(); $root.undoEngine().end(); e.stopPropagation(); return true; }', function ($context, $element) { return { 'dxCheckBox': function () { return { 'value': $context.$data.selectedWrapper }; }, 'click': function () { return function (surface, e) { $context.$root.undoEngine().start(); surface.toggleSelected(); $context.$root.undoEngine().end(); e.stopPropagation(); return true; }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $root.columnsLoadingMsg()', function ($context, $element) { return { 'text': function () { return $context.$root.columnsLoadingMsg(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('style: { cursor: selected() ? \'move\' : \'default\' }', function ($context, $element) { return { 'style': function () { return { 'cursor': $context.$data.selected() ? 'move' : 'default' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: { dblclick: $root.inlineTextEdit.show($element) }, style: { cursor: selected() ? \'move\' : \'default\' }', function ($context, $element) { return { 'event': function () { return { 'dblclick': $context.$root.inlineTextEdit.show($element) }; }, 'style': function () { return { 'cursor': $context.$data.selected() ? 'move' : 'default' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: getControlModel().displaySourceName', function ($context, $element) { return { 'text': function () { return $context.$data.getControlModel().displaySourceName; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.isSelected()', function ($context, $element) { return { 'if': function () { return $context.$data.isSelected(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: titletemplate', function ($context, $element) { return { 'template': function () { return $context.$data.titletemplate; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: $data.isInitialized()', function ($context, $element) { return { 'ifnot': function () { return $context.$data.isInitialized(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.isInitialized()', function ($context, $element) { return { 'if': function () { return $context.$data.isInitialized(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: asterisk.template, data: asterisk }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.asterisk.template, 'data': $context.$data.asterisk }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: columns', function ($context, $element) { return { 'foreach': function () { return $context.$data.columns; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('lazy: { template: $data.template  }', function ($context, $element) { return { 'lazy': function () { return { 'template': $context.$data.template }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !$root.inlineTextEdit.visible() || !selected()', function ($context, $element) { return { 'if': function () { return !$context.$root.inlineTextEdit.visible() || !$context.$data.selected(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $root.inlineTextEdit.visible() && selected()', function ($context, $element) { return { 'if': function () { return $context.$root.inlineTextEdit.visible() && $context.$data.selected(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: { showTitle: true, resizeEnabled: true, width: 700, height: 500, maxHeight: \'95%\', maxWidth: \'95%\', minHeight: 300, minWidth: 400, title: title(), visible: popupVisible, wrapperAttr: { class: \'dx-filtereditor\' }, toolbarItems: buttonItems, showCloseButton: true, container: $root.getPopupContainer($element), position: { of: $root.getPopupContainer($element) }, }', function ($context, $element) { return { 'dxPopup': function () { return { 'showTitle': true, 'resizeEnabled': true, 'width': 700, 'height': 500, 'maxHeight': '95%', 'maxWidth': '95%', 'minHeight': 300, 'minWidth': 400, 'title': $context.$data.title(), 'visible': $context.$data.popupVisible, 'wrapperAttr': { 'class': 'dx-filtereditor' }, 'toolbarItems': $context.$data.buttonItems, 'showCloseButton': true, 'container': $context.$root.getPopupContainer($element), 'position': { 'of': $context.$root.getPopupContainer($element) } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: queryName', function ($context, $element) { return { 'text': function () { return $context.$data.queryName; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('service: { name: \'createRelation\' }', function ($context, $element) { return { 'service': function () { return { 'name': 'createRelation' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopupWithAutoHeight: { height: \'300px\', focusStateEnabled: false, wrapperAttr: { class: \'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay\' }, position: $root.rtl ? { my: \'right top\', at: \'right bottom\', of: popupService.target } : { my: \'left top\', at: \'left bottom\', of: popupService.target }, container: $root.getPopupContainer($element), target: popupService.target, showTitle: false, showCloseButton: false, animation: {}, hideOnOutsideClick: true, shading: false, minWidth:\'170px\', maxWidth:\'500px\', width: \'auto\', visible: popupService.visible }', function ($context, $element) { return { 'dxPopupWithAutoHeight': function () { return { 'height': '300px', 'focusStateEnabled': false, 'wrapperAttr': { 'class': 'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay' }, 'position': $context.$root.rtl ? { 'my': 'right top', 'at': 'right bottom', 'of': $context.$data.popupService.target } : { 'my': 'left top', 'at': 'left bottom', 'of': $context.$data.popupService.target }, 'container': $context.$root.getPopupContainer($element), 'target': $context.$data.popupService.target, 'showTitle': false, 'showCloseButton': false, 'animation': {}, 'hideOnOutsideClick': true, 'shading': false, 'minWidth': '170px', 'maxWidth': '500px', 'width': 'auto', 'visible': $context.$data.popupService.visible }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: masterQueries', function ($context, $element) { return { 'foreach': function () { return $context.$data.masterQueries; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-masterDetail-editor-relation\', foreach: relations }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-masterDetail-editor-relation', 'foreach': $context.$data.relations }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { title: $data.getTitle() }, text: queryName', function ($context, $element) { return { 'attr': function () { return { 'title': $context.$data.getTitle() }; }, 'text': function () { return $context.$data.queryName; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('service: { name: \'setColumn\' }', function ($context, $element) { return { 'service': function () { return { 'name': 'setColumn' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxclick: function() {  $parent.remove($data); }', function ($context, $element) { return { 'dxclick': function () { return function () { $context.$parent.remove($context.$data); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: relationName, onFocusOut: function() { isEditable(false); } }, focus: { on: isEditable }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data.relationName, 'onFocusOut': function () { $context.$data.isEditable(false); } }; }, 'focus': function () { return { 'on': $context.$data.isEditable }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: relationName, click: function() { isEditable(true); }', function ($context, $element) { return { 'text': function () { return $context.$data.relationName; }, 'click': function () { return function () { $context.$data.isEditable(true); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxclick: create', function ($context, $element) { return { 'dxclick': function () { return $context.$data.create; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: keyColumns', function ($context, $element) { return { 'foreach': function () { return $context.$data.keyColumns; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-masterDetail-editor-keyColumn\', data: master }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-masterDetail-editor-keyColumn', 'data': $context.$data.master }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-masterDetail-editor-keyColumn\', data: detail }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-masterDetail-editor-keyColumn', 'data': $context.$data.detail }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: target.column() || target.selectColumnText(), dxclick: showPopup, css: { \'dxd-state-selected\': target.isSelected, \'default\': !target.column() }', function ($context, $element) { return { 'text': function () { return $context.$data.target.column() || $context.$data.target.selectColumnText(); }, 'dxclick': function () { return $context.$data.showPopup; }, 'css': function () { return { 'dxd-state-selected': $context.$data.target.isSelected, 'default': !$context.$data.target.column() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxdTableView: $data.value', function ($context, $element) { return { 'dxdTableView': function () { return $context.$data.value; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: { animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, wrapperAttr: { class: \'dxqb-preview\' }, visible: isVisible, title: title(), showTitle: true, resizeEnabled: true, shading: true, shadingColor: \'transparent\', fullScreen: false, width: 800, height: 544, container: container($element), position: { of: container($element) }, onHidden: function() { $data.data.value(null) }, focusStateEnabled: false }', function ($context, $element) { return { 'dxPopup': function () { return { 'animation': { 'show': { 'type': 'fade', 'from': 0, 'to': 1, 'duration': 700 }, 'hide': { 'type': 'fade', 'from': 1, 'to': 0, 'duration': 700 } }, 'wrapperAttr': { 'class': 'dxqb-preview' }, 'visible': $context.$data.isVisible, 'title': $context.$data.title(), 'showTitle': true, 'resizeEnabled': true, 'shading': true, 'shadingColor': 'transparent', 'fullScreen': false, 'width': 800, 'height': 544, 'container': $context.$data.container($element), 'position': { 'of': $context.$data.container($element) }, 'onHidden': function () { $context.$data.data.value(null); }, 'focusStateEnabled': false }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxLoadIndicator: { visible: true }', function ($context, $element) { return { 'dxLoadIndicator': function () { return { 'visible': true }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { text: okButtonText(), onClick: okButtonHandler, disabled: isLoading }', function ($context, $element) { return { 'dxButton': function () { return { 'text': $context.$data.okButtonText(), 'onClick': $context.$data.okButtonHandler, 'disabled': $context.$data.isLoading }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextArea: { value: value, valueChangeEvent: \'keyup\', readOnly: true }', function ($context, $element) { return { 'dxTextArea': function () { return { 'value': $context.$data.value, 'valueChangeEvent': 'keyup', 'readOnly': true }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxAceEditor: { value: value, options: aceOptions, additionalOptions: additionalOptions }', function ($context, $element) { return { 'dxAceEditor': function () { return { 'value': $context.$data.value, 'options': $context.$data.aceOptions, 'additionalOptions': $context.$data.additionalOptions }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: data && !isLoading()', function ($context, $element) { return { 'if': function () { return $context.$data.data && !$context.$data.isLoading(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: template, data: data }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.template, 'data': $context.$data.data }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { class: \'dxd-tableview-title-cell dxd-border-secondary dxd-tableview-resizable\' }', function ($context, $element) { return { 'attr': function () { return { 'class': 'dxd-tableview-title-cell dxd-border-secondary dxd-tableview-resizable' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: name', function ($context, $element) { return { 'text': function () { return $context.$data.name; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { direction: \'both\', scrollByContent: false, showScrollbar: \'always\', scrollByThumb: true, bounceEnabled: false, useNative: false, onScroll: $data.onDataScroll, onInitialized: $data.onDataScrollInitialized }', function ($context, $element) { return { 'dxScrollView': function () { return { 'direction': 'both', 'scrollByContent': false, 'showScrollbar': 'always', 'scrollByThumb': true, 'bounceEnabled': false, 'useNative': false, 'onScroll': $context.$data.onDataScroll, 'onInitialized': $context.$data.onDataScrollInitialized }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data, attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }', function ($context, $element) { return { 'text': function () { return $context.$data; }, 'attr': function () { return { 'class': 'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable' + $context.$index() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parents[1].getImageTooLargeText($data), attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }', function ($context, $element) { return { 'text': function () { return $context.$parents[1].getImageTooLargeText($context.$data); }, 'attr': function () { return { 'class': 'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable' + $context.$index() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }', function ($context, $element) { return { 'attr': function () { return { 'class': 'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable' + $context.$index() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { src: \'data:image/bmp;base64,\' + $data }', function ($context, $element) { return { 'attr': function () { return { 'src': 'data:image/bmp;base64,' + $context.$data }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: noDataText()', function ($context, $element) { return { 'text': function () { return $context.$data.noDataText(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: rtl ? data.schema.reverse() : data.schema', function ($context, $element) { return { 'foreach': function () { return $context.$data.rtl ? $context.$data.data.schema.reverse() : $context.$data.data.schema; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: data.values', function ($context, $element) { return { 'if': function () { return $context.$data.data.values; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: data.values', function ($context, $element) { return { 'foreach': function () { return $context.$data.data.values; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: $parent.rtl ? $data.reverse() : $data', function ($context, $element) { return { 'foreach': function () { return $context.$parent.rtl ? $context.$data.reverse() : $context.$data; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: $parents[1].isImage($index())', function ($context, $element) { return { 'ifnot': function () { return $context.$parents[1].isImage($context.$index()); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $parents[1].isImage($index())', function ($context, $element) { return { 'if': function () { return $context.$parents[1].isImage($context.$index()); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $parents[1].isImageTooLarge($data)', function ($context, $element) { return { 'if': function () { return $context.$parents[1].isImageTooLarge($context.$data); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: $parents[1].isImageTooLarge($data)', function ($context, $element) { return { 'ifnot': function () { return $context.$parents[1].isImageTooLarge($context.$data); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: data.values[0]', function ($context, $element) { return { 'foreach': function () { return $context.$data.data.values[0]; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: data.values', function ($context, $element) { return { 'ifnot': function () { return $context.$data.data.values; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: treeListOptions', function ($context, $element) { return { 'treelist': function () { return $context.$data.treeListOptions; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treeListSearchPanel: { controllers: $data.fieldListModel.treeListOptions().treeListController }', function ($context, $element) { return { 'treeListSearchPanel': function () { return { 'controllers': $context.$data.fieldListModel.treeListOptions().treeListController }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', scrollByContent: false, scrollByThumb: true, bounceEnabled: false, useNative: false }', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'scrollByContent': false, 'scrollByThumb': true, 'bounceEnabled': false, 'useNative': false }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCollectionEditor: $data', function ($context, $element) { return { 'dxCollectionEditor': function () { return $context.$data; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'dx-image-expanded\': !collapsed }, template: \'dxrd-svg-collapsed\'', function ($context, $element) { return { 'css': function () { return { 'dx-image-expanded': !$context.$data.collapsed }; }, 'template': function () { return 'dxrd-svg-collapsed'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $root.getLocalization(value.name), attr: { title: $root.getLocalization(value.name) }', function ($context, $element) { return { 'text': function () { return $context.$root.getLocalization($context.$data.value.name); }, 'attr': function () { return { 'title': $context.$root.getLocalization($context.$data.value.name) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-propertieseditor\', data: getProperties({ level: 1 }) }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-propertieseditor', 'data': $context.$data.getProperties({ 'level': 1 }) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: properties.getEditors()', function ($context, $element) { return { 'foreach': function () { return $context.$data.properties.getEditors(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: "dxqb-fieldlist-wrapper", data: fieldListModel }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxqb-fieldlist-wrapper', 'data': $context.$data.fieldListModel }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: getEditors()', function ($context, $element) { return { 'foreach': function () { return $context.$data.getEditors(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('click: $root.findControl', function ($context, $element) { return { 'click': function () { return $context.$root.findControl; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: displayName, attr: { \'title\': displayName }', function ($context, $element) { return { 'text': function () { return $context.$data.displayName; }, 'attr': function () { return { 'title': $context.$data.displayName }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('lazy: { template: templateName }', function ($context, $element) { return { 'lazy': function () { return { 'template': $context.$data.templateName }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxExpressionEditor: getOptions({ options: value, fieldListProvider: $root.parametersBindingsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() })', function ($context, $element) { return { 'dxExpressionEditor': function () { return $context.$data.getOptions({ 'options': $context.$data.value, 'fieldListProvider': $context.$root.parametersBindingsProvider, 'displayNameProvider': $context.$root.displayNameProvider && $context.$root.displayNameProvider() }); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.value', function ($context, $element) { return { 'if': function () { return $context.$data.value; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: target.isEditable', function ($context, $element) { return { 'visible': function () { return $context.$data.target.isEditable; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: target._parameterName, onFocusOut: function() { target.createParameter(); target.isEditable(false); } }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data.target._parameterName, 'onFocusOut': function () { $context.$data.target.createParameter(); $context.$data.target.isEditable(false); } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: target.parameterName, dxclick: showPopup, css: { \'dxd-state-selected\': target.isSelected, \'default\': target.isDefaultTextDisplayed() }, visible: $data.target.isEditable() === false', function ($context, $element) { return { 'text': function () { return $context.$data.target.parameterName; }, 'dxclick': function () { return $context.$data.showPopup; }, 'css': function () { return { 'dxd-state-selected': $context.$data.target.isSelected, 'default': $context.$data.target.isDefaultTextDisplayed() }; }, 'visible': function () { return $context.$data.target.isEditable() === false; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: name, attr: { \'title\': name }, click: function() { $parents[1].click($data); }', function ($context, $element) { return { 'text': function () { return $context.$data.name; }, 'attr': function () { return { 'title': $context.$data.name }; }, 'click': function () { return function () { $context.$parents[1].click($context.$data); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parent.viewModel.defaultDisplay(), click: function() { $parent.viewModel.isEditable(true); $parent.viewModel._parameterName(\'\'); $parent.visible(false); }', function ($context, $element) { return { 'text': function () { return $context.$parent.viewModel.defaultDisplay(); }, 'click': function () { return function () { $context.$parent.viewModel.isEditable(true); $context.$parent.viewModel._parameterName(''); $context.$parent.visible(false); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: name, click: function() { $parent.click($data); }', function ($context, $element) { return { 'text': function () { return $context.$data.name; }, 'click': function () { return function () { $context.$parent.click($context.$data); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $parent.viewModel.canCreateParameters', function ($context, $element) { return { 'if': function () { return $context.$parent.viewModel.canCreateParameters; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: { dblclick: events.dblclick }, click: toggleSelected, draggable: isDraggable ? dragDropHandler : null, styleunit: padding, css: { \'dx-treelist-item-selected dxd-state-selected\': isSelected || isMultiSelected }', function ($context, $element) { return { 'event': function () { return { 'dblclick': $context.$data.events.dblclick }; }, 'click': function () { return $context.$data.toggleSelected; }, 'draggable': function () { return $context.$data.isDraggable ? $context.$data.dragDropHandler : null; }, 'styleunit': function () { return $context.$data.padding; }, 'css': function () { return { 'dx-treelist-item-selected dxd-state-selected': $context.$data.isSelected || $context.$data.isMultiSelected }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: "dxqb-treelist-header-item-with-search"', function ($context, $element) { return { 'template': function () { return 'dxqb-treelist-header-item-with-search'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: $data._extendCssClass(\'type-page\')', function ($context, $element) { return { 'css': function () { return $context.$data._extendCssClass('type-page'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: { click: $parent._itemClick, dblclick: function() { $parent._goToNextPage() } }, attr: { class: \'dxd-back-highlighted dxd-state-normal dxrd-wizard-type-item dx-fontsize-reestablished dxrd-wizard-type-item-border-color dxd-list-item-back-color \' + $parent._extendCssClass(\'type-item\')}, css: { \'dxd-border-secondary dxd-back-secondary\': $parent._IsSelected($data) }', function ($context, $element) { return { 'event': function () { return { 'click': $context.$parent._itemClick, 'dblclick': function () { $context.$parent._goToNextPage(); } }; }, 'attr': function () { return { 'class': 'dxd-back-highlighted dxd-state-normal dxrd-wizard-type-item dx-fontsize-reestablished dxrd-wizard-type-item-border-color dxd-list-item-back-color ' + $context.$parent._extendCssClass('type-item') }; }, 'css': function () { return { 'dxd-border-secondary dxd-back-secondary': $context.$parent._IsSelected($context.$data) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { class: \'dxrd-wizard-type-image \' + $parent._extendCssClass(\'type-image\') }, css: imageClassName, template: { name: imageTemplateName }', function ($context, $element) { return { 'attr': function () { return { 'class': 'dxrd-wizard-type-image ' + $context.$parent._extendCssClass('type-image') }; }, 'css': function () { return $context.$data.imageClassName; }, 'template': function () { return { 'name': $context.$data.imageTemplateName }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('searchHighlighting: { text: text, textToSearch: $parent._textToSearch }, attr: { title: text }, css: $parent._extendCssClass(\'type-text\')', function ($context, $element) { return { 'searchHighlighting': function () { return { 'text': $context.$data.text, 'textToSearch': $context.$parent._textToSearch }; }, 'attr': function () { return { 'title': $context.$data.text }; }, 'css': function () { return $context.$parent._extendCssClass('type-text'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: text, attr: { title: text }, css: $parent._extendCssClass(\'type-text\')', function ($context, $element) { return { 'text': function () { return $context.$data.text; }, 'attr': function () { return { 'title': $context.$data.text }; }, 'css': function () { return $context.$parent._extendCssClass('type-text'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: typeItems', function ($context, $element) { return { 'foreach': function () { return $context.$data.typeItems; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-page-choose-type-item\'', function ($context, $element) { return { 'template': function () { return 'dxrd-page-choose-type-item'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !!$parent._textToSearch', function ($context, $element) { return { 'if': function () { return !!$context.$parent._textToSearch; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: !!$parent._textToSearch', function ($context, $element) { return { 'ifnot': function () { return !!$context.$parent._textToSearch; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treeListSearchPanel: {controllers: ko.unwrap(_fieldListModel).treeListController}', function ($context, $element) { return { 'treeListSearchPanel': function () { return { 'controllers': $context.ko.unwrap($context.$data._fieldListModel).treeListController }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight }', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'height': $context.$data._scrollViewHeight }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: _fieldListModel', function ($context, $element) { return { 'treelist': function () { return $context.$data._fieldListModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxLoadPanel: _loadPanelViewModel($element)', function ($context, $element) { return { 'dxLoadPanel': function () { return $context.$data._loadPanelViewModel($element); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: $data.imageTemplateName, iconClass: $data.imageClassName , disabled: $data.disabled && $data.disabled() }, attr: { title: text }', function ($context, $element) { return { 'dxButtonWithTemplate': function () { return { 'onClick': function () { $context.$data.clickAction($context.$parent); }, 'icon': $context.$data.imageTemplateName, 'iconClass': $context.$data.imageClassName, 'disabled': $context.$data.disabled && $context.$data.disabled() }; }, 'attr': function () { return { 'title': $context.$data.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-popover-list-items\', data: ko.utils.extend($parent.data, { className: \'dxrd-federation-addqueries-popover\', position: $root.rtl ? \'right\' : \'left\', popupContainer: $root.getPopupContainer, width: 150 }) }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-popover-list-items', 'data': $context.ko.utils.extend($context.$parent.data, { 'className': 'dxrd-federation-addqueries-popover', 'position': $context.$root.rtl ? 'right' : 'left', 'popupContainer': $context.$root.getPopupContainer, 'width': 150 }) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('resizable: gridResizeHelper.resizable($root.resizeHandler, \'s\')', function ($context, $element) { return { 'resizable': function () { return $context.$data.gridResizeHelper.resizable($context.$root.resizeHandler, 's'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxQueryBuilderSurface: queryBuilderSurfaceCreator', function ($context, $element) { return { 'dxQueryBuilderSurface': function () { return $context.$data.queryBuilderSurfaceCreator; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxdAccordion: { collapsed: joinResultCollapsed }', function ($context, $element) { return { 'dxdAccordion': function () { return { 'collapsed': $context.$data.joinResultCollapsed }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !joinResultCollapsed() }', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-collapsed'; }, 'css': function () { return { 'dx-image-expanded': !$context.$data.joinResultCollapsed() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $root.getLocalization(\'Join result\', \'DataAccessUIStringId.FederationDataSourceQueryBuilder_SelectResult\')', function ($context, $element) { return { 'text': function () { return $context.$root.getLocalization('Join result', 'DataAccessUIStringId.FederationDataSourceQueryBuilder_SelectResult'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('style: { height: resultGridHeight }', function ($context, $element) { return { 'style': function () { return { 'height': $context.$data.resultGridHeight }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { icon: \'add\', disabled: addRowDisabled, onClick:addRow }', function ($context, $element) { return { 'dxButton': function () { return { 'icon': 'add', 'disabled': $context.$data.addRowDisabled, 'onClick': $context.$data.addRow }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: columnsGrid', function ($context, $element) { return { 'dxDataGrid': function () { return $context.$data.columnsGrid; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: cellInfo.column', function ($context, $element) { return { 'text': function () { return $context.$data.cellInfo.column; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDropDownBox: { dataSource: $data.allColumns, dropDownOptions: { container: $root.getPopupContainer($element), width:  $root.calculateWithZoomFactor(357)  }, popupPosition: { boundary: $root.getPopupContainer($element) }, value: cellInfo.key, valueExpr: \'key\', displayExpr: \'column\' }, visible: !$data.isExpression()', function ($context, $element) { return { 'dxDropDownBox': function () { return { 'dataSource': $context.$data.allColumns, 'dropDownOptions': { 'container': $context.$root.getPopupContainer($element), 'width': $context.$root.calculateWithZoomFactor(357) }, 'popupPosition': { 'boundary': $context.$root.getPopupContainer($element) }, 'value': $context.$data.cellInfo.key, 'valueExpr': 'key', 'displayExpr': 'column' }; }, 'visible': function () { return !$context.$data.isExpression(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: { dataSource: $data.allColumns, keyExpr: \'key\', remoteOperations: true, height: $root.calculateWithZoomFactor(250), columns: [{ dataField: \'column\', sortOrder: \'asc\' } , { dataField: \'table\'}], hoverStateEnabled: true, selection: { mode: \'single\' }, selectedRowKeys: [cellInfo.key], focusedRowEnabled: true, focusedRowKey: cellInfo.key, onSelectionChanged: $data.changeColumn }', function ($context, $element) { return { 'dxDataGrid': function () { return { 'dataSource': $context.$data.allColumns, 'keyExpr': 'key', 'remoteOperations': true, 'height': $context.$root.calculateWithZoomFactor(250), 'columns': [{ 'dataField': 'column', 'sortOrder': 'asc' }, { 'dataField': 'table' }], 'hoverStateEnabled': true, 'selection': { 'mode': 'single' }, 'selectedRowKeys': [$context.$data.cellInfo.key], 'focusedRowEnabled': true, 'focusedRowKey': $context.$data.cellInfo.key, 'onSelectionChanged': $context.$data.changeColumn }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxExpressionEditor: { options: expression, fieldListProvider: itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() }, visible: $data.isExpression', function ($context, $element) { return { 'dxExpressionEditor': function () { return { 'options': $context.$data.expression, 'fieldListProvider': $context.$data.itemsProvider, 'displayNameProvider': $context.$root.displayNameProvider && $context.$root.displayNameProvider() }; }, 'visible': function () { return $context.$data.isExpression; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-wizard-menu-box-editorswitch\', data: $data }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-wizard-menu-box-editorswitch', 'data': $context.$data }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data.transformGridTitle', function ($context, $element) { return { 'text': function () { return $context.$data.transformGridTitle; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: transformGrid', function ($context, $element) { return { 'dxDataGrid': function () { return $context.$data.transformGrid; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxdAccordion: { collapsed: transformResultCollapsed }', function ($context, $element) { return { 'dxdAccordion': function () { return { 'collapsed': $context.$data.transformResultCollapsed }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !transformResultCollapsed() }', function ($context, $element) { return { 'template': function () { return 'dxrd-svg-collapsed'; }, 'css': function () { return { 'dx-image-expanded': !$context.$data.transformResultCollapsed() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data.transformResultGridTitle', function ($context, $element) { return { 'text': function () { return $context.$data.transformResultGridTitle; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: resultFieldListModel', function ($context, $element) { return { 'treelist': function () { return $context.$data.resultFieldListModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCheckBox: transform', function ($context, $element) { return { 'dxCheckBox': function () { return $context.$data.transform; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: { showTitle: true, title: title(), wrapperAttr: { class: cssClass }, visible: popupVisible, toolbarItems: buttonItems, showCloseButton: true, fullScreen: false, maxHeight: maxHeight, height: height, width: width, onShowing: function(e) { $data.loaded(true); }, rtlEnabled: rtl, onHiding: onHiding, container: getPopupContainer($element), position: { of: getPopupContainer($element)}, resizeEnabled: true, minHeight: 450, minWidth: 650 }', function ($context, $element) { return { 'dxPopup': function () { return { 'showTitle': true, 'title': $context.$data.title(), 'wrapperAttr': { 'class': $context.$data.cssClass }, 'visible': $context.$data.popupVisible, 'toolbarItems': $context.$data.buttonItems, 'showCloseButton': true, 'fullScreen': false, 'maxHeight': $context.$data.maxHeight, 'height': $context.$data.height, 'width': $context.$data.width, 'onShowing': function (e) { $context.$data.loaded(true); }, 'rtlEnabled': $context.$data.rtl, 'onHiding': $context.$data.onHiding, 'container': $context.$data.getPopupContainer($element), 'position': { 'of': $context.$data.getPopupContainer($element) }, 'resizeEnabled': true, 'minHeight': 450, 'minWidth': 650 }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('resizable: resizeHelper.resizable($root.resizeHandler, \'e\')', function ($context, $element) { return { 'resizable': function () { return $context.$data.resizeHelper.resizable($context.$root.resizeHandler, 'e'); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: fieldListModel', function ($context, $element) { return { 'treelist': function () { return $context.$data.fieldListModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: sourcesGrid', function ($context, $element) { return { 'dxDataGrid': function () { return $context.$data.sourcesGrid; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: aliasGrid', function ($context, $element) { return { 'dxDataGrid': function () { return $context.$data.aliasGrid; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: loaded', function ($context, $element) { return { 'if': function () { return $context.$data.loaded; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: popupContentTemplate }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.popupContentTemplate }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'dx-rtl\': $root.rtl, \'dx-ltr\': !$root.rtl }', function ($context, $element) { return { 'css': function () { return { 'dx-rtl': $context.$root.rtl, 'dx-ltr': !$context.$root.rtl }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: _extendCssClass', function ($context, $element) { return { 'css': function () { return $context.$data._extendCssClass; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: $data.navigationPanel().isVisible', function ($context, $element) { return { 'visible': function () { return $context.$data.navigationPanel().isVisible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _description(), attr: {\'title\': _description() }', function ($context, $element) { return { 'text': function () { return $context.$data._description(); }, 'attr': function () { return { 'title': $context.$data._description() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: _steps', function ($context, $element) { return { 'foreach': function () { return $context.$data._steps; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: $data.visible', function ($context, $element) { return { 'visible': function () { return $context.$data.visible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('click: $data.clickAction, text: $data.text, attr: {\'title\': $data.text }, css: { \'dxrd-disabled\': $data.disabled, \'dxd-back-secondary\': $data.isActive() }', function ($context, $element) { return { 'click': function () { return $context.$data.clickAction; }, 'text': function () { return $context.$data.text; }, 'attr': function () { return { 'title': $context.$data.text }; }, 'css': function () { return { 'dxrd-disabled': $context.$data.disabled, 'dxd-back-secondary': $context.$data.isActive() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: $data.isActive', function ($context, $element) { return { 'visible': function () { return $context.$data.isActive; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'withoutPanel\': !$data.navigationPanel().isVisible() }', function ($context, $element) { return { 'css': function () { return { 'withoutPanel': !$context.$data.navigationPanel().isVisible() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _pageDescription()', function ($context, $element) { return { 'text': function () { return $context.$data._pageDescription(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: template, data: page }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.template, 'data': $context.$data.page }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxAILoadingPanel: _currentPage().page._customLoadPanelViewModel($element, $data.isLoading)', function ($context, $element) { return { 'dxAILoadingPanel': function () { return $context.$data._currentPage().page._customLoadPanelViewModel($element, $context.$data.isLoading); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: cancelButton', function ($context, $element) { return { 'dxButton': function () { return $context.$data.cancelButton; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: finishButton', function ($context, $element) { return { 'dxButton': function () { return $context.$data.finishButton; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: nextButton', function ($context, $element) { return { 'dxButton': function () { return $context.$data.nextButton; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: previousButton', function ($context, $element) { return { 'dxButton': function () { return $context.$data.previousButton; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: _sections, css: _className, styleunit: { top: _parentMarginOffset, left: _parentMarginOffset, right: _parentMarginOffset, bottom: _parentMarginOffset }', function ($context, $element) { return { 'foreach': function () { return $context.$data._sections; }, 'css': function () { return $context.$data._className; }, 'styleunit': function () { return { 'top': $context.$data._parentMarginOffset, 'left': $context.$data._parentMarginOffset, 'right': $context.$data._parentMarginOffset, 'bottom': $context.$data._parentMarginOffset }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'dxrd-disabled\': !$data.page() }, style: $parent._pageCss[$data.pageId]', function ($context, $element) { return { 'css': function () { return { 'dxrd-disabled': !$context.$data.page() }; }, 'style': function () { return $context.$parent._pageCss[$context.$data.pageId]; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: $parent._showPageDescription($data), text: $parent._getPageDescription($index(), $data), attr: { title: $parent._getPageDescription($index(), $data) }', function ($context, $element) { return { 'visible': function () { return $context.$parent._showPageDescription($context.$data); }, 'text': function () { return $context.$parent._getPageDescription($context.$index(), $context.$data); }, 'attr': function () { return { 'title': $context.$parent._getPageDescription($context.$index(), $context.$data) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: $parent.metadata.template, data: page }, dxScrollView: { showScrollbar: \'onHover\'}', function ($context, $element) { return { 'template': function () { return { 'name': $context.$parent.metadata.template, 'data': $context.$data.page }; }, 'dxScrollView': function () { return { 'showScrollbar': 'onHover' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: metadata.disabledText', function ($context, $element) { return { 'text': function () { return $context.$data.metadata.disabledText; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data && $data.isVisible()', function ($context, $element) { return { 'if': function () { return $context.$data && $context.$data.isVisible(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: navigationPanel', function ($context, $element) { return { 'with': function () { return $context.$data.navigationPanel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: _currentPage', function ($context, $element) { return { 'with': function () { return $context.$data._currentPage; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !(_currentPage() && _currentPage().page._customLoadPanelViewModel)', function ($context, $element) { return { 'if': function () { return !($context.$data._currentPage() && $context.$data._currentPage().page._customLoadPanelViewModel); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: _currentPage() && _currentPage().page._customLoadPanelViewModel', function ($context, $element) { return { 'if': function () { return $context.$data._currentPage() && $context.$data._currentPage().page._customLoadPanelViewModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data._sections.length > 0', function ($context, $element) { return { 'if': function () { return $context.$data._sections.length > 0; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.page() !== null', function ($context, $element) { return { 'if': function () { return $context.$data.page() !== null; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: page', function ($context, $element) { return { 'with': function () { return $context.$data.page; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.page() === null', function ($context, $element) { return { 'if': function () { return $context.$data.page() === null; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _rootElementTitle', function ($context, $element) { return { 'text': function () { return $context.$data._rootElementTitle; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxSelectBox: { dataSource: _rootElementList, value: _selectedRootElement, displayExpr: \'fullPath\', displayCustomValue: true, useItemTextAsTitle: true, dropDownOptions: { container: $root.getPopupContainer($element) } }', function ($context, $element) { return { 'dxSelectBox': function () { return { 'dataSource': $context.$data._rootElementList, 'value': $context.$data._selectedRootElement, 'displayExpr': 'fullPath', 'displayCustomValue': true, 'useItemTextAsTitle': true, 'dropDownOptions': { 'container': $context.$root.getPopupContainer($element) } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true }, dxValidationGroup: $data._validationGroup || {}', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'useNative': false, 'scrollByThumb': true }; }, 'dxValidationGroup': function () { return $context.$data._validationGroup || {}; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _jsonConnectionTitle, attr: { \'title\': _jsonConnectionTitle }', function ($context, $element) { return { 'text': function () { return $context.$data._jsonConnectionTitle; }, 'attr': function () { return { 'title': $context.$data._jsonConnectionTitle }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: _connectionName }, dxValidator: { validationRules: $data._connectionNameValidationRules || [] }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data._connectionName }; }, 'dxValidator': function () { return { 'validationRules': $context.$data._connectionNameValidationRules || [] }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _jsonSourceTitle, attr: { \'title\': _jsonSourceTitle }', function ($context, $element) { return { 'text': function () { return $context.$data._jsonSourceTitle; }, 'attr': function () { return { 'title': $context.$data._jsonSourceTitle }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxLocalizedSelectBox: { dataSource: _sources, value: _selectedSource, valueExpr:\'value\', displayExpr: \'displayValue\', displayCustomValue: true, dropDownOptions: { container: $root.getPopupContainer($element) } }', function ($context, $element) { return { 'dxLocalizedSelectBox': function () { return { 'dataSource': $context.$data._sources, 'value': $context.$data._selectedSource, 'valueExpr': 'value', 'displayExpr': 'displayValue', 'displayCustomValue': true, 'dropDownOptions': { 'container': $context.$root.getPopupContainer($element) } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxValidationGroup: $data.validationGroup || {}', function ($context, $element) { return { 'dxValidationGroup': function () { return $context.$data.validationGroup || {}; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: $data.cssClass', function ($context, $element) { return { 'css': function () { return $context.$data.cssClass; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxValidationSummary: $data.validationSummary || {}, visible: $data.validationSummary && !isValid()', function ($context, $element) { return { 'dxValidationSummary': function () { return $context.$data.validationSummary || {}; }, 'visible': function () { return $context.$data.validationSummary && !$context.$data.isValid(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxValidationSummary: $data._validationSummary || {}, visible: $data._validationSummary && _selectedSource().validationSummary && !canNext()', function ($context, $element) { return { 'dxValidationSummary': function () { return $context.$data._validationSummary || {}; }, 'visible': function () { return $context.$data._validationSummary && $context.$data._selectedSource().validationSummary && !$context.$data.canNext(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('with: _selectedSource', function ($context, $element) { return { 'with': function () { return $context.$data._selectedSource; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-propertieseditor\', data: grid }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-propertieseditor', 'data': $context.$data.grid }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxRadioGroup: { value: selectedDataSourceOperation, items: dataSourceOperations, layout: \'vertical\' }', function ($context, $element) { return { 'dxRadioGroup': function () { return { 'value': $context.$data.selectedDataSourceOperation, 'items': $context.$data.dataSourceOperations, 'layout': 'vertical' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: text, attr: { \'title\': text }', function ($context, $element) { return { 'text': function () { return $context.$data.text; }, 'attr': function () { return { 'title': $context.$data.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: availableDataSources, selectedItems: selectedDataSource, focusStateEnabled:false, editEnabled: true, height: dataSourcesListHeight, editConfig: { selectionEnabled: false }, selectionMode: \'single\', activeStateEnabled: false, disabled: createNewDataSource, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data.availableDataSources, 'selectedItems': $context.$data.selectedDataSource, 'focusStateEnabled': false, 'editEnabled': true, 'height': $context.$data.dataSourcesListHeight, 'editConfig': { 'selectionEnabled': false }, 'selectionMode': 'single', 'activeStateEnabled': false, 'disabled': $context.$data.createNewDataSource, 'noDataText': $context.$root.dx._static.noDataText(), 'encodeNoDataText': true }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: isDataSourceCreationAvailable', function ($context, $element) { return { 'if': function () { return $context.$data.isDataSourceCreationAvailable; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTemplate : { name: \'item\' }', function () { return { dxTemplate: { name: 'item' } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxRadioGroup: { value: selectedOperation, items: operations, layout: \'vertical\' }', function ($context, $element) { return { 'dxRadioGroup': function () { return { 'value': $context.$data.selectedOperation, 'items': $context.$data.operations, 'layout': 'vertical' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { top: $data.canCreateNew() ? 65 : 0 }', function ($context, $element) { return { 'styleunit': function () { return { 'top': $context.$data.canCreateNew() ? 65 : 0 }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: items, onSelectionChanged: function(e) { selectedItems(e.addedItems) }, selectedItems: selectedItems.peek(), hoverStateEnabled:false, editEnabled: true, height: \'100%\', editConfig: { selectionEnabled: false }, searchExpr: \'name\', searchEnabled: true, selectionMode: \'single\', activeStateEnabled: false, disabled: _createNew, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data.items, 'onSelectionChanged': function (e) { $context.$data.selectedItems(e.addedItems); }, 'selectedItems': $context.$data.selectedItems.peek(), 'hoverStateEnabled': false, 'editEnabled': true, 'height': '100%', 'editConfig': { 'selectionEnabled': false }, 'searchExpr': 'name', 'searchEnabled': true, 'selectionMode': 'single', 'activeStateEnabled': false, 'disabled': $context.$data._createNew, 'noDataText': $context.$root.dx._static.noDataText(), 'encodeNoDataText': true, 'searchEditorOptions': { 'placeholder': $context.$root.dx._static.searchPlaceholder() } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: { dblclick: function(e){ $parent.onDblClick() } }', function ($context, $element) { return { 'event': function () { return { 'dblclick': function (e) { $context.$parent.onDblClick(); } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parent._displayExpr($data)', function ($context, $element) { return { 'text': function () { return $context.$parent._displayExpr($context.$data); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-page-selectitems-radio-group\', data: $data }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-page-selectitems-radio-group', 'data': $context.$data }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-page-selectitems-list\', data: $data }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-page-selectitems-list', 'data': $context.$data }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: canCreateNew', function ($context, $element) { return { 'if': function () { return $context.$data.canCreateNew; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !_createNew()', function ($context, $element) { return { 'if': function () { return !$context.$data._createNew(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: _createNew', function ($context, $element) { return { 'if': function () { return $context.$data._createNew; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-page-jsonsource\', data: _specifySourceData }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-page-jsonsource', 'data': $context.$data._specifySourceData }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxLocalizedSelectBox: { dataSource: _parameterTypes, value: itemType, valueExpr:\'value\', displayExpr: \'displayValue\', displayCustomValue: true, dropDownOptions: { container: $root.getPopupContainer($element) } }', function ($context, $element) { return { 'dxLocalizedSelectBox': function () { return { 'dataSource': $context.$data._parameterTypes, 'value': $context.$data.itemType, 'valueExpr': 'value', 'displayExpr': 'displayValue', 'displayCustomValue': true, 'dropDownOptions': { 'container': $context.$root.getPopupContainer($element) } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: name, placeholder: $data.namePlaceholder() }, dxValidator: { validationRules: nameValidationRules || [] }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data.name, 'placeholder': $context.$data.namePlaceholder() }; }, 'dxValidator': function () { return { 'validationRules': $context.$data.nameValidationRules || [] }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: _editingValue, placeholder: $data.valuePlaceholder() }, visible: !$data.isExpression()', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data._editingValue, 'placeholder': $context.$data.valuePlaceholder() }; }, 'visible': function () { return !$context.$data.isExpression(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxExpressionEditor: { options: _expression, fieldListProvider: itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() }, visible: $data.isExpression', function ($context, $element) { return { 'dxExpressionEditor': function () { return { 'options': $context.$data._expression, 'fieldListProvider': $context.$data.itemsProvider, 'displayNameProvider': $context.$root.displayNameProvider && $context.$root.displayNameProvider() }; }, 'visible': function () { return $context.$data.isExpression; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxFileImagePicker: { value: value, placeholderId: \'File\', accept:\'.json,.txt\' }', function ($context, $element) { return { 'dxFileImagePicker': function () { return { 'value': $context.$data.value, 'placeholderId': 'File', 'accept': '.json,.txt' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextArea: { value: value, spellcheck: false, isValid: isValid }, dxValidator: $data.validator || { validationRules: jsonStringValidationRules || [] }', function ($context, $element) { return { 'dxTextArea': function () { return { 'value': $context.$data.value, 'spellcheck': false, 'isValid': $context.$data.isValid }; }, 'dxValidator': function () { return $context.$data.validator || { 'validationRules': $context.$data.jsonStringValidationRules || [] }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dragDropJsonToDataSource, dxAceEditor: { value: value, editorContainer: editorContainer, options: aceOptions, additionalOptions: additionalOptions }, css: { \'dx-invalid\' : !value() || !isValid(), \'dxrd-drop-zone-border\': showDropBorders }', function ($context, $element) { return { 'dragDropJsonToDataSource': function () { return undefined; }, 'dxAceEditor': function () { return { 'value': $context.$data.value, 'editorContainer': $context.$data.editorContainer, 'options': $context.$data.aceOptions, 'additionalOptions': $context.$data.additionalOptions }; }, 'css': function () { return { 'dx-invalid': !$context.$data.value() || !$context.$data.isValid(), 'dxrd-drop-zone-border': $context.$data.showDropBorders }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButtonWithTemplate: { onClick: uploadFile, hint: $data.getUploadTitle(), icon: \'dxrd-svg-wizard-Download\' }', function ($context, $element) { return { 'dxButtonWithTemplate': function () { return { 'onClick': $context.$data.uploadFile, 'hint': $context.$data.getUploadTitle(), 'icon': 'dxrd-svg-wizard-Download' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { icon: \'add\', onClick: _addCollection, disabled: _addQueryDisabled }', function ($context, $element) { return { 'dxButton': function () { return { 'icon': 'add', 'onClick': $context.$data._addCollection, 'disabled': $context.$data._addQueryDisabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxDataGrid: _collectionsGrid', function ($context, $element) { return { 'dxDataGrid': function () { return $context.$data._collectionsGrid; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxFilterEditor: { options: _filterEditorOptions, template: \'dx-filtereditor-popup\', popupVisible: _filterEditorVisible, fieldListProvider: _filedListProvider, getDisplayNameByPath: $root.getDisplayNameByPath, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider(), aiExpressionGenerator: $root.aiExpressionGenerator }', function ($context, $element) { return { 'dxFilterEditor': function () { return { 'options': $context.$data._filterEditorOptions, 'template': 'dx-filtereditor-popup', 'popupVisible': $context.$data._filterEditorVisible, 'fieldListProvider': $context.$data._filedListProvider, 'getDisplayNameByPath': $context.$root.getDisplayNameByPath, 'displayNameProvider': $context.$root.displayNameProvider && $context.$root.displayNameProvider(), 'aiExpressionGenerator': $context.$root.aiExpressionGenerator }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxEllipsisEditor: { value: value, buttonAction: buttonAction, disabled: disabled }', function ($context, $element) { return { 'dxEllipsisEditor': function () { return { 'value': $context.$data.value, 'buttonAction': $context.$data.buttonAction, 'disabled': $context.$data.disabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: _filterEditorVisible', function ($context, $element) { return { 'if': function () { return $context.$data._filterEditorVisible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: _querySelectBoxTitle', function ($context, $element) { return { 'text': function () { return $context.$data._querySelectBoxTitle; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxSelectBox: { dataSource: _availableQueries, value: _selectedQueryName, disabled: _isLoading, useItemTextAsTitle: true, dropDownOptions: { container: $root.getPopupContainer($element) } }', function ($context, $element) { return { 'dxSelectBox': function () { return { 'dataSource': $context.$data._availableQueries, 'value': $context.$data._selectedQueryName, 'disabled': $context.$data._isLoading, 'useItemTextAsTitle': true, 'dropDownOptions': { 'container': $context.$root.getPopupContainer($element) } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: _isLoading', function ($context, $element) { return { 'if': function () { return $context.$data._isLoading; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: _isLoading', function ($context, $element) { return { 'ifnot': function () { return $context.$data._isLoading; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: padding, click: toggleSelected, css: { \'dx-treelist-item-selected dxd-state-selected dxd-back-secondary\': isSelected }', function ($context, $element) { return { 'styleunit': function () { return $context.$data.padding; }, 'click': function () { return $context.$data.toggleSelected; }, 'css': function () { return { 'dx-treelist-item-selected dxd-state-selected dxd-back-secondary': $context.$data.isSelected }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: nodeImageClass', function ($context, $element) { return { 'css': function () { return $context.$data.nodeImageClass; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: imageTemplateName, iconClass: imageClassName, disabled: ko.unwrap(disabled) }, attr: { title: text }', function ($context, $element) { return { 'dxButtonWithTemplate': function () { return { 'onClick': function () { $context.$data.clickAction($context.$parent); }, 'icon': $context.$data.imageTemplateName, 'iconClass': $context.$data.imageClassName, 'disabled': $context.ko.unwrap($context.$data.disabled) }; }, 'attr': function () { return { 'title': $context.$data.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCheckBox: { value: data.checked }, click: function(treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }', function ($context, $element) { return { 'dxCheckBox': function () { return { 'value': $context.$data.data.checked }; }, 'click': function () { return function (treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: queryName, onValueChanged: queryNameHasChanged }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data.queryName, 'onValueChanged': $context.$data.queryNameHasChanged }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: $data.disableCustomSql', function ($context, $element) { return { 'ifnot': function () { return $context.$data.disableCustomSql; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxqb-popup-selectStatment\', data: _popupSelectStatement }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxqb-popup-selectStatment', 'data': $context.$data._popupSelectStatement }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-querybuilder-popup\', data: _popupQueryBuilder }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-querybuilder-popup', 'data': $context.$data._popupQueryBuilder }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !!$data._fieldListModel()', function ($context, $element) { return { 'if': function () { return !!$context.$data._fieldListModel(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data._relationsEditor()', function ($context, $element) { return { 'if': function () { return $context.$data._relationsEditor(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-masterDetail-editor-complete-wizard\', data: $data._relationsEditor }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-masterDetail-editor-complete-wizard', 'data': $context.$data._relationsEditor }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopupWithAutoHeight: { height: \'235px\', focusStateEnabled: false, position: $root.rtl ? { my: \'right top\', at: \'right bottom\', of: popupService.target, collision: \'flipfit\' } : { my: \'left top\', at: \'left bottom\', of: popupService.target, collision: \'flipfit\' }, wrapperAttr: { class: \'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay\' }, container: $root.getPopupContainer($element), target: popupService.target, showTitle: false, showCloseButton: false, animation: {}, hideOnOutsideClick: true, shading: false, minWidth:\'170px\', maxWidth:\'500px\', width: \'auto\', visible: popupService.visible }', function ($context, $element) { return { 'dxPopupWithAutoHeight': function () { return { 'height': '235px', 'focusStateEnabled': false, 'position': $context.$root.rtl ? { 'my': 'right top', 'at': 'right bottom', 'of': $context.$data.popupService.target, 'collision': 'flipfit' } : { 'my': 'left top', 'at': 'left bottom', 'of': $context.$data.popupService.target, 'collision': 'flipfit' }, 'wrapperAttr': { 'class': 'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay' }, 'container': $context.$root.getPopupContainer($element), 'target': $context.$data.popupService.target, 'showTitle': false, 'showCloseButton': false, 'animation': {}, 'hideOnOutsideClick': true, 'shading': false, 'minWidth': '170px', 'maxWidth': '500px', 'width': 'auto', 'visible': $context.$data.popupService.visible }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: { animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, wrapperAttr: { class: \'dxqb-preview\' }, visible: isVisible, title: title(), showTitle: true, resizeEnabled: true, shading: true, shadingColor: \'transparent\', fullScreen: false, width: 800, height: 544, container: closest($element, \'.dxrd-wizard\'), position: { of: closest($element, \'.dx-designer-viewport\') }, onHidden: function() { $data.data(null) }, focusStateEnabled: false }', function ($context, $element) { return { 'dxPopup': function () { return { 'animation': { 'show': { 'type': 'fade', 'from': 0, 'to': 1, 'duration': 700 }, 'hide': { 'type': 'fade', 'from': 1, 'to': 0, 'duration': 700 } }, 'wrapperAttr': { 'class': 'dxqb-preview' }, 'visible': $context.$data.isVisible, 'title': $context.$data.title(), 'showTitle': true, 'resizeEnabled': true, 'shading': true, 'shadingColor': 'transparent', 'fullScreen': false, 'width': 800, 'height': 544, 'container': $context.$data.closest($element, '.dxrd-wizard'), 'position': { 'of': $context.$data.closest($element, '.dx-designer-viewport') }, 'onHidden': function () { $context.$data.data(null); }, 'focusStateEnabled': false }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextArea: { value: data, valueChangeEvent: \'keyup\', disabled: false }', function ($context, $element) { return { 'dxTextArea': function () { return { 'value': $context.$data.data, 'valueChangeEvent': 'keyup', 'disabled': false }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxAceEditor: { value: data, additionalOptions: additionalOptions, options: aceOptions }', function ($context, $element) { return { 'dxAceEditor': function () { return { 'value': $context.$data.data, 'additionalOptions': $context.$data.additionalOptions, 'options': $context.$data.aceOptions }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { text: okButtonText(), onClick: okButtonHandler }', function ($context, $element) { return { 'dxButton': function () { return { 'text': $context.$data.okButtonText(), 'onClick': $context.$data.okButtonHandler }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('event: events, click: toggleSelected, draggable: isDraggable ? dragDropHandler : null, attr: { class: \'dx-treeview-item dx-treelist-item dx-fontsize-reestablished dxd-list-item-back-color \' + (hasItems ? \'dx-treelist-list-item-content \' : \'dx-treelist-field-item-content \') + (isSelected ? \'dx-treelist-item-selected dxd-state-selected dxd-back-secondary\' : \'\') }, styleunit: padding, css: { \'dx-state-hover\': isHovered }', function ($context, $element) { return { 'event': function () { return $context.$data.events; }, 'click': function () { return $context.$data.toggleSelected; }, 'draggable': function () { return $context.$data.isDraggable ? $context.$data.dragDropHandler : null; }, 'attr': function () { return { 'class': 'dx-treeview-item dx-treelist-item dx-fontsize-reestablished dxd-list-item-back-color ' + ($context.$data.hasItems ? 'dx-treelist-list-item-content ' : 'dx-treelist-field-item-content ') + ($context.$data.isSelected ? 'dx-treelist-item-selected dxd-state-selected dxd-back-secondary' : '') }; }, 'styleunit': function () { return $context.$data.padding; }, 'css': function () { return { 'dx-state-hover': $context.$data.isHovered }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: imageClassName, template: { name: imageTemplateName, if: !!imageTemplateName  }, attr: { title: text }', function ($context, $element) { return { 'css': function () { return $context.$data.imageClassName; }, 'template': function () { return { 'name': $context.$data.imageTemplateName, 'if': !!$context.$data.imageTemplateName }; }, 'attr': function () { return { 'title': $context.$data.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: nodeImageClass, visible: hasItems, template: \'dxrd-svg-collapsed\', click: toggleCollapsed', function ($context, $element) { return { 'css': function () { return $context.$data.nodeImageClass; }, 'visible': function () { return $context.$data.hasItems; }, 'template': function () { return 'dxrd-svg-collapsed'; }, 'click': function () { return $context.$data.toggleCollapsed; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'dxrd-disabled\': data.disabled }', function ($context, $element) { return { 'css': function () { return { 'dxrd-disabled': $context.$data.data.disabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCheckBox: { value: data.checked, disabled: data.selectionDisabled }, click: function(treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }', function ($context, $element) { return { 'dxCheckBox': function () { return { 'value': $context.$data.data.checked, 'disabled': $context.$data.data.selectionDisabled }; }, 'click': function () { return function (treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: imageTemplateName, iconClass: imageClassName , disabled: ko.unwrap(disabled) }, attr: { title: text }', function ($context, $element) { return { 'dxButtonWithTemplate': function () { return { 'onClick': function () { $context.$data.clickAction($context.$parent); }, 'icon': $context.$data.imageTemplateName, 'iconClass': $context.$data.imageClassName, 'disabled': $context.ko.unwrap($context.$data.disabled) }; }, 'attr': function () { return { 'title': $context.$data.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPropertyGrid: { target: $parent.data.dataSourceParameter, level: $parent.data.editor.level + 1 }', function ($context, $element) { return { 'dxPropertyGrid': function () { return { 'target': $context.$parent.data.dataSourceParameter, 'level': $context.$parent.data.editor.level + 1 }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: "dx-treelist-accordion-item-with-checkbox"', function ($context, $element) { return { 'template': function () { return 'dx-treelist-accordion-item-with-checkbox'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: "dx-treelist-header-item-with-checkbox"', function ($context, $element) { return { 'template': function () { return 'dx-treelist-header-item-with-checkbox'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: showIcon', function ($context, $element) { return { 'if': function () { return $context.$data.showIcon; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('ifnot: showIcon', function ($context, $element) { return { 'ifnot': function () { return $context.$data.showIcon; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-popover-list-items\', data: ko.utils.extend($parent.data, { position: $root.rtl ? \'left\' : \'right\', popupContainer: $root.getPopupContainer }) }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-popover-list-items', 'data': $context.ko.utils.extend($context.$parent.data, { 'position': $context.$root.rtl ? 'left' : 'right', 'popupContainer': $context.$root.getPopupContainer }) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: _chooseObjectDataMember.dataMembers, selectedItems: _chooseObjectDataMember.selectedDataMembers, selectionMode: \'single\', activeStateEnabled: false, focusStateEnabled: false, searchExpr: \'displayName\', searchEnabled: true, noDataText: $dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data._chooseObjectDataMember.dataMembers, 'selectedItems': $context.$data._chooseObjectDataMember.selectedDataMembers, 'selectionMode': 'single', 'activeStateEnabled': false, 'focusStateEnabled': false, 'searchExpr': 'displayName', 'searchEnabled': true, 'noDataText': $context.$data.$dx._static.noDataText(), 'encodeNoDataText': true, 'searchEditorOptions': { 'placeholder': $context.$root.dx._static.searchPlaceholder() } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treeListSearchPanel: {controllers: _chooseObjectType.availableTypesTreelistModel.treeListController}', function ($context, $element) { return { 'treeListSearchPanel': function () { return { 'controllers': $context.$data._chooseObjectType.availableTypesTreelistModel.treeListController }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', height: _chooseObjectType._scrollViewHeight}', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'height': $context.$data._chooseObjectType._scrollViewHeight }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: _chooseObjectType.availableTypesTreelistModel', function ($context, $element) { return { 'treelist': function () { return $context.$data._chooseObjectType.availableTypesTreelistModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $root.dx._static.noDataText()', function ($context, $element) { return { 'text': function () { return $context.$root.dx._static.noDataText(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: _chooseObjectType.types && !_chooseObjectType.types().length', function ($context, $element) { return { 'if': function () { return $context.$data._chooseObjectType.types && !$context.$data._chooseObjectType.types().length; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('style: position, css: { \'dxrd-disabled\': $data.disabled() }', function ($context, $element) { return { 'style': function () { return $context.$data.position; }, 'css': function () { return { 'dxrd-disabled': $context.$data.disabled() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parent.showDescription($index(), description), attr: { title: $parent.showDescription($index(), description) }', function ($context, $element) { return { 'text': function () { return $context.$parent.showDescription($context.$index(), $context.$data.description); }, 'attr': function () { return { 'title': $context.$parent.showDescription($context.$index(), $context.$data.description) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: disabledText', function ($context, $element) { return { 'text': function () { return $context.$data.disabledText; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treeListSearchPanel: {controllers: availableTypesTreelistModel.treeListController}', function ($context, $element) { return { 'treeListSearchPanel': function () { return { 'controllers': $context.$data.availableTypesTreelistModel.treeListController }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight}', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'height': $context.$data._scrollViewHeight }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('treelist: availableTypesTreelistModel', function ($context, $element) { return { 'treelist': function () { return $context.$data.availableTypesTreelistModel; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: dataMembers, selectedItems: selectedDataMembers, selectionMode: \'single\', activeStateEnabled: false, focusStateEnabled: false, searchExpr: \'displayName\', searchEnabled: true, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data.dataMembers, 'selectedItems': $context.$data.selectedDataMembers, 'selectionMode': 'single', 'activeStateEnabled': false, 'focusStateEnabled': false, 'searchExpr': 'displayName', 'searchEnabled': true, 'noDataText': $context.$root.dx._static.noDataText(), 'encodeNoDataText': true, 'searchEditorOptions': { 'placeholder': $context.$root.dx._static.searchPlaceholder() } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data.displayName, attr: { \'title\': $data.displayName}', function ($context, $element) { return { 'text': function () { return $context.$data.displayName; }, 'attr': function () { return { 'title': $context.$data.displayName }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxExpressionEditor: getOptions({ options: value, fieldListProvider: value().itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() })', function ($context, $element) { return { 'dxExpressionEditor': function () { return $context.$data.getOptions({ 'options': $context.$data.value, 'fieldListProvider': $context.$data.value().itemsProvider, 'displayNameProvider': $context.$root.displayNameProvider && $context.$root.displayNameProvider() }); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\'}', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: _sections', function ($context, $element) { return { 'foreach': function () { return $context.$data._sections; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !$data.disabled()', function ($context, $element) { return { 'if': function () { return !$context.$data.disabled(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: $data.disabled()', function ($context, $element) { return { 'if': function () { return $context.$data.disabled(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: !!$data', function ($context, $element) { return { 'if': function () { return !!$context.$data; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('foreach: _grids', function ($context, $element) { return { 'foreach': function () { return $context.$data._grids; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-objectdatasource-parameters-grid\', data: _ctorParametersObject }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-objectdatasource-parameters-grid', 'data': $context.$data._ctorParametersObject }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-objectdatasource-parameters-grid\', data: _dataMemberParametersObject }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-objectdatasource-parameters-grid', 'data': $context.$data._dataMemberParametersObject }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dx-objectdatasource-parameters-section\', data: _chooseObjectParameters }', function ($context, $element) { return { 'template': function () { return { 'name': 'dx-objectdatasource-parameters-section', 'data': $context.$data._chooseObjectParameters }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxCollectionEditor: parametersEditorOptions', function ($context, $element) { return { 'dxCollectionEditor': function () { return $context.$data.parametersEditorOptions; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxdAccordion: $data', function ($context, $element) { return { 'dxdAccordion': function () { return $context.$data; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('styleunit: { \'marginLeft\' : padding }, css: { \'dxrd-group-header-collapsed\': collapsed }', function ($context, $element) { return { 'styleunit': function () { return { 'marginLeft': $context.$data.padding }; }, 'css': function () { return { 'dxrd-group-header-collapsed': $context.$data.collapsed }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: value.name', function ($context, $element) { return { 'text': function () { return $context.$data.value.name; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true  }', function ($context, $element) { return { 'dxScrollView': function () { return { 'showScrollbar': 'onHover', 'useNative': false, 'scrollByThumb': true }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxclick: select, css: { \'dxd-state-selected dxd-back-secondary\' : selected }', function ($context, $element) { return { 'dxclick': function () { return $context.$data.select; }, 'css': function () { return { 'dxd-state-selected dxd-back-secondary': $context.$data.selected }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: showButtons', function ($context, $element) { return { 'visible': function () { return $context.$data.showButtons; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { onClick: buttons.add.action, text: buttons.add.text }, attr: { title: buttons.add.text }', function ($context, $element) { return { 'dxButton': function () { return { 'onClick': $context.$data.buttons.add.action, 'text': $context.$data.buttons.add.text }; }, 'attr': function () { return { 'title': $context.$data.buttons.add.text }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { onClick: buttons.delete.action, disabled: selectedIndex === null, text: $parent.removeButtonTitle }, attr: { title: $parent.removeButtonTitle }', function ($context, $element) { return { 'dxButton': function () { return { 'onClick': $context.$data.buttons.delete.action, 'disabled': $context.$data.selectedIndex === null, 'text': $context.$parent.removeButtonTitle }; }, 'attr': function () { return { 'title': $context.$parent.removeButtonTitle }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: _connectionStrings, onSelectionChanged: function(e) { _selectedConnectionString(e.addedItems) },  selectedItems: _selectedConnectionString.peek(), focusStateEnabled: false, searchEnabled: true, searchExpr: \'description\', editEnabled: true, editConfig: { selectionEnabled: true }, selectionMode: \'single\', activeStateEnabled: false, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data._connectionStrings, 'onSelectionChanged': function (e) { $context.$data._selectedConnectionString(e.addedItems); }, 'selectedItems': $context.$data._selectedConnectionString.peek(), 'focusStateEnabled': false, 'searchEnabled': true, 'searchExpr': 'description', 'editEnabled': true, 'editConfig': { 'selectionEnabled': true }, 'selectionMode': 'single', 'activeStateEnabled': false, 'noDataText': $context.$root.dx._static.noDataText(), 'encodeNoDataText': true, 'searchEditorOptions': { 'placeholder': $context.$root.dx._static.searchPlaceholder() } }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data[\'description\'] || $data[\'name\']', function ($context, $element) { return { 'text': function () { return $context.$data['description'] || $context.$data['name']; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: caption()', function ($context, $element) { return { 'text': function () { return $context.$data.caption(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('visible: !runQueryBuilderDisabled, text: $parent.runQueryBuilderBtnText, click: function() { $parent.runQueryBuilder() }', function ($context, $element) { return { 'visible': function () { return !$context.$data.runQueryBuilderDisabled; }, 'text': function () { return $context.$parent.runQueryBuilderBtnText; }, 'click': function () { return function () { $context.$parent.runQueryBuilder(); }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextArea: { value: sqlString, valueChangeEvent: \'keyup input blur\', readOnly: disableCustomSql() }', function ($context, $element) { return { 'dxTextArea': function () { return { 'value': $context.$data.sqlString, 'valueChangeEvent': 'keyup input blur', 'readOnly': $context.$data.disableCustomSql() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxAceEditor: { value: sqlString, additionalOptions: additionalOptions, options: aceOptions }, css: { \'dx-disabled-ace\': disableCustomSql() }', function ($context, $element) { return { 'dxAceEditor': function () { return { 'value': $context.$data.sqlString, 'additionalOptions': $context.$data.additionalOptions, 'options': $context.$data.aceOptions }; }, 'css': function () { return { 'dx-disabled-ace': $context.$data.disableCustomSql() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxList: { dataSource: storedProcedures, searchEnabled: true, searchExpr: \'name\', onContentReady: scrollActiveItem, selectedItems: selectedProcedure, editEnabled: true, editConfig: { selectionEnabled: true }, selectionMode: \'single\', activeStateEnabled: false, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true }', function ($context, $element) { return { 'dxList': function () { return { 'dataSource': $context.$data.storedProcedures, 'searchEnabled': true, 'searchExpr': 'name', 'onContentReady': $context.$data.scrollActiveItem, 'selectedItems': $context.$data.selectedProcedure, 'editEnabled': true, 'editConfig': { 'selectionEnabled': true }, 'selectionMode': 'single', 'activeStateEnabled': false, 'noDataText': $context.$root.dx._static.noDataText(), 'encodeNoDataText': true }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parent.generateStoredProcedureDisplayName($data)', function ($context, $element) { return { 'text': function () { return $context.$parent.generateStoredProcedureDisplayName($context.$data); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: queryNameCaption()', function ($context, $element) { return { 'text': function () { return $context.$data.queryNameCaption(); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxTextBox: { value: queryName, placeholder: $data.placeholder() }', function ($context, $element) { return { 'dxTextBox': function () { return { 'value': $context.$data.queryName, 'placeholder': $context.$data.placeholder() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxRadioGroup: { value: selectedQueryType, layout: \'horizontal\', items: queryTypeItems }', function ($context, $element) { return { 'dxRadioGroup': function () { return { 'value': $context.$data.selectedQueryType, 'layout': 'horizontal', 'items': $context.$data.queryTypeItems }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $parent.localizeQueryType($data), attr: { \'title\': $parent.localizeQueryType($data) }', function ($context, $element) { return { 'text': function () { return $context.$parent.localizeQueryType($context.$data); }, 'attr': function () { return { 'title': $context.$parent.localizeQueryType($context.$data) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: queryControl().template, data: queryControl() }', function ($context, $element) { return { 'template': function () { return { 'name': $context.$data.queryControl().template, 'data': $context.$data.queryControl() }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: { name: \'dxrd-querybuilder-popup\', data: popupQueryBuilder }', function ($context, $element) { return { 'template': function () { return { 'name': 'dxrd-querybuilder-popup', 'data': $context.$data.popupQueryBuilder }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: popupViewModel($element)', function ($context, $element) { return { 'dxPopup': function () { return $context.$data.popupViewModel($element); } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxQueryBuilder: qbOptions', function ($context, $element) { return { 'dxQueryBuilder': function () { return $context.$data.qbOptions; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { text: getDisplayText(\'previewResults\'), onClick: previewHandler, disabled: okButtonDisabled }', function ($context, $element) { return { 'dxButton': function () { return { 'text': $context.$data.getDisplayText('previewResults'), 'onClick': $context.$data.previewHandler, 'disabled': $context.$data.okButtonDisabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { text: getDisplayText(\'cancel\'), onClick: cancelHandler }', function ($context, $element) { return { 'dxButton': function () { return { 'text': $context.$data.getDisplayText('cancel'), 'onClick': $context.$data.cancelHandler }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxButton: { text: getDisplayText(\'ok\'), type: \'default\', onClick: okHandler, disabled: okButtonDisabled }', function ($context, $element) { return { 'dxButton': function () { return { 'text': $context.$data.getDisplayText('ok'), 'type': 'default', 'onClick': $context.$data.okHandler, 'disabled': $context.$data.okButtonDisabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxLoadPanel:{ animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, deferRendering: false, message: getDisplayText(\'loading\'), visible: showLoadIndicator, shading: true, shadingColor: \'transparent\'}', function ($context, $element) { return { 'dxLoadPanel': function () { return { 'animation': { 'show': { 'type': 'fade', 'from': 0, 'to': 1, 'duration': 700 }, 'hide': { 'type': 'fade', 'from': 1, 'to': 0, 'duration': 700 } }, 'deferRendering': false, 'message': $context.$data.getDisplayText('loading'), 'visible': $context.$data.showLoadIndicator, 'shading': true, 'shadingColor': 'transparent' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('template: \'dxrd-designer\'', function ($context, $element) { return { 'template': function () { return 'dxrd-designer'; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: qbOptions', function ($context, $element) { return { 'if': function () { return $context.$data.qbOptions; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('if: isVisible', function ($context, $element) { return { 'if': function () { return $context.$data.isVisible; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('css: { \'dxd-state-active\': $data.isExpression() }, dxButtonWithTemplate: { onClick: switchEditors, icon: \'dxrd-svg-wizard-expressions\', disabled: disabled }', function ($context, $element) { return { 'css': function () { return { 'dxd-state-active': $context.$data.isExpression() }; }, 'dxButtonWithTemplate': function () { return { 'onClick': $context.$data.switchEditors, 'icon': 'dxrd-svg-wizard-expressions', 'disabled': $context.$data.disabled }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopover: { wrapperAttr: { class: $data.className }, width: $data.width || 200, position: position, visible: popoverVisible, target: target, container: popupContainer($element)}', function ($context, $element) { return { 'dxPopover': function () { return { 'wrapperAttr': { 'class': $context.$data.className }, 'width': $context.$data.width || 200, 'position': $context.$data.position, 'visible': $context.$data.popoverVisible, 'target': $context.$data.target, 'container': $context.$data.popupContainer($element) }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { class: $data.className + \'-list\'}, dxList: { dataSource: popoverListItems(), onItemClick: itemClickAction }', function ($context, $element) { return { 'attr': function () { return { 'class': $context.$data.className + '-list' }; }, 'dxList': function () { return { 'dataSource': $context.$data.popoverListItems(), 'onItemClick': $context.$data.itemClickAction }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('attr: { class: $data.className + \'-list-item\'}', function ($context, $element) { return { 'attr': function () { return { 'class': $context.$data.className + '-list-item' }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: name, attr: { title: name }', function ($context, $element) { return { 'text': function () { return $context.$data.name; }, 'attr': function () { return { 'title': $context.$data.name }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: $data.title || $parent.title', function ($context, $element) { return { 'text': function () { return $context.$data.title || $context.$parent.title; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('text: description, attr: { title: description }', function ($context, $element) { return { 'text': function () { return $context.$data.description; }, 'attr': function () { return { 'title': $context.$data.description }; } }; });
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.addToBindingsCache)('dxPopup: { animation: { show: { type: \'fadeIn\', duration: 700 }, hide: { type: \'fadeOut\', duration: 700 } }, visible: isVisible, wrapperAttr: { class: \'dxrd-wizard dx-editors dxd-text-primary\' }, title: isVisible() ? title : \'\', showTitle: true, fullScreen: false, width: width, height: height, container: $element, visualContainer: _container($element), titleTemplate: _titleTemplate, position: _wizardPopupPosition($element) }, cssArray: [ $data._extendCssClass, { \'dx-rtl\': $root.rtl, \'dx-ltr\': !$root.rtl }]', function ($context, $element) { return { 'dxPopup': function () { return { 'animation': { 'show': { 'type': 'fadeIn', 'duration': 700 }, 'hide': { 'type': 'fadeOut', 'duration': 700 } }, 'visible': $context.$data.isVisible, 'wrapperAttr': { 'class': 'dxrd-wizard dx-editors dxd-text-primary' }, 'title': $context.$data.isVisible() ? $context.$data.title : '', 'showTitle': true, 'fullScreen': false, 'width': $context.$data.width, 'height': $context.$data.height, 'container': $element, 'visualContainer': $context.$data._container($element), 'titleTemplate': $context.$data._titleTemplate, 'position': $context.$data._wizardPopupPosition($element) }; }, 'cssArray': function () { return [$context.$data._extendCssClass, { 'dx-rtl': $context.$root.rtl, 'dx-ltr': !$context.$root.rtl }]; } }; });

;// ./dist/dx-query-builder-templates.generated.js

external_DevExpress_Analytics_Widgets_Internal_namespaceObject.SvgTemplatesEngine.addTemplates({
    'dxdd-connection-line': '<svg class="dxdd-connection-line dxrd-width-100 dxrd-height-100" > <line class="dxd-qb-relationship-line-color" data-bind="attr: { x1: startPoint().relativeX, y1: startPoint().relativeY, x2: endPoint().relativeX, y2: endPoint().relativeY }" /> </svg>',
    'dxdd-connecting-point': '<div class="dxdd-connecting-point" data-bind="styleunit: position, trackCursor: underCursor, style: { \'marginLeft\': -3 * _context.zoom() + \'px\', \'marginTop\': -3 * _context.zoom() + \'px\' }, draggable: $root.connectingPointDragHandler"> <svg class="dxrd-width-100 dxrd-height-100"> <line x1="0" y1="0" x2="100%" y2="100%" /> <line x1=0 y1="100%" x2="100%" y2="0" /> </svg> </div>',
    'dxdd-connection-point-selection': '<div class="dxrd-control" data-bind="draggable: $root.connectionPointDragHandler, styleunit: { top: relativeY, left: relativeX }"> </div>',
    'dxdd-connector': '<div class="dxdd-connector" data-bind="styleunit: position, trackCursor: underCursor"> <!-- ko template: { name: \'dxdd-connection-line\' } --> <!-- /ko --> </div>',
    'dxdd-connector-selection': '<div class="dxdd-connector" data-bind="styleunit: position, dxclick: function() {}, draggable: $root.dragHandler"> <!-- ko template: { name: \'dxdd-connection-line\' } --> <!-- /ko --> <!-- ko with: startPoint --> <!-- ko template: { name: \'dxdd-connection-point-selection\' } --> <!-- /ko --> <!-- /ko --> <!-- ko with: endPoint --> <!-- ko template: { name: \'dxdd-connection-point-selection\' } --> <!-- /ko --> <!-- /ko --> </div>',
    'dxdd-routed-connection-line': '<svg class="dxdd-connection-line"> <defs> <!-- ko if: showArrow --> <marker data-bind="attr: { id: \'dxqb-arrow_end\' + connectorID() }" viewBox="0 0 5 10" refX="5" refY="5" markerUnits="userSpaceOnUse" orient="auto" markerWidth="14" markerHeight="14"> <polyline class="dxd-qb-relationship-line-color" points="0,0 5,5 0,10" /> </marker> <!-- /ko --> <!-- ko if: showRightArrow --> <marker data-bind="attr: { id: \'dxqb-arrow_start\' + connectorID() }" viewBox="0 0 5 10" refX="5" refY="5" markerUnits="userSpaceOnUse" orient="auto-start-reverse" markerWidth="14" markerHeight="14"> <polyline class="dxd-qb-relationship-line-color" points="0,0 5,5 0,10" /> </marker> <!-- /ko --> </defs> <polyline class="dxd-qb-relationship-line-color" fill="none" data-bind="attr: { points: routePointsSet, \'marker-end\': showArrow() ? \'url(#dxqb-arrow_end\' + connectorID() + \')\' : \'\', \'marker-start\': showRightArrow() ? \'url(#dxqb-arrow_start\' + connectorID() + \')\' : \'\' }" /> </svg>',
    'dxdd-routed-connector': '<div class="dxdd-connector" data-bind="styleunit: position, visible: isVisible"> <!-- ko template: { name: \'dxdd-routed-connection-line\' } --> <!-- /ko --> <!-- ko foreach: routeLineWrappers --> <div data-bind="styleunit: position" class="dxrd-position-absolute"> <!-- ko with: $parent --> <div class="dxd-selectable dxrd-position-relative dxrd-width-100 dxrd-height-100" data-bind="trackCursor: underCursor, click: $root.selectItemProperties" ></div> <!-- /ko --> </div> <!-- /ko --> </div>',
    'dxdd-routed-connector-selection': '<div class="dxdd-connector dxrd-selected dxd-state-selected" data-bind="styleunit: position, dxclick: function() {}, visible: isVisible"> <!-- ko template: { name: \'dxdd-routed-connection-line\' } --> <!-- /ko --> <!-- ko foreach: routeLineWrappers --> <!-- ko ifnot: isLocked --> <div data-bind="styleunit: position, style: { cursor: isVerticalLine ? \'ew-resize\' : \'ns-resize\' }, routeLineDraggable: { starting: $root.resizeHandler.starting, stopped: function() { resizeStopped(); $root.resizeHandler.stopped(); }, forceResize: resizeHandler }" class="dxrd-position-absolute"> </div> <!-- /ko --> <!-- ko if: isLocked --> <div data-bind="styleunit: position" class="dxrd-position-absolute"> </div> <!-- /ko --> <!-- /ko --> <!-- ko with: startPoint --> <!-- ko template: { name: \'dxdd-connection-point-selection\' } --> <!-- /ko --> <!-- /ko --> <!-- ko with: endPoint --> <!-- ko template: { name: \'dxdd-connection-point-selection\' } --> <!-- /ko --> <!-- /ko --> </div>',
    'dx-diagram-element': '<div class="dxrd-control dxd-selectable" data-bind="styleunit: position, trackCursor: underCursor"> <div class="dxrd-control-content-main" data-bind="styleunit: { lineHeight: positionLineHeightWithoutMargins }, style: css"> <div data-bind="template: contenttemplate, styleunit: { \'height\': positionLineHeightWithoutMargins, \'width\': positionWidthWithoutMargins }"> </div> </div> </div>',
    'dx-diagram-element-selection': '<div class="dxrd-control" data-bind="event: { dblclick: $root.inlineTextEdit.show($element) }, css: {\'dxrd-selected\': selected, \'dxrd-focused\': focused }, resizable: $root.resizeHandler, draggable: $root.dragHandler, styleunit: position, trackCursor: underCursor"> <!-- ko if: !$root.inlineTextEdit.visible() --> <div class="dxrd-control-content-main" data-bind="styleunit: { lineHeight: positionLineHeightWithoutMargins }, style: css"> <div data-bind="template: contenttemplate, styleunit: { \'height\': positionLineHeightWithoutMargins, \'width\': positionWidthWithoutMargins}"> </div> </div> <!-- /ko --> <!-- ko if: $root.inlineTextEdit.visible --> <!-- ko with: $root.inlineTextEdit --> <div class="inline-text-edit" data-bind="dxTextArea: { value: text, onKeyUp: keypressAction, valueChangeEvent: \'keyup\' }"></div> <!-- /ko --> <!-- /ko --> </div>',
    'dx-diagram-element-content': '<div class="dxrd-width-100 dxrd-height-100 dxrd-border-1px-solid-black"> <div data-bind="text: getControlModel().text"></div> </div>',
    'dxdd-element-content-with-connecting-points': '<div class="dxrd-width-100 dxrd-height-100 dxrd-overflow-hidden"> <svg class="dxrd-top-0 dxrd-left-0 dxrd-width-100 dxrd-height-100 dxrd-stroke-black dxrd-fill-transparent dxrd-stroke-width-1"> <!-- ko if: getControlModel().type() === \'Ellipse\' --> <ellipse cx="50%" cy="50%" rx="50%" ry="50%" /> <!-- /ko --> <!-- ko if: getControlModel().type() === \'Condition\' --> <line x1="50%" y1="0" x2="100%" y2="50%" /> <line x1="100%" y1="50%" x2="50%" y2="100%" /> <line x1="50%" y1="100%" x2="0" y2="50%" /> <line x1="0" y1="50%" x2="50%" y2="0" /> <!-- /ko --> <!-- ko if: !getControlModel().type() --> <rect height="100%" width="100%" /> <!-- /ko --> <text x="50%" y="50%" alignment-baseline="middle" class="dxrd-text-anchor-middle dxrd-cursor-default" data-bind="text: getControlModel().text"></text> </svg> <!-- ko foreach: connectingPoints --> <!-- ko template: \'dxdd-connecting-point\' --> <!-- /ko --> <!-- /ko --> </div>',
    'dx-diagram-surface': '<div class="dxrd-surface dxrd-height-100 dxrd-display-inline-block"  data-bind="styleunit: { \'width\': Math.min(pageWidth(), $root.surfaceSize()) }, click: function(_, e) { $root.selection.clickHandler(null, e); e.stopPropagation(); }, keyDownActions: $root.actionLists.getViewModel()"> <div class="dxrd-viewport dxrd-overflow-auto dxrd-width-inherit dxrd-height-inherit"  data-bind="styleunit: { minWidth: Math.min(pageWidth(), $root.surfaceSize()), maxWidth: pageWidth(),  maxHeight: pageHeight() + 20 }"> <div data-bind="selectable: { selection: $root.selection, zoom: zoom }"> <div class="dxrd-ghost-containment"> <div class="dxrd-ghost-container dxrd-background-white dxrd-position-relative"  data-bind="styleunit: { \'width\': pageWidth(), \'height\': pageHeight() }, trackCursor: underCursor"> <!-- ko foreach: controls --> <!-- ko template: { name: isSelected() ? selectiontemplate : template } --> <!-- /ko --> <!-- /ko --> </div> </div> </div> </div> </div>',
    'dxqb-joincondition': '<div class="dxdd-connector" data-bind="styleunit: position, trackCursor: underCursor"> <svg class="dxrd-width-100 dxrd-height-100"> <line data-bind="attr: { x1: startPoint.x, y1: startPoint.y, x2: endPoint.x, y2: endPoint.y }" /> </svg> </div>',
    'dx-query-surface': '<div class="dxrd-surface dxrd-display-inline-block"  data-bind="styleunit: { \'width\': $root.surfaceSize }, click: function(_, e) { $root.selection.clickHandler(null, e); e.stopPropagation(); }, keyDownActions: $root.actionLists.getViewModel()"> <div class="dxrd-viewport dxrd-width-inherit dxrd-height-inherit" > <div class="dxqb-mainwin" data-bind="dxScrollView: { direction: \'both\', showScrollbar: \'always\', scrollByContent: false, scrollByThumb: true, bounceEnabled: false, useNative: true, height: \'100%\' }"> <div class="dxrd-height-100"> <div class="dxrd-ghost-containment dxrd-height-100" > <div class="dxrd-ghost-container dxqb-ghost-container" data-bind="styleunit: { \'minWidth\': pageWidth, \'minHeight\': pageHeight }, trackCursor: { recalculateBounds: true, underCursor: underCursor }"> <div class="dxqb-main" data-bind="trackCursor: { recalculateBounds: true, underCursor: underCursor }, style: { \'z-index\' : $parent.dragDropStarted() ? 2 : null }"> <!-- ko if: tables().length === 0--> <div class="dxqb-placeholder dxd-empty-area-placeholder-text-color dxd-text-info" data-bind="text: placeholder()"></div> <!-- /ko --> <!-- ko foreach: relations --> <!-- ko foreach: conditions --> <!-- ko if: !isSelected()--> <!-- ko template: template --> <!-- /ko --> <!-- /ko --> <!-- /ko --> <!-- /ko --> <!-- ko foreach: tables --> <!-- ko template: template --> <!-- /ko --> <!-- /ko --> <!-- ko foreach: relations --> <!-- ko foreach: conditions --> <!-- ko if: isSelected()--> <!-- ko template: selectiontemplate --> <!-- /ko --> <!-- /ko --> <!-- /ko --> <!-- /ko --> <!-- ko with: $root.columnDragHandler.dragDropConnector --> <svg class="dxdd-connection-line-draggable" data-bind="styleunit: { top: position.top(), left: position.left() }"> <!-- ko foreach: routePoints --> <!-- ko if: $index() < ($parent.routePoints().length - 1) --> <line class="dxd-qb-relationship-line-color" fill="none" data-bind="attr: {x1 : $data.x, y1: $data.y, x2: $parent.routePoints()[$index()+1].x, y2: $parent.routePoints()[$index()+1].y }"></line> <!-- /ko --> <!-- /ko --> </svg> <!-- /ko --> </div> </div> </div> </div> </div> </div> </div>',
    'dxqb-relation': '<!-- ko foreach: conditions --> <!-- ko template: { name: isSelected() ? selectiontemplate : template } --> <!-- /ko --> <!-- /ko -->',
    'dxqb-table-field': '<div class="dx-border-inheritor dxd-border-accented"> <div class="dxqb-table-field dxd-qb-table-field-border-color dxd-qb-table-field-back-color dxd-back-highlighted dxd-qb-table-field-separator-color dxd-border-primary" data-bind="trackCursor: { recalculateBounds: true, underCursor: underCursor }, draggable: $root.columnDragHandler, css: cssClasses($root.surface(), $root.columnDragHandler, $parent), click: $root.selectItemProperties"> <div class="dxqb-table-field-background dxd-back-accented"></div> <div class="dxqb-table-field-checkbox-wrapper"> <div class="dxqb-table-field-checkbox" data-bind="dxCheckBox: { value: selectedWrapper }, click: function(surface, e) { surface.toggleSelected(); e.stopPropagation(); return true;  }"></div> </div> <div class="dxqb-table-field-content dxd-text-primary" data-bind="attr: { title: getControlModel().actualName }"> <div class="dxqb-table-field-state"> <!-- ko if:  $data.isAscending() --> <div class="dxqb-image-field-state dxqb-image-field-sorting-asc"><!-- ko template: \'dxrd-svg-queryBuilder-sorting_asc\' --><!-- /ko --></div> <!-- /ko --> <!-- ko if:  $data.isDescending() --> <div class="dxqb-image-field-state dxqb-image-field-sorting-desc"><!-- ko template: \'dxrd-svg-queryBuilder-sorting_desc\' --><!-- /ko --></div> <!-- /ko --> <!-- ko if: $data.getControlModel().groupBy --> <div class="dxqb-image-field-state dxqb-image-field-group-by"><!-- ko template: \'dxrd-svg-queryBuilder-group_by\' --><!-- /ko --></div> <!-- /ko --> <!-- ko if: $data.isAggregate() --> <div class="dxqb-image-field-state dxqb-image-field-aggregate"><!-- ko template: \'dxrd-svg-queryBuilder-aggregate\' --><!-- /ko --></div> <!-- /ko --> </div> <div class="dxqb-table-field-caption"> <div class="dxqb-table-field-text dxd-qb-table-field-text-color" data-bind="text: getControlModel().actualName"></div> </div> </div> </div> </div>',
    'dxqb-table-asterisk-field': '<div class="dx-border-inheritor dxd-border-accented"> <div class="dxqb-table-field dxd-qb-table-field-border-color dxd-qb-table-field-back-color dxd-back-highlighted dxd-border-primary" data-bind="trackCursor: underCursor, css: cssClasses(), click: $root.selectItemProperties"> <div class="dxqb-table-field-background dxd-back-accented"></div> <div class="dxqb-table-field-checkbox-wrapper"> <div class="dxqb-table-field-checkbox" data-bind="dxCheckBox: { value: selectedWrapper }, click: function(surface, e) { surface.toggleSelected(); e.stopPropagation(); return true;  }"></div> </div> <div class="dxqb-table-field-content dxd-text-primary" data-bind="attr: { title: getControlModel().name }"> <div class="dxqb-table-field-caption"> <div class="dxqb-table-field-text dxd-qb-table-field-text-color" data-bind="text: getControlModel().name"></div> </div> </div> </div> </div>',
    'dxqb-table-main': '<!-- ko if: $data.isSelected() --> <div class="dxqb-table-resize-ghost" data-bind="resizable: resizable($root.resizeHandler, $element), styleunit: position"></div> <!-- /ko --> <div class="dxrd-control" data-bind="css: {\'dxrd-selected\': selected, \'dxrd-focused\': focused }, draggable: $root.dragHandler, styleunit: position, trackCursor: underCursor, click: $root.selectItemProperties"> <div class="dxrd-control-content-main dxd-qb-table-back-color dxd-back-primary2" data-bind="styleunit: { lineHeight: position.lineHeight }, style: css"> <div class="dxqb-table-background dxd-back-accented"></div> <div class="dx-background-inheritor dxd-back-accented"> <div class="dxqb-table-border dxd-qb-table-border-color dxd-back-highlighted dxd-state-active dxd-state-no-hover"></div> </div> <div data-bind="template: contenttemplate, styleunit: { \'height\': position.lineHeight, \'width\': position.width }"> </div> </div> </div>',
    'dxqb-table': '<div class="dxqb-table dxd-qb-table-back-color dxd-back-primary2"> <div class="dx-border-inheritor dxd-border-accented"> <div class="dxqb-table-head-field dxd-qb-table-field-border-color dxd-qb-table-field-back-color dxd-back-highlighted dxd-border-primary"> <div class="dxqb-table-checkbox-all"> <div data-bind="dxCheckBox: { value: selectedWrapper }, click: function(surface, e) { $root.undoEngine().start(); surface.toggleSelected(); $root.undoEngine().end(); e.stopPropagation(); return true; }"></div> </div> <!-- ko template: titletemplate --> <!-- /ko --> </div> </div> <div class="dxqb-table-content"> <div class="dxqb-tableFields-wrapper"> <!-- ko ifnot: $data.isInitialized() --> <div class="dxqb-table-loading"> <div class="dxqb-table-loading-text dxd-text-primary" data-bind="text: $root.columnsLoadingMsg()"></div> </div> <!-- /ko --> <!-- ko if: $data.isInitialized() --> <!-- ko template: { name: asterisk.template, data: asterisk } --> <!-- /ko --> <!-- ko foreach: columns --> <!-- ko lazy: { template: $data.template  }--> <!-- /ko --> <!-- /ko --> <!-- /ko --> </div> </div> </div>',
    'dxqb-table-title': '<div class="dxqb-table-title" data-bind="style: { cursor: selected() ? \'move\' : \'default\' }"> <div class="dxqb-table-title-content dxd-text-primary" data-bind="text: getControlModel().actualName"></div> </div>',
    'dxqb-federation-table-title': '<div class="dxqb-table-title dxqb-federation-table-title" data-bind="event: { dblclick: $root.inlineTextEdit.show($element) }, style: { cursor: selected() ? \'move\' : \'default\' }"> <div class="dxqb-federation-table-title-container"> <!-- ko if: !$root.inlineTextEdit.visible() || !selected()  --> <div class="dxqb-table-title-content dxd-text-primary" data-bind="text: getControlModel().actualName"></div> <!-- /ko --> <!-- ko if: $root.inlineTextEdit.visible() && selected() --> <!-- ko with: $root.inlineTextEdit --> <div class="inline-text-edit" data-bind="dxTextArea: { value: text, onKeyUp: keypressAction, valueChangeEvent: \'keyup\' }"></div> <!-- /ko --> <!-- /ko --> </div> <div class="dxqb-table-subtitle dxd-text-info" data-bind="text: getControlModel().displaySourceName"></div> </div>',
    'dxrd-masterDetail-editor': '<!-- ko if: $data --> <div> <div data-bind="dxPopup: { showTitle: true, resizeEnabled: true, width: 700, height: 500, maxHeight: \'95%\', maxWidth: \'95%\', minHeight: 300, minWidth: 400, title: title(), visible: popupVisible, wrapperAttr: { class: \'dx-filtereditor\' }, toolbarItems: buttonItems, showCloseButton: true, container: $root.getPopupContainer($element), position: { of: $root.getPopupContainer($element) }, }"> <div class="dx-filtereditor-tree dxd-border-secondary dxrd-cursor-default" data-bind="dxScrollView: { showScrollbar: \'onHover\', direction: \'both\' }" > <!-- ko foreach: masterQueries --> <div class="criteria-operator-group"> <div class="criteria-operator-group-item"> <div class="criteria-operator-text dxd-filter-editor-text-color criteria-operator-item-group dxd-filter-editor-group-back-color dxd-state-no-hover stylized" data-bind="text: queryName"></div> <div data-bind="service: { name: \'createRelation\' }"></div> </div> <div class="criteria-operator-content"> <!-- ko template: { name: \'dx-masterDetail-editor-relation\', foreach: relations }--> <!-- /ko --> </div> </div> <!-- /ko --> </div> </div> <div class="dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay" data-bind="dxPopupWithAutoHeight: { height: \'300px\', focusStateEnabled: false, wrapperAttr: { class: \'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay\' }, position: $root.rtl ? { my: \'right top\', at: \'right bottom\', of: popupService.target } : { my: \'left top\', at: \'left bottom\', of: popupService.target }, container: $root.getPopupContainer($element), target: popupService.target, showTitle: false, showCloseButton: false, animation: {}, hideOnOutsideClick: true, shading: false, minWidth:\'170px\', maxWidth:\'500px\', width: \'auto\', visible: popupService.visible }"> <!-- ko with: popupService--> <!-- ko with: data --> <!-- ko template: template--> <!-- /ko --> <!-- /ko --> <!-- /ko --> </div> </div> <!-- /ko -->',
    'dx-masterDetail-editor-keyColumn': '<div class="criteria-operator-text dxd-filter-editor-text-color criteria-operator-item-field dxd-filter-editor-field-back-color dxd-state-no-hover stylized" data-bind="attr: { title: $data.getTitle() }, text: queryName"></div> <div class="criteria-operator-text dxd-filter-editor-text-color">.</div> <div class="criteria-operator-text dxd-filter-editor-text-color"> <div data-bind="service: { name: \'setColumn\' }"></div> </div>',
    'dx-masterDetail-editor-relation': '<div class="criteria-operator-group"> <div class="criteria-operator-group-item"> <div class="criteria-operator-action" data-bind="dxclick: function() {  $parent.remove($data); }"> <div class="criteria-operator-action-image dx-image-filtereditor-remove"><!-- ko template: \'dxrd-svg-filtereditor-remove\' --><!-- /ko --></div> </div> <div class="criteria-operator-item" data-bind="visible: isEditable"> <div class="criteria-operator-item-editor" data-bind="dxTextBox: { value: relationName, onFocusOut: function() { isEditable(false); } }, focus: { on: isEditable }"></div> </div> <div class="criteria-operator-text dxd-filter-editor-text-color criteria-operator-item-value dxd-filter-editor-value-back-color" data-bind="visible: !isEditable()"> <div class="criteria-operator-text dxd-filter-editor-text-color clickable" data-bind="text: relationName, click: function() { isEditable(true); }"></div> </div> <div class="criteria-operator-action" data-bind="dxclick: create"> <div class="criteria-operator-action-image dx-image-filtereditor-add"><!-- ko template: \'dxrd-svg-filtereditor-add\' --><!-- /ko --></div> </div> </div> <div class="criteria-operator-content"> <!-- ko foreach: keyColumns --> <div class="criteria-operator-group"> <div class="criteria-operator-group-item"> <div class="criteria-operator-action" data-bind="dxclick: function() {  $parent.remove($data); }"> <div class="criteria-operator-action-image dx-image-filtereditor-remove"><!-- ko template: \'dxrd-svg-filtereditor-remove\' --><!-- /ko --></div> </div> <!-- ko template: { name: \'dx-masterDetail-editor-keyColumn\', data: master } --> <!-- /ko --> <div class="criteria-operator-text dxd-filter-editor-text-color"> = </div> <!-- ko template: { name: \'dx-masterDetail-editor-keyColumn\', data: detail } --> <!-- /ko --> </div> </div> <!-- /ko --> </div> </div>',
    'dx-masterdetail-editor-setColumn': '<div class="criteria-operator-text dxd-filter-editor-text-color clickable criteria-operator-item-field dxd-filter-editor-field-back-color" data-bind="text: target.column() || target.selectColumnText(), dxclick: showPopup, css: { \'dxd-state-selected\': target.isSelected, \'default\': !target.column() }"></div>',
    'dxqb-data-preview': '<div class="dxrd-height-100" data-bind="dxdTableView: $data.value"></div>',
    'dxqb-popup': '<div data-bind="dxPopup: { animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, wrapperAttr: { class: \'dxqb-preview\' }, visible: isVisible, title: title(), showTitle: true, resizeEnabled: true, shading: true, shadingColor: \'transparent\', fullScreen: false, width: 800, height: 544, container: container($element), position: { of: container($element) }, onHidden: function() { $data.data.value(null) }, focusStateEnabled: false }"> <div class="dxqb-preview-popup-content"> <!-- ko if: data && !isLoading() --> <!-- ko template: { name: template, data: data } --> <!-- /ko --> <!-- /ko --> <!-- ko if: isLoading --> <div class="dxrd-text-align-center dxrd-padding-top-25"> <div data-bind="dxLoadIndicator: { visible: true }"></div> </div> <!-- /ko --> </div> <div class="dxqb-preview-popup-buttons dxd-border-secondary"> <div class="dxqb-preview-popup-button" data-bind="dxButton: { text: okButtonText(), onClick: okButtonHandler, disabled: isLoading }"></div> </div> </div>',
    'dxqb-selectstatment-preview': '<div class="dxqb-show-query-string-content dx-widget"> <!-- ko if: !aceAvailable --> <div class="dxrd-show-query-string-editor" data-bind="dxTextArea: { value: value, valueChangeEvent: \'keyup\', readOnly: true }"></div> <!-- /ko --> <!-- ko if: aceAvailable --> <div class="dxrd-show-query-string-editor"> <div class="dxrd-show-query-string-editor-content"> <div class="dx-sql_editor dx-disabled-ace dxd-back-primary2" data-bind="dxAceEditor: { value: value, options: aceOptions, additionalOptions: additionalOptions }"></div> </div> </div> <!-- /ko --> </div>',
    'dxd-tableview': '<div class="dxd-tableview"> <div class="dxd-tableview-titles"> <table> <tbody> <tr> <!-- ko foreach: rtl ? data.schema.reverse() : data.schema --> <td data-bind="attr: { class: \'dxd-tableview-title-cell dxd-border-secondary dxd-tableview-resizable\' }"> <div class="dxd-tableview-cell-text dxd-qb-data-preview-cell-text-color dxd-text-info" data-bind="text: name"></div> </td> <!-- /ko --> </tr> </tbody> </table> </div> <!-- ko if: data.values --> <div class="dxd-tableview-data" data-bind="dxScrollView: { direction: \'both\', scrollByContent: false, showScrollbar: \'always\', scrollByThumb: true, bounceEnabled: false, useNative: false, onScroll: $data.onDataScroll, onInitialized: $data.onDataScrollInitialized }"> <table> <tbody> <!-- ko foreach: data.values --> <tr> <!-- ko foreach: $parent.rtl ? $data.reverse() : $data --> <td class="dxd-tableview-cell dxd-border-secondary"> <!-- ko ifnot: $parents[1].isImage($index()) --> <div data-bind="text: $data, attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }"></div> <!-- /ko --> <!-- ko if: $parents[1].isImage($index()) --> <!-- ko if: $parents[1].isImageTooLarge($data) --> <div data-bind="text: $parents[1].getImageTooLargeText($data), attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }"></div> <!-- /ko --> <!-- ko ifnot: $parents[1].isImageTooLarge($data) --> <div data-bind="attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }"> <img data-bind="attr: { src: \'data:image/bmp;base64,\' + $data }" /> </div> <!-- /ko --> <!-- /ko --> </td> <!-- /ko --> </tr> <!-- /ko --> <tr class="dxd-tableview-data-fakerow"> <!-- ko foreach: data.values[0] --> <td class="dxd-tableview-cell dxd-border-secondary"> <div data-bind="attr: { class: \'dxd-tableview-cell-text dxd-text-primary dxd-tableview-resizable\' + $index() }"></div> </td> <!-- /ko --> </tr> </tbody> </table> </div> <!-- /ko --> <!-- ko ifnot: data.values --> <div class="dxd-tableview-empty-message" data-bind="text: noDataText()"></div> <!-- /ko --> </div>',
    'dxqb-fieldlist-wrapper': '<div id="tree" data-bind="treelist: treeListOptions" class="dxrd-width-100 dxrd-height-100"></div>',
    'dxqb-properties-wrapper': '<div class="dxrd-properties-wrapper" data-bind="visible: active && visible"> <div class="dxd-text-primary dxrd-properties-wrapper-full-height"> <!-- ko with: model --> <div class="dxrd-properties-grid dxd-border-primary dxrd-top-0px" data-bind="dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true }" > <div> <!-- ko foreach: groups --> <div class="dx-fieldset" data-bind="visible: visible"> <div class="dx-accordion" data-bind="dxdAccordion: { collapsed: collapsed }"> <div class="dxrd-group-header dx-accordion-header dxd-border-primary" data-bind="css: { \'dxrd-group-header-collapsed\': collapsed() }"> <div class="dx-collapsing-image dxrd-display-inline-block" data-bind="template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !collapsed() }" ></div> <span class="dxrd-group-header-text" data-bind="text: displayName()"></span> </div> <div class="dx-accordion-content dxd-back-primary dxd-border-primary"> <!-- ko ifnot: editorsCreated --> <div class="dx-accordion-content-loading-panel"> <div data-bind="dxLoadIndicator: { visible: !editorsCreated() }"></div> </div> <!-- /ko --> <!-- ko if: $data.editorsRendered() --> <div data-bind="visible: editorsCreated"> <div class="dx-editors"> <!-- ko foreach: editors --> <!-- ko template: editorTemplate --> <!-- /ko --> <!-- ko if: ($index() === $parent.editors().length - 1 && $parent.editorsCreated(true)) --> <!-- /ko --> <!-- /ko --> </div> </div> <!-- /ko --> </div> </div> </div> <!-- /ko --> </div> </div> <!-- /ko --> </div> </div>',
    'dxqb-propertygrid': '<!-- ko with: value --> <div class="dx-fieldset dxqb-selected-properties"> <!-- ko foreach: properties.getEditors() --> <!-- ko template: editorTemplate --> <!-- /ko --> <!-- /ko --> </div> <!-- /ko -->',
    'dxqb-fieldspanel': '<!-- ko with: value --> <div data-bind="treeListSearchPanel: { controllers: $data.fieldListModel.treeListOptions().treeListController }"></div> <div class="dxqb-right-panel-fields" data-bind="dxScrollView: { showScrollbar: \'onHover\', scrollByContent: false, scrollByThumb: true, bounceEnabled: false, useNative: false }"> <!-- ko template: { name: "dxqb-fieldlist-wrapper", data: fieldListModel } --> <!-- /ko --> </div> <!-- /ko -->',
    'dxqb-parameterspanel': '<!-- ko with: value --> <div class="dxqb-right-panel-parameters" data-bind="dxScrollView: { showScrollbar: \'onHover\', scrollByContent: false, scrollByThumb: true, bounceEnabled: false, useNative: false }"> <div class="dxrd-width-100" data-bind="dxCollectionEditor: $data"></div> </div> <!-- /ko -->',
    'dxqb-collectioneditor-template': '<div class="dxrd-accordion-collection-item" data-bind="dxdAccordion: { collapsed: collapsed, setCollapsed: setCollapsed, getCollapsed: getCollapsed, setCollapsedChangedEvent: setCollapsedChangedEvent }"> <div class="dxrd-group-header dx-accordion-header"> <div> <div class="dx-collapsing-image dxrd-display-inline-block" data-bind="css: { \'dx-image-expanded\': !collapsed }, template: \'dxrd-svg-collapsed\'"></div> <div class="dx-group-header-font dxrd-display-inline-block" data-bind="text: $root.getLocalization(value.name), attr: { title: $root.getLocalization(value.name) }"></div> </div> </div> <div class="dx-accordion-content"> <div data-bind="template: { name: \'dx-propertieseditor\', data: getProperties({ level: 1 }) }"></div> </div> </div>',
    'dxqb-properties-wrapper-editorlist': '<div class="dxrd-properties-wrapper" data-bind="visible: active && visible"> <div  class="dxd-text-primary dxrd-height-100"> <!-- ko with: model --> <div class="dxrd-properties-grid dxd-border-primary dxrd-top-0px" data-bind="dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true }" > <div class="dx-fieldset dxqb-selected-properties"> <!-- ko foreach: getEditors() --> <!-- ko template: editorTemplate --> <!-- /ko --> <!-- /ko --> </div> </div> <!-- /ko --> </div> </div>',
    'dxqb-toolbar': '<div class="dxrd-toolbar-wrapper dxd-toolbar-back-color" data-bind="click: $root.findControl"> <div class="dxqb-toolbar-background dxd-back-primary-invariant"></div> <div class="dxrd-toolbar" data-bind="template: {name: \'dxrd-toolbar-tmplt\', data: actionLists.getViewModel().toolbarItems }"></div> </div>',
    'dxqb-property-editor': '<div class="dxrd-editor" data-bind="visible: visible"> <div class="dxrd-editor-header"> <div class="dx-field dxd-back-primary"> <div class="dx-field-label dxd-text-primary"> <div class="propertygrid-editor-displayName" data-bind="text: displayName, attr: { \'title\': displayName }"></div> </div> <div class="dx-field-value"> <div data-bind="service: { name: \'createEditorAddOn\' }"></div> <!-- ko lazy: { template: templateName } --> <!-- /ko --> </div> </div> </div> </div>',
    'dxqb-expressionstring': '<!-- ko if: $data.value --> <div data-bind="dxExpressionEditor: getOptions({ options: value, fieldListProvider: $root.parametersBindingsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() })"></div> <!-- /ko -->',
    'dxqb-filtereditor-changeparameter': '<div class="criteria-operator-item" data-bind="visible: target.isEditable"> <div class="criteria-operator-item" data-bind="dxTextBox: { value: target._parameterName, onFocusOut: function() { target.createParameter(); target.isEditable(false); } }"></div> </div> <div class="criteria-operator-text dxd-filter-editor-text-color clickable " data-bind="text: target.parameterName, dxclick: showPopup, css: { \'dxd-state-selected\': target.isSelected, \'default\': target.isDefaultTextDisplayed() }, visible: $data.target.isEditable() === false"></div>',
    'dxqb-filtereditor-propertiespopup': '<div class="dx-widget" data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <!-- ko foreach: data --> <div data-bind="dxdAccordion: { collapsed: collapsed }"> <div class="dx-accordion-header"> <div class="dx-filtereditor-popup-item dxd-list-item-back-color dx-item dx-list-item dxd-back-highlighted"> <div class="dx-collapsing-image dxrd-display-inline-block dxrd-margin-left-5px" data-bind="template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !collapsed() }" ></div> <span class="dx-item-content dx-list-item-content dxrd-padding-left-17px"  data-bind="text: name"></span> </div> </div> <div class="dx-accordion-content dxd-back-primary"> <!-- ko foreach: items --> <div class="dx-filtereditor-popup-item dx-item dx-list-item dxd-list-item-back-color dxd-back-highlighted"> <span class="dx-item-content dx-list-item-content dxrd-padding-left-17px"  data-bind="text: name, attr: { \'title\': name }, click: function() { $parents[1].click($data); }"></span> </div> <!--/ko --> </div> </div> <!--/ko --> </div>',
    'dxqb-filtereditor-parameterspopup': '<div class="dx-widget" data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <!-- ko if: $parent.viewModel.canCreateParameters --> <div class="dx-filtereditor-popup-item dx-item dx-list-item dxd-list-item-back-color dxd-back-highlighted"> <span class="dx-item-content dx-list-item-content" data-bind="text: $parent.viewModel.defaultDisplay(), click: function() { $parent.viewModel.isEditable(true); $parent.viewModel._parameterName(\'\'); $parent.visible(false); }"></span> </div> <!-- /ko --> <!-- ko foreach: data --> <div class="dx-filtereditor-popup-item dx-item dx-list-item dxd-list-item-back-color dxd-back-highlighted"> <span class="dx-item-content dx-list-item-content" data-bind="text: name, click: function() { $parent.click($data); } "></span> </div> <!-- /ko --> </div>',
    'dxqb-treelist-item-with-search': '<div data-bind="visible: visible"> <!-- ko template: "dxqb-treelist-header-item-with-search" --> <!-- /ko --> </div>',
    'dxqb-treelist-header-item-with-search': '<div class="dx-treelist-item dxd-list-item-back-color dxd-back-highlighted" data-bind="event: { dblclick: events.dblclick }, click: toggleSelected, draggable: isDraggable ? dragDropHandler : null, styleunit: padding, css: { \'dx-treelist-item-selected dxd-state-selected\': isSelected || isMultiSelected }"> <div class="dx-treelist-collapsedbutton"></div> <div class="dx-treelist-caption"> <div class="dx-treelist-selectedcontent"> <div class="dx-treelist-image" data-bind="css: imageClassName, template: { name: imageTemplateName, if: !!imageTemplateName }, attr: { title: text }"> </div> <!-- ko template: { name: \'dx-treelist-item-text-content\' } --> <!-- /ko --> </div> </div> </div>',
    'dxrd-page-choose-datasource-type': '<div class="dxrd-wizard-type-page" data-bind="css: $data._extendCssClass(\'type-page\')"> <!-- ko foreach: typeItems --> <!-- ko template: \'dxrd-page-choose-type-item\' --> <!-- /ko --> <!-- /ko --> </div>',
    'dxrd-page-choose-type-item': '<div data-bind="event: { click: $parent._itemClick, dblclick: function() { $parent._goToNextPage() } }, attr: { class: \'dxd-back-highlighted dxd-state-normal dxrd-wizard-type-item dx-fontsize-reestablished dxrd-wizard-type-item-border-color dxd-list-item-back-color \' + $parent._extendCssClass(\'type-item\')}, css: { \'dxd-border-secondary dxd-back-secondary\': $parent._IsSelected($data) } "> <div data-bind="attr: { class: \'dxrd-wizard-type-image \' + $parent._extendCssClass(\'type-image\') }, css: imageClassName, template: { name: imageTemplateName }"> </div> <!-- ko if: !!$parent._textToSearch --> <div class="dxrd-wizard-type-text" data-bind="searchHighlighting: { text: text, textToSearch: $parent._textToSearch }, attr: { title: text }, css: $parent._extendCssClass(\'type-text\')"></div> <!-- /ko --> <!-- ko ifnot: !!$parent._textToSearch --> <div class="dxrd-wizard-type-text" data-bind="text: text, attr: { title: text }, css: $parent._extendCssClass(\'type-text\')"></div> <!-- /ko --> </div>',
    'dxrd-wizard-add-federated-queries-page': '<div class="dxrd-wizard-page dxrd-wizard-add-federation-queries-page dxrd-wizard-page-treelist-accordion-style"> <div data-bind="treeListSearchPanel: {controllers: ko.unwrap(_fieldListModel).treeListController}"></div> <div class="dxrd-wizard-dataMember dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight }"> <div class="dxrd-wizard-dataMember-treelist dxrd-width-100 dxrd-height-100" data-bind="treelist: _fieldListModel" ></div> </div> <!-- ko template: { name: $data._queriesPopupHelper.template, data: _queriesPopupHelper } --> <!-- /ko --> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxLoadPanel: _loadPanelViewModel($element)"> </div> </div>',
    'dx-treelist-federation-action-with-popover': '<div class="dx-treelist-action-with-popover dxrd-display-inline-block" > <div class="dx-treelist-action" data-bind="dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: $data.imageTemplateName, iconClass: $data.imageClassName , disabled: $data.disabled && $data.disabled() }, attr: { title: text }"></div> <!-- ko template: { name: \'dx-popover-list-items\', data: ko.utils.extend($parent.data, { className: \'dxrd-federation-addqueries-popover\', position: $root.rtl ? \'right\' : \'left\', popupContainer: $root.getPopupContainer, width: 150 }) }--> <!-- /ko  --> </div>',
    'dxrd-querybuilder-select-popup-content': '<div class="dxrd-federation-result-grids dxrd-federation-data-container-column"> <div class="dxd-border-secondary dxrd-select-querybuilder-surface" data-bind="resizable: gridResizeHelper.resizable($root.resizeHandler, \'s\')"> <div class="dxrd-height-100" data-bind="dxQueryBuilderSurface: queryBuilderSurfaceCreator"></div> </div> <div class="dx-accordion dxd-border-secondary dxrd-select-result-alias-grid" data-bind="dxdAccordion: { collapsed: joinResultCollapsed }"> <div class="dxd-back-secondary dx-accordion-header dxd-text-primary"> <div class="dx-collapsing-image dxrd-display-inline-block" data-bind="template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !joinResultCollapsed() }" ></div> <span class="dxrd-group-header-text" data-bind="text: $root.getLocalization(\'Join result\', \'DataAccessUIStringId.FederationDataSourceQueryBuilder_SelectResult\')"></span> </div> <div class="dx-accordion-content dxd-border-secondary" data-bind="style: { height: resultGridHeight }"> <div class="dxrd-federation-action-add dxrd-collectioneditor-action" data-bind="dxButton: { icon: \'add\', disabled: addRowDisabled, onClick:addRow }"></div> <div data-bind="dxDataGrid: columnsGrid"></div> </div> </div> </div>',
    'dxrd-querybuilder-column-combobox-masked': '<div class="dxrd-querybuilder-column-container dxrd-querybuilder-column-container-masked"> <div class="dxrd-querybuilder-column-data"> <div class="dx-texteditor-input" data-bind="text: cellInfo.column"></div> </div> <div class="dxrd-querybuilder-column-editorswitch"> <!-- ko template: { name: \'dx-wizard-menu-box-editorswitch\', data: $data } --> <!-- /ko  --> </div> </div>',
    'dxrd-querybuilder-column-combobox': '<div class="dxrd-querybuilder-column-container dxrd-querybuilder-column-container-full"> <div class="dxrd-querybuilder-column-data"> <div class="dx-texteditor-input dx-padding-none" data-bind="dxDropDownBox: { dataSource: $data.allColumns, dropDownOptions: { container: $root.getPopupContainer($element), width:  $root.calculateWithZoomFactor(357)  }, popupPosition: { boundary: $root.getPopupContainer($element) }, value: cellInfo.key, valueExpr: \'key\', displayExpr: \'column\' }, visible: !$data.isExpression()"> <div data-bind="dxDataGrid: { dataSource: $data.allColumns, keyExpr: \'key\', remoteOperations: true, height: $root.calculateWithZoomFactor(250), columns: [{ dataField: \'column\', sortOrder: \'asc\' } , { dataField: \'table\'}], hoverStateEnabled: true, selection: { mode: \'single\' }, selectedRowKeys: [cellInfo.key], focusedRowEnabled: true, focusedRowKey: cellInfo.key, onSelectionChanged: $data.changeColumn } "></div> </div> <div class="dx-texteditor-input dx-padding-none" data-bind="dxExpressionEditor: { options: expression, fieldListProvider: itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() }, visible: $data.isExpression"></div> </div> <div class="dxrd-querybuilder-column-editorswitch"> <!-- ko template: { name: \'dx-wizard-menu-box-editorswitch\', data: $data } --> <!-- /ko  --> </div> </div>',
    'dxrd-querybuilder-transform-popup-content': '<div class="dxrd-federation-result-grids dxrd-federation-data-container-column"> <div class="dx-accordion dxd-border-secondary dxrd-transform-source-grid dxrd-result-source-grid"> <div class="dxd-back-secondary dx-accordion-header dxd-text-primary"> <span class="dxrd-group-header-text" data-bind="text: $data.transformGridTitle"></span> </div> <div class="dx-accordion-content dxd-border-secondary"> <div class="dxd-border-primary dxrd-transform-source-grid dxrd-result-source-grid" data-bind="dxDataGrid: transformGrid"></div> </div> </div> <div class="dx-accordion dxd-border-secondary dxrd-transform-result-grid"  data-bind="dxdAccordion: { collapsed: transformResultCollapsed }"> <div class="dxd-back-secondary dx-accordion-header dxd-text-primary"> <div class="dx-collapsing-image dxrd-display-inline-block" data-bind="template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !transformResultCollapsed() }" ></div> <span class="dxrd-group-header-text" data-bind="text: $data.transformResultGridTitle"></span> </div> <div class="dx-accordion-content dxd-border-secondary"> <div data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <div data-bind="treelist: resultFieldListModel" class="dxrd-width-100 dxrd-height-100"></div> </div> </div> </div> </div>',
    'dxrd-querybuilder-transform-checkbox': '<div class="dxrd-querybuilder-transform-checkbox" data-bind="dxCheckBox: transform"></div>',
    'dxrd-querybuilder-federation-popup': '<div data-bind="dxPopup: { showTitle: true, title: title(), wrapperAttr: { class: cssClass }, visible: popupVisible, toolbarItems: buttonItems, showCloseButton: true, fullScreen: false, maxHeight: maxHeight, height: height, width: width, onShowing: function(e) { $data.loaded(true); }, rtlEnabled: rtl, onHiding: onHiding, container: getPopupContainer($element), position: { of: getPopupContainer($element)}, resizeEnabled: true, minHeight: 450, minWidth: 650 }"> <div class="dxrd-querybuilder-federation-popup"> <!-- ko if: popupVisible --> <!-- ko if: loaded --> <div class="dxrd-federation-treelist-wrapper dxd-border-secondary" data-bind="resizable: resizeHelper.resizable($root.resizeHandler, \'e\')"> <div class="dxrd-federation-treelist" data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <div data-bind="treelist: fieldListModel" class="dxrd-width-100 dxrd-height-100"></div> </div> </div> <div class="dxrd-federation-data-container"> <!-- ko template: { name: popupContentTemplate } --> <!-- /ko --> </div> <!-- /ko --> <!-- /ko --> </div> </div>',
    'dxrd-querybuilder-union-popup-content': '<div class="dxrd-federation-result-grids dxrd-federation-data-container-column"> <div class="dxd-border-secondary dxrd-union-result-source-grid dxrd-result-source-grid" data-bind="dxDataGrid: sourcesGrid"></div> <div class="dxd-border-secondary dxrd-union-result-alias-grid"> <div data-bind="dxDataGrid: aliasGrid"></div> </div> </div>',
    'dx-wizard-fullscreen': '<!-- ko if: $data && $data.isVisible() --> <div class="dx-fullscreen-wizard dx-editors" data-bind="css: { \'dx-rtl\': $root.rtl, \'dx-ltr\': !$root.rtl }"> <div class="dxrd-wizard dxrd-report-wizard dx-editors dxd-text-primary dxd-back-primary2" data-bind="css: _extendCssClass"> <div class="dxrd-wizard-steps-container dxd-back-primary" data-bind="visible: $data.navigationPanel().isVisible"> <div class="dxrd-wizard-title dxd-border-primary" data-bind="text: _description(), attr: {\'title\': _description() }"></div> <!-- ko with: navigationPanel --> <div class="dxrd-wizard-steps" data-bind="foreach: _steps"> <div class="dxrd-wizard-steps-relative dxrd-position-relative"  data-bind="visible: $data.visible"> <div class="dxrd-wizard-steps-content" data-bind="click: $data.clickAction, text: $data.text, attr: {\'title\': $data.text }, css: { \'dxrd-disabled\': $data.disabled, \'dxd-back-secondary\': $data.isActive() }"></div> <div class="dxrd-wizard-steps-marker dxd-back-primary2" data-bind="visible: $data.isActive"></div> </div> </div> <!-- /ko --> </div> <div class="dxrd-wizard-content" data-bind="css: { \'withoutPanel\': !$data.navigationPanel().isVisible() }"> <div class="dxrd-wizard-part-description dxd-back-contrast"> <div class="dxrd-wizard-part-description-text dxd-text-primary dxd-border-primary" data-bind="text: _pageDescription()"></div> </div> <!-- ko with: _currentPage --> <div class="dxrd-wizard-work-content"> <div class="dxrd-wizard-work-content-relative"> <div data-bind="template: { name: template, data: page } "></div> </div> </div> <!-- /ko --> <!-- ko if: !(_currentPage() && _currentPage().page._customLoadPanelViewModel) --> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxLoadPanel: _loadPanelViewModel($element)"> </div> <!-- /ko --> <!-- ko if: _currentPage() && _currentPage().page._customLoadPanelViewModel --> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxAILoadingPanel: _currentPage().page._customLoadPanelViewModel($element, $data.isLoading)"> </div> <!-- /ko --> <div class="dxrd-wizard-navigation"> <div data-bind="dxButton: cancelButton" class="dxrd-wizard-btn left"></div> <div data-bind="dxButton: finishButton" class="dxrd-wizard-btn right"></div> <div data-bind="dxButton: nextButton" class="dxrd-wizard-btn right"></div> <div data-bind="dxButton: previousButton" class="dxrd-wizard-btn right"></div> </div> </div> </div> </div> <!-- /ko -->',
    'dx-wizard-fullscreen-page': '<!-- ko if: $data._sections.length > 0 --> <div class="dxrd-position-absolute" data-bind="foreach: _sections, css: _className, styleunit: { top: _parentMarginOffset, left: _parentMarginOffset, right: _parentMarginOffset, bottom: _parentMarginOffset }"> <div class="dx-border-inheritor dxd-border-accented"> <div class="dxrd-report-page-tile dxd-border-secondary" data-bind="css: { \'dxrd-disabled\': !$data.page() }, style: $parent._pageCss[$data.pageId]"> <div class="dxrd-report-page-tile-title" data-bind="visible: $parent._showPageDescription($data), text: $parent._getPageDescription($index(), $data), attr: { title: $parent._getPageDescription($index(), $data) }"></div> <!-- ko if: $data.page() !== null --> <!-- ko with: page --> <div class="dxrd-report-page-tile-content" data-bind="template: { name: $parent.metadata.template, data: page }, dxScrollView: { showScrollbar: \'onHover\'}"></div> <!-- /ko --> <!-- /ko --> <!-- ko if: $data.page() === null --> <div class="dxrd-report-page-tile-content dx-default-border-style dxd-border-secondary"> <div class="dxrd-wizard-page dxrd-wizard-disabled-content" data-bind="text: metadata.disabledText"></div> </div> <!-- /ko --> </div> </div> </div> <!-- /ko -->',
    'dxrd-jsondatasource-fields-page': '<div class="dxrd-wizard-page dx-jsonschema-page dx-frameless-style"> <div class="dx-default-border-style dxd-border-secondary dxrd-wizard-dataMember dx-fieldset dxrd-height-100" > <div class="dx-field"> <div class="dx-field-label" data-bind="text: _rootElementTitle"></div> <div class="dx-field-value" data-bind="dxSelectBox: { dataSource: _rootElementList, value: _selectedRootElement, displayExpr: \'fullPath\', displayCustomValue: true, useItemTextAsTitle: true, dropDownOptions: { container: $root.getPopupContainer($element) } }"></div> </div> <div class="dxrd-wizard-add-queries-page dxrd-wizard-page-treelist-accordion-style dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <div data-bind="treelist: _fieldListModel" class="dxrd-width-100 dxrd-height-100"></div> </div> </div> </div>',
    'dxrd-page-jsonsource': '<div class="dxrd-wizard-page dx-jsonsource-page dx-frameless-style"> <div class="dx-default-border-style dxd-border-secondary dxrd-wizard-dataMember dx-fieldset dxrd-height-100"  data-bind="dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true }, dxValidationGroup: $data._validationGroup || {}"> <div class="dx-field"> <div class="dx-property-required dx-field-label" data-bind="text: _jsonConnectionTitle, attr: { \'title\': _jsonConnectionTitle }"></div> <div class="dx-field-value" data-bind="dxTextBox: { value: _connectionName }, dxValidator: { validationRules: $data._connectionNameValidationRules || [] }"></div> </div> <div class="dx-field"> <div class="dx-field-label" data-bind="text: _jsonSourceTitle, attr: { \'title\': _jsonSourceTitle }"></div> <div class="dx-field-value" data-bind="dxLocalizedSelectBox: { dataSource: _sources, value: _selectedSource, valueExpr:\'value\', displayExpr: \'displayValue\', displayCustomValue: true, dropDownOptions: { container: $root.getPopupContainer($element) } }"></div> </div> <!-- ko with: _selectedSource --> <div data-bind="dxValidationGroup: $data.validationGroup || {}"> <div data-bind="css: $data.cssClass"> <!-- ko template: { name: \'dx-propertieseditor\', data: grid } --> <!-- /ko --> </div> <div class="dxrd-wizard-validationsummary" data-bind="dxValidationSummary: $data.validationSummary || {}, visible: $data.validationSummary && !isValid()"></div> </div> <!-- /ko --> <div class="dxrd-wizard-validationsummary" data-bind="dxValidationSummary: $data._validationSummary || {}, visible: $data._validationSummary && _selectedSource().validationSummary && !canNext()"></div> </div> </div>',
    'dx-property-json-string-editor': '<div class="dx-field" data-bind="visible: visible"> <!-- ko template: templateName --> <!-- /ko --> </div>',
    'dxrd-page-dataSource': '<div class="dxrd-wizard-page"> <!-- ko if: isDataSourceCreationAvailable --> <div class="dxrd-wizard-datasourceoperation dxrd-radio-nowrap-ellipsis" data-bind="dxRadioGroup: { value: selectedDataSourceOperation, items: dataSourceOperations, layout: \'vertical\' }"> <div data-options="dxTemplate : { name: \'item\' }"> <div class="dxrd-radio-nowrap-ellipsis-text" data-bind="text: text, attr: { \'title\': text }"></div> </div> </div> <!-- /ko --> <div class="dxrd-wizard-availabledatasources dx-default-border-style dxd-border-secondary"> <div class="dxrd-wizard-list" data-bind="dxList: { dataSource: availableDataSources, selectedItems: selectedDataSource, focusStateEnabled:false, editEnabled: true, height: dataSourcesListHeight, editConfig: { selectionEnabled: false }, selectionMode: \'single\', activeStateEnabled: false, disabled: createNewDataSource, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true }"> <div data-options="dxTemplate : { name: \'item\' }"> <div data-bind="text: name"></div> </div> </div> </div></div>',
    'dxrd-page-selectitems': '<div class="dxrd-wizard-page"> <!-- ko template: { name: \'dxrd-page-selectitems-radio-group\', data: $data } --> <!-- /ko --> <!-- ko template: { name: \'dxrd-page-selectitems-list\', data: $data } --> <!-- /ko --> </div>',
    'dxrd-page-selectitems-radio-group': '<!-- ko if: canCreateNew --> <div class="dxrd-wizard-datasourceoperation dxrd-radio-nowrap-ellipsis" data-bind="dxRadioGroup: { value: selectedOperation, items: operations, layout: \'vertical\' }"> <div data-options="dxTemplate : { name: \'item\' }"> <div class="dxrd-radio-nowrap-ellipsis-text" data-bind="text: text, attr: { \'title\': text }"></div> </div> </div> <!-- /ko -->',
    'dxrd-page-selectitems-list': '<div class="dxrd-wizard-availabledatasources dx-default-border-style dxd-border-secondary" data-bind="styleunit: { top: $data.canCreateNew() ? 65 : 0 }"> <div class="dxrd-wizard-list dxd-border-primary" data-bind="dxList: { dataSource: items, onSelectionChanged: function(e) { selectedItems(e.addedItems) }, selectedItems: selectedItems.peek(), hoverStateEnabled:false, editEnabled: true, height: \'100%\', editConfig: { selectionEnabled: false }, searchExpr: \'name\', searchEnabled: true, selectionMode: \'single\', activeStateEnabled: false, disabled: _createNew, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }"> <div data-options="dxTemplate : { name: \'item\' }" data-bind="event: { dblclick: function(e){ $parent.onDblClick() } }"> <div data-bind="text: $parent._displayExpr($data)"></div> </div> </div> </div>',
    'dxrd-page-specify-connection': '<div class="dxrd-wizard-page"> <!-- ko template: { name: \'dxrd-page-selectitems-radio-group\', data: $data } --> <!-- /ko --> <!-- ko if: !_createNew() --> <!-- ko template: { name: \'dxrd-page-selectitems-list\', data: $data } --> <!-- /ko --> <!-- /ko --> <!-- ko if: _createNew --> <div class="dxrd-page-specify-connection_createNew"> <!-- ko template: { name: \'dxrd-page-jsonsource\', data: _specifySourceData } --> <!-- /ko --> </div> <!-- /ko --></div>',
    'dx-jsonwizard-parametercollection': '<div class="dx-field"> <!-- ko with: value --> <div class="dx-jsonwizard-parameter-left-container"> <div class="dx-jsonwizard-parameter"> <div data-bind="dxLocalizedSelectBox: { dataSource: _parameterTypes, value: itemType, valueExpr:\'value\', displayExpr: \'displayValue\', displayCustomValue: true, dropDownOptions: { container: $root.getPopupContainer($element) } }"></div> </div> </div> <div class="dx-jsonwizard-parameter-middle-container"> <div data-bind="dxTextBox: { value: name, placeholder: $data.namePlaceholder() }, dxValidator: { validationRules: nameValidationRules || [] }"></div> </div> <div class="dx-jsonwizard-parameter-right-container"> <div class="dx-jsonwizard-parameter"> <div data-bind="dxTextBox: { value: _editingValue, placeholder: $data.valuePlaceholder() }, visible: !$data.isExpression()"></div> <div data-bind="dxExpressionEditor: { options: _expression, fieldListProvider: itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() }, visible: $data.isExpression"></div> </div> </div> <!-- ko template: { name: \'dx-wizard-menu-box-editorswitch\', data: $data } --> <!-- /ko  --> <!-- /ko --> </div>',
    'dx-jsonwizard-loadfile-editor': '<div data-bind="dxFileImagePicker: { value: value, placeholderId: \'File\', accept:\'.json,.txt\' }"></div>',
    'dx-jsonwizard-jsonstring-editor': '<!-- ko if: !aceAvailable --> <div class="dxrd-jsonwizard-jsonstring-editor dxd-border-secondary dxd-back-primary2" data-bind="dxTextArea: { value: value, spellcheck: false, isValid: isValid }, dxValidator: $data.validator || { validationRules: jsonStringValidationRules || [] }"></div> <!-- /ko --> <!-- ko if: aceAvailable --> <div class="dx-texteditor dx-editor-outlined dxrd-jsonwizard-jsonstring-editor dxd-wizard-jsoneditor dxd-border-secondary dxd-back-primary2" data-bind="dragDropJsonToDataSource, dxAceEditor: { value: value, editorContainer: editorContainer, options: aceOptions, additionalOptions: additionalOptions }, css: { \'dx-invalid\' : !value() || !isValid(), \'dxrd-drop-zone-border\': showDropBorders }"></div> <!-- /ko --> <div class="dxd-upload-file"> <div class="dxd-back-primary2"></div> <div data-bind="dxButtonWithTemplate: { onClick: uploadFile, hint: $data.getUploadTitle(), icon: \'dxrd-svg-wizard-Download\' }"></div> </div>',
    'dxrd-wizard-mongoDB-collections': '<div class="dxrd-wizard-page"> <div class="dxrd-wizard-mongoDB-collections dx-default-border-style dxd-border-secondary"> <div class="dxrd-mongoDB-action-add dxrd-collectioneditor-action" data-bind="dxButton: { icon: \'add\', onClick: _addCollection, disabled: _addQueryDisabled }"></div> <div class="dxrd-wizard-list dxd-border-primary" data-bind="dxDataGrid: _collectionsGrid"></div> <!-- ko if: _filterEditorVisible --> <div data-bind="dxFilterEditor: { options: _filterEditorOptions, template: \'dx-filtereditor-popup\', popupVisible: _filterEditorVisible, fieldListProvider: _filedListProvider, getDisplayNameByPath: $root.getDisplayNameByPath, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider(), aiExpressionGenerator: $root.aiExpressionGenerator }"></div> <!-- /ko --> </div> </div>',
    'dxrd-wizard-mongoDB-filterstring': '<div data-bind="dxEllipsisEditor: { value: value, buttonAction: buttonAction, disabled: disabled }"></div>',
    'dxrd-mongoDBdatasource-fields-page': '<div class="dxrd-wizard-page dx-mongodbschema-page dx-frameless-style"> <div class="dx-default-border-style dxd-border-secondary dxrd-wizard-dataMember dx-fieldset dxrd-height-100" > <div class="dx-field"> <div class="dx-field-label" data-bind="text: _querySelectBoxTitle"></div> <div class="dx-field-value" data-bind="dxSelectBox: { dataSource: _availableQueries, value: _selectedQueryName, disabled: _isLoading, useItemTextAsTitle: true, dropDownOptions: { container: $root.getPopupContainer($element) } }"></div> </div> <div class="dxrd-wizard-add-queries-page dxrd-wizard-page-treelist-accordion-style dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\' }"> <!-- ko if: _isLoading --> <div class="dxrd-text-align-center dxrd-padding-top-25"> <div data-bind="dxLoadIndicator: { visible: true }"></div> </div> <!-- /ko --> <!-- ko ifnot: _isLoading --> <div data-bind="treelist: _fieldListModel" class="dxrd-width-100 dxrd-height-100"></div> <!-- /ko --> </div> </div> </div>',
    'dxrd-wizard-add-queries-page': '<div class="dxrd-wizard-page dxrd-wizard-add-queries-page dxrd-wizard-page-treelist-accordion-style"> <div data-bind="treeListSearchPanel: {controllers: ko.unwrap(_fieldListModel).treeListController}"></div> <div class="dxrd-wizard-dataMember dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight }"> <div data-bind="treelist: _fieldListModel" class="dxrd-width-100 dxrd-height-100"></div> </div> <!-- ko ifnot: $data.disableCustomSql --> <!-- ko template: { name: \'dxqb-popup-selectStatment\', data: _popupSelectStatement } --> <!-- /ko --> <!-- /ko --> <!-- ko template: { name: \'dxrd-querybuilder-popup\', data: _popupQueryBuilder } --> <!-- /ko --> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxLoadPanel: _loadPanelViewModel($element)"> </div> </div>',
    'dxd-custom-query-treelisitem': '<div data-bind="visible: visible"> <div class="dx-background-inheritor dxd-back-highlighted dxd-state-selected"> <div class="dx-treelist-item dx-fontsize-reestablished dxd-list-item-back-color" data-bind=" styleunit: padding, click: toggleSelected, css: { \'dx-treelist-item-selected dxd-state-selected dxd-back-secondary\': isSelected }"> <div class="dx-treelist-collapsedbutton" data-bind="css: nodeImageClass"></div> <div class="dx-treelist-caption"> <!-- ko if: actions && actions.length > 0 --> <div class="dx-treelist-action-container" data-bind="visible: isSelected || isHovered, click: function(s, e) { e.stopPropagation(); }"> <!-- ko foreach: actions --> <!-- ko if: templateName --> <!-- ko template: templateName  --> <!-- /ko --> <!-- /ko --> <!-- ko if: !templateName --> <div class="dx-treelist-action" data-bind="dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: imageTemplateName, iconClass: imageClassName, disabled: ko.unwrap(disabled) }, attr: { title: text }"></div> <!-- /ko --> <!-- /ko --> </div> <!-- /ko  --> <div class="dx-treelist-selectedcontent"> <div class="dx-treelist-text-wrapper"> <div class="dx-add-queries-page-checkbox" data-bind="dxCheckBox: { value: data.checked }, click: function(treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }"></div> <div class="dx-treelist-text dx-treelist-text-with-checkbox"> <div class="dx-queryname-textbox" data-bind="dxTextBox: { value: queryName, onValueChanged: queryNameHasChanged }"></div> </div> </div> </div> </div> </div> </div> </div>',
    'dxrd-configure-query-parameters-page': '<div class="dxrd-wizard-page dxrd-configure-query-parameters-page dxrd-wizard-page-treelist-accordion-style"> <div class="dxrd-wizard-dataMember dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight }"> <!-- ko if: !!$data._fieldListModel() --> <div data-bind="treelist: _fieldListModel" class="dxrd-width-100 dxrd-height-100"></div> <!-- /ko --> </div> </div>',
    'dxrd-wizard-configure-relations-page': '<div class="dxrd-wizard-page dxrd-wizard-configure-relations-page"> <!-- ko if: $data._relationsEditor() --> <!-- ko template: { name: \'dxrd-masterDetail-editor-complete-wizard\', data: $data._relationsEditor }--> <!-- /ko --> <!-- /ko --> </div>',
    'dxrd-masterDetail-editor-complete-wizard': '<div class="dx-filtereditor dxrd-masterDetail-editor-complete-wizard dxd-border-secondary"> <!-- ko if: $data --> <div class="dx-filtereditor-tree dxd-border-secondary dxrd-cursor-default" data-bind="dxScrollView: { showScrollbar: \'onHover\', direction: \'both\' }" > <!-- ko foreach: masterQueries --> <div class="criteria-operator-group"> <div class="criteria-operator-group-item"> <div class="criteria-operator-text dxd-filter-editor-text-color criteria-operator-item-group dxd-filter-editor-group-back-color stylized" data-bind="text: queryName"></div> <div data-bind="service: { name: \'createRelation\' }"></div> </div> <div class="criteria-operator-content"> <!-- ko template: { name: \'dx-masterDetail-editor-relation\', foreach: relations }--> <!-- /ko --> </div> </div> <!-- /ko --> </div> <div class="dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay" data-bind="dxPopupWithAutoHeight: { height: \'235px\', focusStateEnabled: false, position: $root.rtl ? { my: \'right top\', at: \'right bottom\', of: popupService.target, collision: \'flipfit\' } : { my: \'left top\', at: \'left bottom\', of: popupService.target, collision: \'flipfit\' }, wrapperAttr: { class: \'dx-selectbox-popup-wrapper dx-dropdownlist-popup-wrapper dx-filtereditor-criteriaoperator-popup dx-dropdowneditor-overlay\' }, container: $root.getPopupContainer($element), target: popupService.target, showTitle: false, showCloseButton: false, animation: {}, hideOnOutsideClick: true, shading: false, minWidth:\'170px\', maxWidth:\'500px\', width: \'auto\', visible: popupService.visible }"> <!-- ko with: popupService--> <!-- ko with: data --> <!-- ko template: template--> <!-- /ko --> <!-- /ko --> <!-- /ko --> </div> <!-- /ko --> </div>',
    'dxqb-popup-selectStatment': '<div data-bind="dxPopup: { animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, wrapperAttr: { class: \'dxqb-preview\' }, visible: isVisible, title: title(), showTitle: true, resizeEnabled: true, shading: true, shadingColor: \'transparent\', fullScreen: false, width: 800, height: 544, container: closest($element, \'.dxrd-wizard\'), position: { of: closest($element, \'.dx-designer-viewport\') }, onHidden: function() { $data.data(null) }, focusStateEnabled: false }"> <div class="dxqb-preview-popup-content"> <div class="dxqb-show-query-string-content dx-widget"> <!-- ko if: !aceAvailable --> <div class="dxrd-show-query-string-editor" data-bind="dxTextArea: { value: data, valueChangeEvent: \'keyup\', disabled: false }"></div> <!-- /ko --> <!-- ko if: aceAvailable --> <div class="dxrd-show-query-string-editor"> <div class="dxrd-show-query-string-editor-content"> <div class="dx-sql_editor dxd-back-primary2" data-bind="dxAceEditor: { value: data, additionalOptions: additionalOptions, options: aceOptions }"></div> </div> </div> <!-- /ko --> </div> </div> <div class="dxqb-preview-popup-buttons dxd-border-secondary"> <div data-bind="dxButton: { text: okButtonText(), onClick: okButtonHandler }" class="dxqb-preview-popup-button"></div> </div> </div>',
    'dxrd-treelist-with-checkbox': '<!-- ko if: visible --> <!-- ko if: hasContent --> <!-- ko template: "dx-treelist-accordion-item-with-checkbox" --> <!-- /ko --> <!-- /ko --> <!-- ko ifnot: hasContent --> <!-- ko template: "dx-treelist-header-item-with-checkbox" --> <!-- /ko --> <!-- /ko --> <!-- /ko -->',
    'dx-treelist-accordion-item-with-checkbox': '<div data-bind="dxdAccordionExt: { collapsed: collapsed, lazyContentRendering: true, setCollapsedChangedEvent: setCollapsedChangedEvent }"> <!-- ko template: "dx-treelist-header-item-with-checkbox" --> <!-- /ko --> <div class="dx-fieldset dx-accordion-content dxd-back-primary"> <!-- ko with: data --> <!-- ko template: { name: contenttemplate } --> <!-- /ko --> <!-- /ko --> </div> </div>',
    'dx-treelist-header-item-with-checkbox': '<div class="dx-background-inheritor dxd-back-highlighted dxd-state-selected"> <div data-bind="event: events, click: toggleSelected, draggable: isDraggable ? dragDropHandler : null, attr: { class: \'dx-treeview-item dx-treelist-item dx-fontsize-reestablished dxd-list-item-back-color \' + (hasItems ? \'dx-treelist-list-item-content \' : \'dx-treelist-field-item-content \') + (isSelected ? \'dx-treelist-item-selected dxd-state-selected dxd-back-secondary\' : \'\') }, styleunit: padding, css: { \'dx-state-hover\': isHovered }"> <!-- ko ifnot: hasItems--> <!-- ko if: showIcon --> <div class="dx-treelist-image" data-bind="css: imageClassName, template: { name: imageTemplateName, if: !!imageTemplateName  }, attr: { title: text }"> </div> <!-- /ko --> <!-- ko ifnot: showIcon --> <div class="dx-treelist-collapsedbutton" data-bind="css: nodeImageClass"></div> <!-- /ko --> <!-- /ko --> <!-- ko if: hasItems --> <div class="dx-treelist-collapsedbutton" data-bind="css: nodeImageClass, visible: hasItems, template: \'dxrd-svg-collapsed\', click: toggleCollapsed"></div> <!-- /ko --> <div class="dx-treelist-caption"> <!-- ko if: actions && actions.length > 0 --> <div class="dx-treelist-action-container" data-bind="visible: isSelected || isHovered, click: function(s, e) { e.stopPropagation(); }"> <!-- ko foreach: actions --> <!-- ko if: templateName --> <!-- ko template: templateName  --> <!-- /ko --> <!-- /ko --> <!-- ko if: !templateName --> <div class="dx-treelist-action" data-bind="dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: imageTemplateName, iconClass: imageClassName, disabled: ko.unwrap(disabled) }, attr: { title: text }"></div> <!-- /ko --> <!-- /ko --> </div> <!-- /ko  --> <div class="dx-treelist-selectedcontent" data-bind="css: { \'dxrd-disabled\': data.disabled }"> <div class="dx-treelist-text-wrapper"> <div class="dx-add-queries-page-checkbox" data-bind="dxCheckBox: { value: data.checked, disabled: data.selectionDisabled }, click: function(treeNode, e) { treeNode.data.toggleChecked(); e.stopPropagation(); return true; }"></div> <!--ko if: searchModel.searchEnabled --> <div class="dx-treelist-text dx-highlighted-search-text dx-treelist-text-with-checkbox" data-bind="searchHighlighting: { text: text, textToSearch: searchModel.textToSearch, searchOptions: searchModel.searchOptions }, attr: { title: text }"></div> <!-- /ko  --> <!--ko ifnot: searchModel.searchEnabled --> <div class="dx-treelist-text dx-treelist-text-with-checkbox" data-bind="text: text, attr: { title: text }"></div> <!-- /ko  --> </div> </div> </div> </div></div>',
    'dx-treelist-action-with-popover': '<div class="dx-treelist-action-with-popover dxrd-display-inline-block" > <div class="dx-treelist-action" data-bind="dxButtonWithTemplate: { onClick: function() { clickAction($parent); }, icon: imageTemplateName, iconClass: imageClassName , disabled: ko.unwrap(disabled) }, attr: { title: text }"></div> <!-- ko template: { name: \'dx-popover-list-items\', data: ko.utils.extend($parent.data, { position: $root.rtl ? \'left\' : \'right\', popupContainer: $root.getPopupContainer }) }--> <!-- /ko  --> </div>',
    'dx-treelist-accordion-contenttemplate-custom-with-actions': '<div data-bind="dxPropertyGrid: { target: $parent.data.dataSourceParameter, level: $parent.data.editor.level + 1 }"></div>',
    'dxrd-page-objectdatasource-datamembers': '<div class="dx-objectdatasource-datamembers-page"> <div data-bind="dxList: { dataSource: _chooseObjectDataMember.dataMembers, selectedItems: _chooseObjectDataMember.selectedDataMembers, selectionMode: \'single\', activeStateEnabled: false, focusStateEnabled: false, searchExpr: \'displayName\', searchEnabled: true, noDataText: $dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }"> <div data-options="dxTemplate : { name: \'item\' }"> <div data-bind="text: $data.displayName"></div> </div> </div> </div>',
    'dxrd-page-objectdatasource-types': '<div class="dx-objectdatasource-types-page"> <div class="dxd-border-primary" data-bind="treeListSearchPanel: {controllers: _chooseObjectType.availableTypesTreelistModel.treeListController}"></div> <div data-bind="dxScrollView: { showScrollbar: \'onHover\', height: _chooseObjectType._scrollViewHeight}"> <div data-bind="treelist: _chooseObjectType.availableTypesTreelistModel"></div> <!-- ko if: _chooseObjectType.types && !_chooseObjectType.types().length --> <div class="dxa-no-data-message" data-bind="text: $root.dx._static.noDataText()"></div> <!-- /ko--> </div> </div>',
    'dxrd-page-objectsource': '<div class="dxrd-wizard-page dx-objectsource-page dx-frameless-style dxrd-position-absolute dxrd-top-45px dxrd-left-45px dxrd-right-45px dxrd-bottom-45px dxrd-margin-0" > <!-- ko foreach: _sections--> <div class="dx-border-inheritor dxd-border-accented"> <div class="dxrd-report-page-tile dxd-border-secondary" data-bind="style: position, css: { \'dxrd-disabled\': $data.disabled() }"> <div class="dxrd-report-page-tile-title" data-bind="text: $parent.showDescription($index(), description), attr: { title: $parent.showDescription($index(), description) }"></div> <div class="dxrd-report-page-tile-content dx-default-border-style dxd-border-secondary"> <!-- ko if: !$data.disabled() --> <!-- ko template: { name: template, data: data } --> <!-- /ko --> <!-- /ko --> <!-- ko if: $data.disabled() --> <div class="dxrd-wizard-page dxrd-wizard-disabled-content" data-bind="text: disabledText"></div> <!-- /ko --> </div> </div> </div> <!-- /ko --> </div>',
    'dx-objectdatasource-types-section': '<div class="dx-objectdatasource-types-section"> <div data-bind="treeListSearchPanel: {controllers: availableTypesTreelistModel.treeListController}"></div> <div data-bind="dxScrollView: { showScrollbar: \'onHover\', height: _scrollViewHeight}"> <div data-bind="treelist: availableTypesTreelistModel"></div> </div> </div>',
    'dx-objectdatasource-datamembers-section': '<div class="dx-objectdatasource-parameters-datamembers-section"> <div class="dxd-border-primary" data-bind="dxList: { dataSource: dataMembers, selectedItems: selectedDataMembers, selectionMode: \'single\', activeStateEnabled: false, focusStateEnabled: false, searchExpr: \'displayName\', searchEnabled: true, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }"> <div data-options="dxTemplate : { name: \'item\' }"> <div data-bind="text: $data.displayName"></div> </div> </div> </div>',
    'dx-objectdatasource-parameters-grid': '<!-- ko if: !!$data --> <div class="dx-field dxd-back-primary"> <div class="dx-field-label" data-bind="text: $data.displayName, attr: { \'title\': $data.displayName}"></div> </div> <div class="dxrd-properties-wrapper"> <!-- ko foreach: _grids --> <!-- ko template: { name: \'dx-propertieseditor\', data: $data }--> <!-- /ko --> <!-- /ko --> </div> <!-- /ko -->',
    'dx-objectdatasource-expression-popup': '<!-- ko if: $data.value --> <div data-bind="dxExpressionEditor: getOptions({ options: value, fieldListProvider: value().itemsProvider, displayNameProvider: $root.displayNameProvider && $root.displayNameProvider() })"></div> <!-- /ko -->',
    'dx-objectdatasource-parameters-section': '<div class="dx-objectdatasource-parameters-section"> <div class="dx-fieldset" data-bind="dxScrollView: { showScrollbar: \'onHover\'}"> <!-- ko template: { name: \'dx-objectdatasource-parameters-grid\', data: _ctorParametersObject }--> <!-- /ko --> <!-- ko template: { name: \'dx-objectdatasource-parameters-grid\', data: _dataMemberParametersObject }--> <!-- /ko --> </div> </div>',
    'dx-objectdatasource-configureparameters-page': '<div class="dxrd-report-page-tile-content dx-default-border-style dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\'}"> <!-- ko template: { name: \'dx-objectdatasource-parameters-section\', data: _chooseObjectParameters } --> <!-- /ko  --> </div>',
    'dxrd-page-configure-parameters': '<div class="dxrd-wizard-page"> <div class="dxrd-datasource-parameters" data-bind="dxCollectionEditor: parametersEditorOptions"></div> </div>',
    'dxrd-parameter-collection-item': '<div class="dx-accordion" data-bind="dxdAccordion: $data"> <div class="dxrd-group-header dx-accordion-header dxd-text-primary dxd-border-primary dxrd-border-bottom-0"  data-bind="styleunit: { \'marginLeft\' : padding }, css: { \'dxrd-group-header-collapsed\': collapsed }"> <div class="dx-collapsing-image dxrd-display-inline-block" data-bind="template: \'dxrd-svg-collapsed\', css: { \'dx-image-expanded\': !collapsed }" ></div> <span class="dxrd-group-header-text dxd-text-primary" data-bind="text: value.name"></span> </div> <div class="dx-accordion-content dxd-border-primary"> <div data-bind="template: { name: \'dx-propertieseditor\', data: getProperties() }"></div> </div> </div>',
    'dxrd-wizard-datasource-parameters': '<div class="dx-fieldset dxrd-height-100" > <div class="dx-collectioneditor dxrd-height-100" > <div class="dxrd-datasource-parameters-collection"> <div class="dxrd-datasource-parameters-container dxd-border-secondary" data-bind="dxScrollView: { showScrollbar: \'onHover\', useNative: false, scrollByThumb: true  }"> <!-- ko if: values.length === 0 --> <div class="dx-collectioneditor-empty dxd-empty-area-placeholder-text-color dxd-text-info"> <span class="dxrd-datasource-parameters-empty-text" data-bind="text: emptyAreaText"></span> </div> <!-- /ko --> <!-- ko if: values.length !== 0 --> <div class="dx-collectioneditor-items" data-bind="foreach: values"> <div class="dx-background-inheritor dxd-back-highlighted dxd-state-selected"> <div class="dx-collectioneditor-item-container dx-fontsize-reestablished dxd-list-item-back-color" data-bind="dxclick: select, css: { \'dxd-state-selected dxd-back-secondary\' : selected }"> <div class="dx-collection-item"></div> </div> </div> </div> <!-- /ko --> </div> </div> <div class="dxrd-collectioneditor-wizard-buttons" data-bind="visible: showButtons"> <div class="dxrd-collectioneditor-action" data-bind="dxButton: { onClick: buttons.add.action, text: buttons.add.text }, attr: { title: buttons.add.text }"></div> <div class="dxrd-collectioneditor-action" data-bind="dxButton: { onClick: buttons.delete.action, disabled: selectedIndex === null, text: $parent.removeButtonTitle }, attr: { title: $parent.removeButtonTitle }"></div> </div> </div> </div>',
    'dxrd-page-connectionstring': '<div class="dxrd-wizard-page"> <div class="dx-wizard-connections dx-default-border-style dxd-border-secondary"> <div class="dxrd-wizard-list dxd-border-primary" data-bind="dxList: { dataSource: _connectionStrings, onSelectionChanged: function(e) { _selectedConnectionString(e.addedItems) },  selectedItems: _selectedConnectionString.peek(), focusStateEnabled: false, searchEnabled: true, searchExpr: \'description\', editEnabled: true, editConfig: { selectionEnabled: true }, selectionMode: \'single\', activeStateEnabled: false, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true, searchEditorOptions: { placeholder: $root.dx._static.searchPlaceholder() } }"> <div data-options="dxTemplate : { name: \'item\' }"> <div data-bind="text: $data[\'description\'] || $data[\'name\']"></div> </div> </div> </div> </div>',
    'dxrd-select-control': '<div class="dxrd-create-query-page-query-header"> <div class="dx-editquery-editor" data-bind="text: caption()"></div> <div class="dxd-hyperlink-color dxd-text-accented" data-bind="visible: !runQueryBuilderDisabled, text: $parent.runQueryBuilderBtnText, click: function() { $parent.runQueryBuilder() }"></div> </div> <!-- ko if: !aceAvailable --> <div class="dxrd-wizard-list dxrd-create-query-page-editor dx-default-border-style dxd-border-secondary dx-editquery-editor" data-bind="dxTextArea: { value: sqlString, valueChangeEvent: \'keyup input blur\', readOnly: disableCustomSql() }"></div> <!-- /ko --> <!-- ko if: aceAvailable --> <div class="dxrd-create-query-page-editor dxrd-create-query-page-editor-border dxd-border-secondary dx-editquery-editor"> <div class="dxrd-create-query-page-editor-content"> <div class="dx-sql_editor dxd-back-primary2" data-bind="dxAceEditor: { value: sqlString, additionalOptions: additionalOptions, options: aceOptions }, css: { \'dx-disabled-ace\': disableCustomSql() }"></div> </div> </div> <!-- /ko -->',
    'dxrd-procedures-control': '<div class="dx-editquery-editor dxrd-margin-top-20px" data-bind="text: caption()"></div> <div class="dx-editquery-editor dx-default-border-style dxd-border-secondary"> <div class="dxrd-wizard-list dxrd-create-query-page-editor" data-bind="dxList: { dataSource: storedProcedures, searchEnabled: true, searchExpr: \'name\', onContentReady: scrollActiveItem, selectedItems: selectedProcedure, editEnabled: true, editConfig: { selectionEnabled: true }, selectionMode: \'single\', activeStateEnabled: false, noDataText: $root.dx._static.noDataText(), encodeNoDataText: true }"> <div data-options="dxTemplate : { name: \'item\' }"> <div data-bind="text: $parent.generateStoredProcedureDisplayName($data)"></div> </div> </div> </div>',
    'dxrd-wizard-create-query-page': '<div class="dxrd-wizard-page dxrd-wizard-create-query-page"> <div class="dxrd-create-query-page-content"> <div data-bind="text: queryNameCaption()"></div> <div class="dx-editquery-editor" data-bind="dxTextBox: { value: queryName, placeholder: $data.placeholder() }"></div> <div class="dxrd-radio-nowrap-ellipsis" data-bind="dxRadioGroup: { value: selectedQueryType, layout: \'horizontal\', items: queryTypeItems }"> <div data-options="dxTemplate : { name: \'item\' }"> <div class="dxrd-radio-nowrap-ellipsis-text" data-bind="text: $parent.localizeQueryType($data), attr: { \'title\': $parent.localizeQueryType($data) }"></div> </div> </div> <!-- ko template: { name: queryControl().template, data: queryControl() } --> <!-- /ko --> </div> <!-- ko template: { name: \'dxrd-querybuilder-popup\', data: popupQueryBuilder } --> <!-- /ko --> </div>',
    'dxrd-querybuilder-popup': '<div data-bind="dxPopup: popupViewModel($element)"> <!-- ko if: qbOptions --> <!-- ko if: isVisible --> <div class="dxrd-querybuilder-popup-content"> <div class="dxrd-height-100" data-bind="dxQueryBuilder: qbOptions"></div> </div> <!-- /ko --> <div class="dxrd-querybuilder-popup-buttons dxd-border-secondary"> <div data-bind="dxButton: { text: getDisplayText(\'previewResults\'), onClick: previewHandler, disabled: okButtonDisabled }" class="dxrd-querybuilder-popup-button-left"></div> <div data-bind="dxButton: { text: getDisplayText(\'cancel\'), onClick: cancelHandler }" class="dxrd-querybuilder-popup-button"></div> <div data-bind="dxButton: { text: getDisplayText(\'ok\'), type: \'default\', onClick: okHandler, disabled: okButtonDisabled }" class="dxrd-querybuilder-popup-button"></div> </div> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxLoadPanel:{ animation: { show: { type: \'fade\', from: 0, to: 1, duration: 700 }, hide: { type: \'fade\', from: 1, to: 0, duration: 700 } }, deferRendering: false, message: getDisplayText(\'loading\'), visible: showLoadIndicator, shading: true, shadingColor: \'transparent\'}"> </div> <!-- /ko --> </div>',
    'dxrd-querybuilder': '<div class="dx-designer dx-querybuilder" data-bind="template: \'dxrd-designer\'"> </div>',
    'dx-wizard-menu-box-editorswitch': '<div class="dxrd-editormenu-box dxd-property-grid-menu-box-color dxd-back-highlighted" data-bind="css: { \'dxd-state-active\': $data.isExpression() }, dxButtonWithTemplate: { onClick: switchEditors, icon: \'dxrd-svg-wizard-expressions\', disabled: disabled }"> </div>',
    'dx-popover-list-items': '<div data-bind="dxPopover: { wrapperAttr: { class: $data.className }, width: $data.width || 200, position: position, visible: popoverVisible, target: target, container: popupContainer($element)}"> <div data-bind="attr: { class: $data.className + \'-list\'}, dxList: { dataSource: popoverListItems(), onItemClick: itemClickAction }"> <div data-bind="attr: { class: $data.className + \'-list-item\'}"  data-options="dxTemplate : { name: \'item\' }"> <div class="dx-text-content" data-bind="text: name, attr: { title: name }"></div> </div> </div> </div>',
    'dx-wizard-headerNew': '<div class="dxrd-wizard-header-custom"> <!-- ko with: _currentPage --> <div class="dxrd-span-title" data-bind="text: $data.title || $parent.title"></div> <div class="dxrd-span-description" data-bind="text: description, attr: { title: description }"></div> <!-- /ko --> </div>',
    'dx-wizard-newlayout': '<div class="dxrd-wizard dx-editors dxd-text-primary" data-bind="dxPopup: { animation: { show: { type: \'fadeIn\', duration: 700 }, hide: { type: \'fadeOut\', duration: 700 } }, visible: isVisible, wrapperAttr: { class: \'dxrd-wizard dx-editors dxd-text-primary\' }, title: isVisible() ? title : \'\', showTitle: true, fullScreen: false, width: width, height: height, container: $element, visualContainer: _container($element), titleTemplate: _titleTemplate, position: _wizardPopupPosition($element) }, cssArray: [ $data._extendCssClass, { \'dx-rtl\': $root.rtl, \'dx-ltr\': !$root.rtl }]"> <div class="dxrd-wizard-content"> <!-- ko with: _currentPage --> <div class="dxrd-height-100" data-bind="template: { name: template, data: page } "></div> <!-- /ko --> <div class="dxrd-wizard-load-panel dxd-text-primary" data-bind="dxLoadPanel: _loadPanelViewModel($element)"></div> </div> <div class="dxrd-wizard-navigation"> <div data-bind="dxButton: cancelButton" class="dxrd-wizard-btn left"></div> <div data-bind="dxButton: finishButton" class="dxrd-wizard-btn right"></div> <div data-bind="dxButton: nextButton" class="dxrd-wizard-btn right"></div> <div data-bind="dxButton: previousButton" class="dxrd-wizard-btn right"></div> </div> </div>'
});

;// ./dist/query-builder/dragDrop/_columnDragHandler.js










class ColumnDragHandler extends external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler {
    _needToCreateRelation() {
        if (!(this.selection.dropTarget && this.selection.dropTarget instanceof ColumnSurface))
            return false;
        const table = this.selection.dropTarget.getControlModel().parentModel();
        return table !== this.getDragColumn().parentModel();
    }
    constructor(querySurface, selection, undoEngine, snapHelper, dragHelperContent) {
        super(querySurface, selection, undoEngine, snapHelper, dragHelperContent);
        this.querySurface = querySurface;
        this.undoEngine = undoEngine;
        this._dragColumn = external_ko_namespaceObject.observable(null);
        this._dragConditionSurface = null;
        this.dragDropConnector = external_ko_namespaceObject.observable(null);
        this.cursor = 'arrow';
        this.containment = '.dxrd-ghost-container';
        this['helper'] = undefined;
    }
    startDrag(control) {
        this._scrollProcessor = new external_DevExpress_Analytics_Internal_namespaceObject.dxScrollProcessor(document.getElementsByClassName('dxqb-mainwin')[0]);
        if (control instanceof ConnectionPointSurface) {
            const condition = control.getControlModel().parentModel();
            this._dragConditionSurface = (0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(condition);
            if (!this._dragConditionSurface)
                return;
            this._dragConditionSurface.isVisible(false);
            this._dragColumn((condition.startPoint() === control.getControlModel()) ? condition.nestedColumn() : condition.parentColumn());
        }
        else if (!(control instanceof ColumnSurface)) {
            throw new Error('ColumnDragHandler can be applied to the Column only.');
        }
        else {
            this._dragColumn(control.getControlModel());
        }
        const connectorModel = new RoutedConnectorViewModel({}, this.querySurface().getControlModel());
        this.dragDropConnector(new RoutedConnectorSurface(connectorModel, this.surface()));
    }
    setConnectorPoints(cursorPosition) {
        const startColumn = this._dragColumn(), connectorModel = this.dragDropConnector().getControlModel();
        if (this._needToCreateRelation()) {
            const points = determineConnectingPoints(startColumn, this.selection.dropTarget.getControlModel());
            connectorModel.startPoint().connectingPoint(points.start);
            connectorModel.endPoint().connectingPoint(points.end);
        }
        else {
            const _leftConnectionPointX = this.querySurface().rtl() ? this.querySurface().pageWidth() - startColumn.leftConnectionPoint.location.x() : startColumn.leftConnectionPoint.location.x();
            const _rightConnectionPointX = this.querySurface().rtl() ? this.querySurface().pageWidth() - startColumn.rightConnectionPoint.location.x() : startColumn.rightConnectionPoint.location.x();
            const point = Math.abs(_leftConnectionPointX - cursorPosition.left) > Math.abs(_rightConnectionPointX - cursorPosition.left) ?
                startColumn.rightConnectionPoint : startColumn.leftConnectionPoint;
            connectorModel.startPoint().connectingPoint(point);
            this.dragDropConnector().endPoint().rect({ top: cursorPosition.top, left: cursorPosition.left });
        }
    }
    drag(event, uiElement) {
        const scrollOffset = this._scrollProcessor.getScrollOffset();
        uiElement.dataset.leftPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.leftPosition) + (uiElement['scroll'].left + scrollOffset.left));
        uiElement.dataset.topPosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)((0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.topPosition) + (uiElement['scroll'].top + scrollOffset.top));
        uiElement['delta'].left = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.leftPosition) - this.surface()['underCursor']().x - 6;
        uiElement['delta'].top = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(uiElement.dataset.topPosition) - this.surface()['underCursor']().y - 6;
        this.setConnectorPoints(this._getAbsoluteSurfacePosition(uiElement));
        this._scrollProcessor.processOffset({ x: event.clientX, y: event.clientY });
    }
    doStopDrag() {
        this._scrollProcessor.dispose();
        this._scrollProcessor = null;
        this.dragHelperContent.reset();
        this.dragDropConnector() && this.dragDropConnector().dispose();
        this.dragDropConnector(null);
        try {
            let editableCondition = null;
            if (this._dragConditionSurface && !this._dragConditionSurface.isVisible()) {
                editableCondition = this._dragConditionSurface.getControlModel();
                this._dragConditionSurface.isVisible(true);
            }
            const query = this.querySurface().getControlModel();
            if (this._needToCreateRelation()) {
                const nestedColumn = this.selection.dropTarget.getControlModel();
                this.undoEngine().start();
                if (editableCondition) {
                    editableCondition.parentModel().removeChild(editableCondition);
                }
                const condition = query.cerateJoinCondition(this._dragColumn(), nestedColumn);
                this.undoEngine().end();
                if (condition !== null) {
                    this.selection.initialize((0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(condition));
                }
            }
        }
        finally {
            this._dragColumn(null);
        }
    }
    getDragColumn() {
        return this._dragColumn();
    }
}

;// ./dist/query-builder/dragDrop/_dbObjectDragDropHandler.js







class DbObjectDragDropHandler extends external_DevExpress_Analytics_Internal_namespaceObject.DragDropHandler {
    suggestLocation(newControl, query) {
        const posX = Math.max.apply(null, query.tables.peek()
            .filter(t => t !== newControl)
            .map((t) => t.location.x.peek() + t.size.width.peek() * 3 / 2)
            .concat([30]));
        newControl.location.x(posX);
        newControl.location.y(65);
    }
    constructor(surface, selection, _undoEngine, snapHelper, dragHelperContent) {
        super(surface, selection, _undoEngine, snapHelper, dragHelperContent);
        this._undoEngine = _undoEngine;
        this._query = () => this._querySurface().getControlModel();
        this.getDropCallback = (undoEngine, suggestLocation) => ((item, query) => {
            const newControl = query.createChild((0,external_DevExpress_Analytics_Internal_namespaceObject.extend)({ '@ControlType': 'Table', '@Name': item.data.name }, controlsFactory.controlsMap['Table'].defaultVal));
            if (newControl.isInitialized()) {
                query.tryToCreateRelationsByFK(newControl);
            }
            else {
                newControl.isInitialized.subscribe(() => {
                    undoEngine().start();
                    query.tryToCreateRelationsByFK(newControl);
                    undoEngine().end();
                });
            }
            if (suggestLocation) {
                this.suggestLocation(newControl, query);
            }
            return newControl;
        });
        this._querySurface = surface;
        this.cursor = 'arrow';
        this.containment = '.dxqb-designer';
        this['cursorAt'] = {
            top: 0,
            left: 0
        };
        this['helper'] = (draggable) => {
            super.helper(draggable);
            this.recalculateSize(external_DevExpress_Analytics_Elements_namespaceObject.Size.fromString('199, 123'));
            dragHelperContent.setContent(new external_DevExpress_Analytics_Elements_namespaceObject.Rectangle(0, 0, this._size.width(), this._size.height()));
        };
    }
    startDrag(draggable) {
        if (draggable && draggable.name) {
            this._query().dbSchemaProvider.getDbTable(draggable.name);
        }
    }
    doStopDrag(uiElement, _) {
        this.dragHelperContent.reset();
        if (this.selection.dropTarget) {
            const position = this._getAbsoluteSurfacePosition(uiElement);
            this._querySurface().underCursor().x = position.left - this._querySurface()['absolutePosition'].x();
            this._querySurface().underCursor().y = position.top - this._querySurface()['absolutePosition'].y();
            const item = external_ko_namespaceObject.dataFor(uiElement);
            const control = this.getDropCallback(this._undoEngine, false)(item, this._query());
            this.addControl(control, this._querySurface(), this._size);
        }
    }
    addControl(control, dropTargetSurface, size) {
        dropTargetSurface.getControlModel().addChild(control);
        const controlSurface = (0,external_DevExpress_Analytics_Internal_namespaceObject.findSurface)(control);
        if (!controlSurface)
            return;
        controlSurface.rect({ left: dropTargetSurface.underCursor().x, top: dropTargetSurface.underCursor().y, width: size.width() });
        this.selection.initialize(controlSurface);
    }
}

;// ./dist/query-builder/elements/relationSurface.js



class RelationSurface extends external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase {
    constructor(control, context) {
        super(control, context, null);
        this.conditions = external_ko_namespaceObject.observableArray();
        this.template = 'dxqb-relation';
        (0,external_DevExpress_Analytics_Internal_namespaceObject.createObservableArrayMapCollection)(control.conditions, this.conditions, this._createSurface);
    }
    _getChildrenHolderName() {
        return 'conditions';
    }
}

;// ./dist/query-builder/elements/joinConditionModelMeta.js

const ConditionType = {
    Equal: 'Equal',
    NotEqual: 'NotEqual',
    Greater: 'Greater',
    GreaterOrEqual: 'GreaterOrEqual',
    Less: 'Less',
    LessOrEqual: 'LessOrEqual'
};
const joinConditionSerializationInfo = [
    { propertyName: 'left', displayName: 'Left', editor: editorTemplates.getEditor('text'), disabled: true, localizationId: 'AnalyticsCoreStringId.QueryBuilder_LeftOperand' },
    { propertyName: 'right', displayName: 'Right', editor: editorTemplates.getEditor('text'), disabled: true, localizationId: 'AnalyticsCoreStringId.QueryBuilder_RightOperand' },
    { propertyName: 'parentColumnName', modelName: '@Parent' },
    { propertyName: 'nestedColumnName', modelName: '@Nested' },
    {
        propertyName: 'joinType',
        displayName: 'Join Type',
        editor: editorTemplates.getEditor('combobox'),
        defaultVal: 'Inner',
        valuesArray: [
            { value: 'Inner', displayValue: 'Inner join', localizationId: 'DataAccessStringId.RelationEditorRelationTypeInnerJoin' },
            { value: 'LeftOuter', displayValue: 'Left outer join', localizationId: 'DataAccessStringId.RelationEditorRelationTypeLeftOuterJoin' },
            { value: 'RightOuter', displayValue: 'Right outer join', localizationId: 'DataAccessStringId.RelationEditorRelationTypeRightOuterJoin' },
            { value: 'FullOuter', displayValue: 'Full outer join', localizationId: 'DataAccessStringId.RelationEditorRelationTypeFullOuterJoin' },
        ],
        localizationId: 'AnalyticsCoreStringId.QueryBuilder_JoinType'
    },
    {
        propertyName: 'operator',
        modelName: '@Operator',
        displayName: 'Operator',
        editor: editorTemplates.getEditor('combobox'),
        defaultVal: ConditionType.Equal,
        valuesArray: [
            { value: 'Equal', displayValue: 'Equals to', localizationId: 'DataAccessUIStringId.JoinEditorEqualOperator' },
            { value: 'NotEqual', displayValue: 'Does not equal to', localizationId: 'DataAccessUIStringId.JoinEditorNotEqualOperator' },
            { value: 'Greater', displayValue: 'Is greater than', localizationId: 'DataAccessUIStringId.JoinEditorGreaterOperator' },
            { value: 'GreaterOrEqual', displayValue: 'Is greater than or equal to', localizationId: 'DataAccessUIStringId.JoinEditorGreaterOrEqualOperator' },
            { value: 'Less', displayValue: 'Is less than', localizationId: 'DataAccessUIStringId.JoinEditorLessOperator' },
            { value: 'LessOrEqual', displayValue: 'Is less than or equal to', localizationId: 'DataAccessUIStringId.JoinEditorLessOrEqualOperator' }
        ],
        localizationId: 'AnalyticsCoreStringId.QueryBuilder_Operator'
    },
    { propertyName: 'itemType', modelName: '@itemType' }
];

;// ./dist/query-builder/elements/joinConditionSurface.js


class JoinConditionSurface extends RoutedConnectorSurface {
    constructor(control, context) {
        super(control, context);
        this._disposables.push(this.showArrow = external_ko_namespaceObject.pureComputed(() => {
            return control.joinType() === 'LeftOuter' || control.joinType() === 'FullOuter';
        }));
        this._disposables.push(this.showRightArrow = external_ko_namespaceObject.pureComputed(() => {
            return control.joinType() === 'RightOuter' || control.joinType() === 'FullOuter';
        }));
    }
    container() {
        return this.getRoot();
    }
}

;// ./dist/query-builder/utils/_controlsFactory.js





















function _controlsFactory_registerControls() {
    registerControls();
    controlsFactory.registerControl('Unknown', {
        info: metadata_unknownSerializationsInfo,
        type: external_DevExpress_Analytics_Elements_namespaceObject.ElementViewModel,
        nonToolboxItem: true,
        surfaceType: external_DevExpress_Analytics_Elements_namespaceObject.SurfaceElementBase,
        isDeleteDeny: true
    });
    controlsFactory.registerControl('Relation', {
        info: relationSerializationInfo,
        defaultVal: {},
        surfaceType: RelationSurface,
        popularProperties: [],
        type: RelationViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
    controlsFactory.registerControl('JoinCondition', {
        info: joinConditionSerializationInfo,
        defaultVal: {},
        surfaceType: JoinConditionSurface,
        popularProperties: ['_parentColumnName', '_nestedColumnName', 'joinType'],
        type: JoinConditionViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
    controlsFactory.registerControl('Table', {
        info: tableSerializationInfo,
        defaultVal: {},
        surfaceType: TableSurface,
        popularProperties: ['name', 'alias', 'columns'],
        type: TableViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
    controlsFactory.registerControl('Column', {
        info: columnSerializationInfo,
        defaultVal: {},
        surfaceType: ColumnSurface,
        popularProperties: ['name', 'alias', 'selected'],
        type: ColumnViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true,
        isDeleteDeny: true
    });
    controlsFactory.registerControl('Query', {
        info: querySerializationsInfo,
        surfaceType: QuerySurface,
        popularProperties: ['name', 'filterString', 'groupFilterString'],
        type: QueryViewModel,
        elementActionsTypes: [],
        isContainer: true,
        nonToolboxItem: true,
        isDeleteDeny: true
    });
    controlsFactory.registerControl('FTable', {
        info: tableSerializationInfo,
        defaultVal: {},
        surfaceType: FederationTableSurface,
        popularProperties: ['name', 'alias', 'columns'],
        type: FederationTableViewModel,
        elementActionsTypes: [],
        nonToolboxItem: true
    });
    controlsFactory.registerControl('FQuery', {
        info: federationQuerySerializationsInfo,
        surfaceType: FederationQuerySurface,
        popularProperties: ['name'],
        type: FederationQueryViewModel,
        elementActionsTypes: [],
        isContainer: true,
        nonToolboxItem: true,
        isDeleteDeny: true
    });
}

;// ./dist/query-builder/utils/_queryBuilderTreeListController.js

class QueryBuilderTreeListController extends external_DevExpress_Analytics_Widgets_Internal_namespaceObject.TreeListController {
    constructor(undoEngine, query, dragDropHandler) {
        super();
        this.undoEngine = undoEngine;
        this.query = query;
        this._dragDropHandler = dragDropHandler.getDropCallback(undoEngine, true);
    }
    dblClickHandler(item) {
        this.undoEngine().start();
        this._dragDropHandler(item, this.query());
        this.undoEngine().end();
    }
}

;// ./dist/query-builder/_accordionTabInfo.js








const parametersInfo = { propertyName: 'parameters', displayName: 'parameters', editor: { custom: 'dxqb-parameterspanel' } };
const selectedItemInfo = { propertyName: 'selectedItem', displayName: 'selectedItem', editor: { custom: 'dxqb-propertygrid' } };
const fieldsInfo = { propertyName: 'fields', displayName: 'fields', editor: { custom: 'dxqb-fieldspanel' } };
const queryInfo = { propertyName: 'query', displayName: 'query', editor: { custom: 'dxqb-propertygrid' } };
class AccordionTabInfo extends external_DevExpress_Analytics_Utils_namespaceObject.TabInfo {
    static _getSelectedItemPropertyName(model) {
        let text = 'Selection Properties';
        let id = 'AnalyticsCoreStringId.QueryBuilder_SelectionProperties';
        switch (model && model.controlType) {
            case 'Query':
            case 'FQuery':
                text = 'Query Properties';
                id = 'AnalyticsCoreStringId.QueryBuilder_QueryProperties';
                break;
            case 'Table':
            case 'FTable':
                text = 'Table Properties';
                id = 'AnalyticsCoreStringId.QueryBuilder_TableProperties';
                break;
            case 'Column':
                text = 'Column Properties';
                id = 'AnalyticsCoreStringId.QueryBuilder_ColumnProperties';
                break;
            case 'JoinCondition':
                text = 'Relation Properties';
                id = 'AnalyticsCoreStringId.QueryBuilder_RelationProperties';
                break;
        }
        return (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)(text, id);
    }
    static _createWrappedObject(query, commonModel, undoEngine, showParameters) {
        const modelProperties = new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(query, null, 1);
        const modelValues = external_ko_namespaceObject.computed(() => query() && query().parameters);
        const info = [queryInfo, selectedItemInfo, fieldsInfo];
        const object = {
            selectedItem: commonModel,
            query: {
                editableObject: query,
                properties: modelProperties
            },
            fields: commonModel,
            isPropertyVisible: (propertyName) => {
                if (propertyName === 'selectedItem') {
                    return commonModel.editableObject() !== query();
                }
                return true;
            }
        };
        if (showParameters) {
            object['parameters'] = {
                values: modelValues,
                addHandler: () => new ParameterViewModel({ '@Type': external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemString }),
                collapsed: false,
                undoEngine: undoEngine,
                isVisibleButton: (index, button) => button === 'add' || button === 'delete',
                template: '#dxqb-collectioneditor-template',
                textEmptyArray: { text: 'Click the Add button to create a parameter.', localizationId: 'AnalyticsCoreStringId.QueryBuilder_PageConfigureParametersEmpty' }
            };
            info.push(parametersInfo);
        }
        object['getInfo'] = () => info;
        return object;
    }
    static _createGroups(editableObject, showParameters) {
        const groups = {
            'Query': {
                info: [queryInfo],
                displayName: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Query Properties', 'AnalyticsCoreStringId.QueryBuilder_QueryProperties')
            },
            'SelectedItem': {
                info: [selectedItemInfo],
                displayName: () => this._getSelectedItemPropertyName(editableObject())
            },
            'Fields': {
                info: [fieldsInfo],
                displayName: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Available tables and views', 'AnalyticsCoreStringId.QueryBuilder_AvailableTables')
            }
        };
        if (showParameters)
            groups['Parameters'] = {
                info: [parametersInfo],
                displayName: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Parameters', 'AnalyticsCoreStringId.QueryBuilder_Parameters')
            };
        return groups;
    }
    static _createQBPropertyGrid(query, commonModel, undoEngine, showParameters) {
        const object = this._createWrappedObject(query, commonModel, undoEngine, showParameters);
        const grid = new external_DevExpress_Analytics_Internal_namespaceObject.ControlProperties(external_ko_namespaceObject.observable(object), {
            groups: this._createGroups(commonModel.editableObject, showParameters),
            editors: object['getInfo']()
        }, undefined, false);
        return grid;
    }
    _getGroupByName(name) {
        return this.model.groups.filter(x => x['_displayName'] === name)[0];
    }
    constructor(query, itemPropertiesTabInfoModel, undoEngine, focused, showParameters) {
        super({
            text: 'Properties',
            template: 'dxqb-properties-wrapper',
            model: AccordionTabInfo._createQBPropertyGrid(query, itemPropertiesTabInfoModel, undoEngine, showParameters)
        });
        this.active = true;
        this._getGroupByName('Fields').collapsed(false);
        this._disposables.push(focused.subscribe((newVal) => {
            if (!(newVal instanceof QueryViewModel)) {
                const group = this._getGroupByName('SelectedItem');
                group.collapsed(false);
            }
        }));
    }
}
class SelectedTabInfo extends external_DevExpress_Analytics_Utils_namespaceObject.TabInfo {
    constructor(model) {
        super({
            text: 'Properties',
            template: 'dxqb-properties-wrapper-editorlist',
            model: model
        });
        this.model = model;
    }
}

;// ./dist/query-builder/_bindings.js








external_ko_namespaceObject.bindingHandlers['dxdTableView'] = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        const templateHtml = (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxd-tableview');
        let $element = (0,external_DevExpress_Analytics_Internal_namespaceObject.createJQueryElement)(element).append(templateHtml), $titles, $content;
        const rtl = !!bindingContext.$root.rtl, value = external_ko_namespaceObject.unwrap(valueAccessor());
        const onDataScroll = (e) => {
            if (e.scrollOffset.left >= 0) {
                if (!$titles)
                    $titles = $element.find('.dxd-tableview-titles');
                if (!$content)
                    $content = $element.find('.dxd-tableview-data table');
                $titles.offset({ left: $content.offset().left, top: $titles.offset().top });
            }
        };
        const onDataScrollInitialized = (e) => {
            $content = null;
            if (!rtl)
                return;
            const dxScroll = e.component;
            setTimeout(() => dxScroll.scrollTo({ left: dxScroll.scrollWidth(), top: 0 }), 1);
        };
        external_ko_namespaceObject.applyBindings({
            onDataScroll: onDataScroll,
            onDataScrollInitialized: onDataScrollInitialized,
            data: value,
            rtl: rtl,
            noDataText: external_DevExpress_Analytics_Internal_namespaceObject.noDataText,
            isImage: index => value.schema[index].type === external_DevExpress_Analytics_Internal_namespaceObject.DotnetTypes.SystemBiteArray,
            isImageTooLarge: cellValue => cellValue[0] === '!',
            getImageTooLargeText: cellValue => 'Image too large (' + cellValue.substring(1) + ' bytes)'
        }, $element.children()[0]);
        $element.find('.dxd-tableview-titles .dxd-tableview-resizable').each((index, resizable) => {
            const $title = (0,external_DevExpress_Analytics_Internal_namespaceObject.createJQueryElement)(resizable).find('.dxd-tableview-cell-text');
            const $column = $element.find('.dxd-tableview-data .dxd-tableview-resizable' + index);
            if (index < value.schema.length - 1) {
                const resizableElement = new external_DevExpress_Analytics_Internal_namespaceObject.Resizable(resizable, {
                    handles: 'e',
                    start: (e, ui) => { },
                    stop: () => { },
                    resize: (e, element) => {
                        const startResizePosition = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(element.dataset.originalLeftMousePosition);
                        const originalWidth = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertFromCssPixelUnits)(element.dataset.originalWidth);
                        const sizeDiff = e.x - startResizePosition;
                        const newWidth = originalWidth + sizeDiff;
                        element.style.width = (0,external_DevExpress_Analytics_Internal_namespaceObject.convertToCssPixelUnits)(newWidth);
                        $title.outerWidth(newWidth);
                        $column.outerWidth(newWidth);
                        $column.parent().width(newWidth);
                    }
                }).initialize();
                (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(resizable, () => {
                    resizableElement.dispose();
                });
            }
            const maxWidth = Math.max($title.width(), $column.width());
            $title.width(maxWidth);
            $column.width(maxWidth);
        });
        (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)($element.children()[0], () => {
            $element = null;
        });
        return { controlsDescendantBindings: true };
    }
};

;// external "DevExpress.config"
const external_DevExpress_config_namespaceObject = DevExpress.config;
var external_DevExpress_config_default = /*#__PURE__*/__webpack_require__.n(external_DevExpress_config_namespaceObject);
;// ./dist/query-builder/_initializer.js





































const QueryBuilderElements = {
    Surface: 'dxrd-surface-template-base',
    Toolbar: 'dxqb-toolbar',
    RightPanel: 'dx-right-panel-lightweight',
    RightPanelSwitcher: 'dx-right-panel-switcher',
    DataPreview: 'dxqb-popup#data',
    SqlPreview: 'dxqb-popup#sql'
};
function customizeDesignerActions(designerModel, nextCustomizer) {
    const query = designerModel.model;
    return ((actions) => {
        const del = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Delete');
        del.imageClassName = 'dx-icon-dxrd-image-recycle-bin';
        del.imageTemplateName = 'dxrd-svg-operations-recycle_bin_xl';
        const undo = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Undo');
        undo.disabled = external_ko_namespaceObject.pureComputed(() => designerModel.isLoading() || !designerModel.undoEngine().undoEnabled());
        const redo = (0,external_DevExpress_Analytics_Internal_namespaceObject.findFirstItemMatchesCondition)(actions, action => action.text === 'Redo');
        actions.splice(0, actions.length, del, undo, redo);
        actions.push({
            id: ActionId.Save,
            text: 'Save',
            displayText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Save', 'AnalyticsCoreStringId.MenuButtons_Save'),
            imageClassName: 'dxqb-image-save',
            imageTemplateName: 'dxrd-svg-menu-save',
            disabled: designerModel.isLoading,
            visible: true,
            hotKey: { ctrlKey: true, keyCode: 'S'.charCodeAt(0) },
            clickAction: () => {
                query().save();
            },
            hasSeparator: true
        });
        actions.push({
            id: ActionId.DataPreview,
            text: 'Preview Results',
            displayText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Preview Results', 'DataAccessUIStringId.QueryBuilderButtons_PreviewResults'),
            imageClassName: 'dxrd-image-data-preview',
            imageTemplateName: 'dxrd-svg-queryBuilder-data_preview',
            disabled: designerModel.isLoading,
            visible: true,
            hotKey: { ctrlKey: true, keyCode: 'P'.charCodeAt(0) },
            clickAction: () => {
                if (!query().canSave())
                    return;
                designerModel.showPreview();
            },
            hasSeparator: true
        });
        actions.push({
            id: ActionId.SelectStatementPreview,
            text: 'Preview Select Statement',
            displayText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Preview Select Statement', 'AnalyticsCoreStringId.QueryBuilder_PreviewSelectStatement_Tooltip'),
            imageClassName: 'dxrd-image-selectstatement-preview',
            imageTemplateName: 'dxrd-svg-queryBuilder-select_statment',
            disabled: designerModel.isLoading,
            visible: true,
            hotKey: { ctrlKey: true, keyCode: 'E'.charCodeAt(0) },
            clickAction: () => {
                if (!query().canSave())
                    return;
                designerModel.showStatement();
            },
            hasSeparator: true
        });
        nextCustomizer && nextCustomizer(actions);
    });
}
function updateQueryBuilderSurfaceContentSize(getRoot, surfaceSize, surface, updateLayoutCallbacks) {
    return () => {
        const $root = getRoot();
        const rightAreaWidth = $root.find('.dxrd-right-panel:visible').outerWidth() || 0;
        const surfaceWidth = ($root.get(0)?.clientWidth ?? $root.width()) - (rightAreaWidth + 5);
        $root.find('.dxrd-surface-wrapper').css(surface().rtl() ?
            { 'left': rightAreaWidth + 'px', 'right': '0px', 'width': surfaceWidth + 'px' } :
            { 'left': '0px', 'right': rightAreaWidth + 'px', 'width': surfaceWidth + 'px' });
        surfaceSize(surfaceWidth);
        surface().pageWidth(surfaceWidth);
    };
}
function createIsLoadingFlag(model, dbSchemaProvider) {
    const isDbSchemaLoaded = external_ko_namespaceObject.observable(false);
    model()._disposables.push(dbSchemaProvider.subscribe(() => { isDbSchemaLoaded(false); }));
    return external_ko_namespaceObject.pureComputed(function () {
        dbSchemaProvider.peek().getDbSchema().done(() => {
            isDbSchemaLoaded(true);
        });
        if (isDbSchemaLoaded()) {
            return model().tables.peek().some(function (table) {
                return !table.isInitialized();
            });
        }
        else {
            return true;
        }
    });
}
function _createDesignerModel(query, surface, options) {
    const querySubscription = query.subscribe((newValue) => {
        surface(new QuerySurface(newValue));
        surface().rtl(!!options.rtl);
    });
    const selection = new external_DevExpress_Analytics_Internal_namespaceObject.SurfaceSelection(['alias', 'name', 'sortOrder']);
    const designer = (0,external_DevExpress_Analytics_Internal_namespaceObject.createDesigner)(query, surface, controlsFactory, undefined, undefined, undefined, !!options.rtl, selection);
    designer.addDisposables(querySubscription);
    designer.findControl = (s, e) => {
        const $childs = Array.from((0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)('.dxqb-main').children('.dxrd-control'));
        $childs.forEach((child) => {
            const $child = (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(child);
            if ($child.offset().top <= e.clientY && $child.offset().left <= e.clientX) {
                designer.selection.focused(external_ko_namespaceObject.dataFor($child[0]));
                return;
            }
        });
    };
    designer.addDisposables({ dispose: () => designer.findControl = null });
    return designer;
}
function _updateSurfaceContentSizeSubscription(element, designerModel, surface, callback) {
    const updateSurfaceContentSize_ = updateQueryBuilderSurfaceContentSize(() => (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).find('.dxqb-designer'), designerModel.surfaceSize, surface);
    designerModel.addDisposables(surface.subscribe(() => { updateSurfaceContentSize_(); }));
    const onResize = () => {
        setTimeout(() => updateSurfaceContentSize_());
    };
    window.addEventListener('resize', onResize);
    (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(element, function () {
        window.removeEventListener('resize', onResize);
        designerModel.disposableContainer.dispose();
    });
    designerModel.addDisposables({
        dispose: designerModel.tabPanel.events.on('widthChanged', (args) => {
            updateSurfaceContentSize_();
        })
    });
    designerModel.updateSurfaceSize = () => {
        updateSurfaceContentSize_();
    };
    designerModel.updateSurface = () => {
        updateSurfaceContentSize_();
        callback && callback();
    };
}
function _createQueryBuilder(element, options, callbacks, applyBindings = true) {
    const disposableCallback = callbacks && callbacks.onServerError && (0,external_DevExpress_Analytics_Internal_namespaceObject.processErrorEvent)(callbacks.onServerError);
    const wrapper = options.requestWrapper || new RequestWrapper();
    const parametersMode = options.parametersMode || ParametersMode.ReadWrite;
    HandlerUri(options.handlerUri);
    if (options.queryModelJson) {
        const dataSource = new SqlDataSource(JSON.parse(options.dataSourceJson));
        options.dbSchemaProvider = (0,external_DevExpress_Analytics_Internal_namespaceObject._wrapModelInObservable)(options.dbSchemaProvider);
        options.dbSchemaProvider(new DBSchemaProvider(dataSource.connection));
        options.querySource = (0,external_DevExpress_Analytics_Internal_namespaceObject._wrapModelInObservable)(options.querySource);
        options.querySource(JSON.parse(options.queryModelJson));
    }
    const dataConnection = options.dbSchemaProvider().connection;
    const query = external_ko_namespaceObject.observable(), surface = external_ko_namespaceObject.observable(), treeListOptions = external_ko_namespaceObject.observable();
    const beforeSaveCallback = (data) => {
        callbacks.saveQueryRequested({
            queryLayout: encodeURIComponent(JSON.stringify(data)),
            connection: encodeURIComponent(serializeDataConnection(dataConnection))
        });
    };
    const initQuery = (querySource) => {
        query(new QueryViewModel(querySource, options.dbSchemaProvider(), parametersMode, (data) => beforeSaveCallback(data)));
    };
    initQuery(options.querySource());
    const designerModel = _createDesignerModel(query, surface, options);
    disposableCallback && designerModel.addDisposables(disposableCallback);
    designerModel.rootStyle = 'dxqb-designer dxd-back-primary-invariant';
    const previewPopupContainer = external_DevExpress_Analytics_Internal_namespaceObject.getParentContainer;
    designerModel.dataPreview = {
        isLoading: external_ko_namespaceObject.observable(false),
        isVisible: external_ko_namespaceObject.observable(false),
        title: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Data Preview (First 100 Rows Displayed)', 'AnalyticsCoreStringId.DataPreview_Title'),
        template: 'dxqb-data-preview',
        data: {
            value: external_ko_namespaceObject.observable()
        },
        okButtonText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('OK', 'DataAccessUIStringId.Button_OK'),
        okButtonHandler: (e) => {
            e.model.isVisible(false);
        },
        container: previewPopupContainer
    };
    designerModel.selectStatmentPreview = {
        isLoading: external_ko_namespaceObject.observable(false),
        isVisible: external_ko_namespaceObject.observable(false),
        template: 'dxqb-selectstatment-preview',
        title: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Select Statement Preview', 'AnalyticsCoreStringId.QueryBuilder_SelectStatementPreview_Title'),
        data: {
            value: external_ko_namespaceObject.observable(),
            aceOptions: createDefaultSQLAceOptions(true),
            aceAvailable: (0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.aceAvailable)(),
            additionalOptions: createDefaultSQLAdditionalOptions((newVal) => { designerModel.selectStatmentPreview.data.value(newVal); }),
            languageHelper: createDefaultSQLLanguageHelper()
        },
        okButtonText: () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('OK', 'DataAccessUIStringId.Button_OK'),
        okButtonHandler: (e) => {
            e.model.isVisible(false);
        },
        container: previewPopupContainer
    };
    designerModel.parts = [
        { id: QueryBuilderElements.Surface, templateName: QueryBuilderElements.Surface, model: designerModel },
        { id: QueryBuilderElements.Toolbar, templateName: QueryBuilderElements.Toolbar, model: designerModel },
        { id: QueryBuilderElements.RightPanel, templateName: QueryBuilderElements.RightPanel, model: designerModel },
        { id: QueryBuilderElements.DataPreview, templateName: QueryBuilderElements.DataPreview.split('#')[0], model: designerModel.dataPreview },
        { id: QueryBuilderElements.SqlPreview, templateName: QueryBuilderElements.SqlPreview.split('#')[0], model: designerModel.selectStatmentPreview }
    ];
    designerModel.columnDragHandler = new ColumnDragHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.fieldDragHandler = new DbObjectDragDropHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.connectionPointDragHandler = designerModel.columnDragHandler;
    designerModel.resizeHandler['handles'] = 'e,w';
    designerModel.columnsLoadingMsg = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Loading...', 'DataAccessWebStringId.QueryBuilder_ColumnsLoading');
    const init = (querySource) => {
        initQuery(querySource);
        treeListOptions({
            itemsProvider: options.dbSchemaProvider(),
            treeListController: new QueryBuilderTreeListController(designerModel.undoEngine, query, designerModel.fieldDragHandler),
            selectedPath: external_ko_namespaceObject.observable(),
            pageSize: 100,
            templateName: 'dxqb-treelist-item-with-search'
        });
    };
    designerModel.addDisposables(options.querySource.subscribe((newValue) => {
        init(newValue);
    }));
    init(options.querySource());
    const tablesTop = external_ko_namespaceObject.observable((0,external_DevExpress_Analytics_Internal_namespaceObject.calculateWithZoomFactor)(355)), itemPropertiesTabInfoModel = {
        editableObject: designerModel.editableObject,
        properties: new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(designerModel.editableObject, null, 1),
        fieldListModel: { treeListOptions: treeListOptions },
        tablesTop: tablesTop,
        searchPlaceholder: () => (0,external_DevExpress_Analytics_Internal_namespaceObject.searchPlaceholder)()
    };
    const tabPanelItem = new AccordionTabInfo(query, itemPropertiesTabInfoModel, designerModel.undoEngine, designerModel.selection.focused, parametersMode === ParametersMode.ReadWrite);
    const tabPanel = designerModel.tabPanel;
    tabPanel.removeTabs();
    tabPanel.addTab(tabPanelItem);
    tabPanel.width = (0,external_DevExpress_Analytics_Internal_namespaceObject.calculateWithZoomFactor)(375);
    designerModel.fieldDragHandler = new DbObjectDragDropHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.fieldListProvider = new QueryBuilderObjectsProvider(query, QueryBuilderObjectsProvider.whereClauseObjectsFilter);
    designerModel.dataBindingsProvider = designerModel.fieldListProvider;
    designerModel.parametersBindingsProvider = options.parametersItemsProvider || designerModel.dataBindingsProvider;
    designerModel.dataBindingsGroupProvider = new QueryBuilderObjectsProvider(query, QueryBuilderObjectsProvider.groupByObjectsFilter);
    designerModel.isLoading = createIsLoadingFlag(designerModel.model, options.dbSchemaProvider);
    designerModel.actionLists = new external_DevExpress_Analytics_Internal_namespaceObject.ActionLists(surface, designerModel.selection, designerModel.undoEngine, customizeDesignerActions(designerModel, callbacks && callbacks.customizeActions));
    if (!designerModel.isLoading()) {
        designerModel.undoEngine && designerModel.undoEngine().clearHistory();
    }
    designerModel.addDisposables(designerModel.isLoading.subscribe((value) => {
        designerModel.undoEngine && designerModel.undoEngine().clearHistory();
    }));
    designerModel.selection.focused(surface());
    const subscribe = surface.subscribe((newValue) => {
        designerModel.selection.focused(newValue);
    });
    designerModel.addDisposables({ dispose: () => subscribe.dispose() });
    designerModel.addDisposables(designerModel.editableObject.subscribe((newValue) => {
        tablesTop.notifySubscribers(null);
    }));
    (0,external_DevExpress_Analytics_Internal_namespaceObject.appendStaticContextToRootViewModel)(designerModel);
    if (applyBindings) {
        callbacks && callbacks.beforeRender && callbacks.beforeRender(designerModel);
        (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).empty();
        external_ko_namespaceObject.cleanNode(element);
        external_ko_namespaceObject.applyBindings(designerModel, element);
    }
    _updateSurfaceContentSizeSubscription(element, designerModel, surface, () => tablesTop(355));
    if (applyBindings) {
        designerModel.updateSurface();
    }
    designerModel.showPreview = () => {
        designerModel.dataPreview.isLoading(true);
        designerModel.dataPreview.isVisible(true);
        wrapper.getDataPreview(dataConnection, (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)(query().serialize(true)))
            .done((data) => {
            designerModel.dataPreview.data.value(JSON.parse(data.dataPreviewJSON));
            designerModel.dataPreview.isLoading(false);
        }).fail((data) => {
            designerModel.dataPreview.isVisible(false);
            (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data));
        });
    };
    designerModel.showStatement = () => {
        designerModel.selectStatmentPreview.isLoading(true);
        designerModel.selectStatmentPreview.isVisible(true);
        wrapper.getSelectStatement(dataConnection, (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)(query().serialize(true)))
            .done((data) => {
            if (data.errorMessage)
                (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)(data.errorMessage);
            designerModel.selectStatmentPreview.data.value(data.sqlSelectStatement);
            designerModel.selectStatmentPreview.isLoading(false);
        }).fail((data) => {
            designerModel.selectStatmentPreview.isVisible(false);
            (0,external_DevExpress_Analytics_Internal_namespaceObject.ShowMessage)((0,external_DevExpress_Analytics_Internal_namespaceObject.getErrorMessage)(data));
        });
    };
    return designerModel;
}
function createQueryBuilder(element, options, callbacks, applyBindings = true) {
    if (options.localization) {
        (0,external_DevExpress_Analytics_Utils_namespaceObject.addCultureInfo)({
            messages: options.localization
        });
    }
    external_DevExpress_config_default()({ rtlEnabled: !!options.rtl });
    _controlsFactory_registerControls();
    const promises = [];
    callbacks && callbacks.customizeLocalization && callbacks.customizeLocalization(promises);
    return (0,external_DevExpress_Analytics_Internal_namespaceObject.resolveFromPromises)(promises, () => {
        return (0,external_DevExpress_Analytics_Internal_namespaceObject.troubleshootingPageWrapper)(() => _createQueryBuilder(element, options, callbacks, applyBindings), options.developmentMode, element);
    });
}
function createQueryBuilderSurface(element, options, queryCreator) {
    const query = external_ko_namespaceObject.observable(), surface = external_ko_namespaceObject.observable();
    _controlsFactory_registerControls();
    const designerModel = _createDesignerModel(query, surface, options);
    const selectedTabPanel = new SelectedTabInfo(new external_DevExpress_Analytics_Widgets_namespaceObject.ObjectProperties(designerModel.editableObject, null, 1));
    const tabPanel = designerModel.tabPanel;
    const collased = external_ko_namespaceObject.observable(tabPanel.collapsed);
    let isUpdate = false;
    const lock = (callback) => {
        if (!isUpdate) {
            isUpdate = true;
            callback();
            isUpdate = false;
        }
    };
    designerModel.addDisposables({
        dispose: tabPanel.events.on('collapsedChanged', (args) => {
            lock(() => collased(tabPanel.collapsed));
        })
    });
    designerModel.addDisposables(collased.subscribe((newVal) => {
        lock(() => tabPanel.collapsed = newVal);
    }));
    const switcher = new RightPanelSwitcher(collased, designerModel.editableObject, options.showPropertyGridCondition, AccordionTabInfo._getSelectedItemPropertyName);
    designerModel.addDisposables(selectedTabPanel, tabPanel, switcher);
    tabPanel.removeTabs();
    tabPanel.addTab(selectedTabPanel);
    tabPanel.width = (0,external_DevExpress_Analytics_Internal_namespaceObject.calculateWithZoomFactor)(325);
    designerModel.parts = [
        { id: QueryBuilderElements.Surface, templateName: QueryBuilderElements.Surface, model: designerModel },
        { id: QueryBuilderElements.RightPanel, templateName: QueryBuilderElements.RightPanel, model: designerModel },
        { id: QueryBuilderElements.RightPanelSwitcher, templateName: QueryBuilderElements.RightPanelSwitcher, model: switcher },
    ];
    designerModel.columnDragHandler = new ColumnDragHandler(surface, designerModel.selection, designerModel.undoEngine, designerModel.snapHelper, designerModel.dragHelperContent);
    designerModel.connectionPointDragHandler = designerModel.columnDragHandler;
    designerModel.selection.focused(surface());
    designerModel.addDisposables(surface.subscribe((newValue) => {
        designerModel.selection.focused(newValue);
    }));
    designerModel.rootStyle = 'dxqb-designer dxd-back-primary-invariant';
    designerModel.resizeHandler['handles'] = 'e,w';
    designerModel.columnsLoadingMsg = () => (0,external_DevExpress_Analytics_Utils_namespaceObject.getLocalization)('Loading...', 'DataAccessWebStringId.QueryBuilder_ColumnsLoading');
    (0,external_DevExpress_Analytics_Internal_namespaceObject.appendStaticContextToRootViewModel)(designerModel);
    query(queryCreator(options));
    (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).empty();
    external_ko_namespaceObject.cleanNode(element);
    external_ko_namespaceObject.applyBindings(designerModel, element);
    _updateSurfaceContentSizeSubscription(element, designerModel, surface);
    designerModel.isLoading(false);
    designerModel.updateSurface();
    (0,external_DevExpress_Analytics_Internal_namespaceObject.addDisposeCallback)(element, function () {
        query() && query().dispose();
        surface() && surface().dispose();
        queryCreator = null;
        designerModel.disposableContainer.dispose();
        designerModel.dispose();
    });
    return designerModel;
}

;// ./dist/queryBuilder-internal.js






































;// ./dist/queryBuilder-elements.js

































;// ./dist/queryBuilder-elements-metadata.js

















;// ./dist/query-builder/binding/jsQueryBuilder.js




class JSQueryBuilder {
    get queryBuilderModel() {
        return this._queryBuilderModel();
    }
    set queryBuilderModel(newVal) {
        this._queryBuilderModel(newVal);
    }
    constructor(_queryBuilderModel) {
        this._queryBuilderModel = _queryBuilderModel;
    }
    UpdateLocalization(localization) {
        (0,external_DevExpress_Analytics_Utils_namespaceObject.updateLocalization)(localization);
    }
    GetQueryBuilderModel() {
        return this.queryBuilderModel;
    }
    GetJsonQueryModel() {
        return { 'Query': this.queryBuilderModel.model().serialize() };
    }
    GetSaveQueryModel() {
        return {
            queryLayout: (0,external_DevExpress_Analytics_Utils_namespaceObject.PrepareRequestArgs)(this.GetJsonQueryModel()),
            connection: this.SerializeDataConnection()
        };
    }
    SerializeDataConnection() {
        const connection = this.queryBuilderModel.model().dbSchemaProvider.connection;
        return serializeDataConnection(connection);
    }
    AdjustControlCore() {
        this.queryBuilderModel && this.queryBuilderModel.updateSurfaceSize();
    }
    Save() {
        this.queryBuilderModel && this.queryBuilderModel.model().onSave();
    }
    ShowPreview() {
        this.queryBuilderModel && this.queryBuilderModel.showPreview();
    }
    IsQueryValid() {
        return this.queryBuilderModel && this.queryBuilderModel.model().isValid();
    }
    OnCallback(result) {
        if (result.queryValidationError) {
            (0,external_DevExpress_Analytics_Internal_namespaceObject.NotifyAboutWarning)(result.queryValidationError, true);
        }
    }
}

;// external "DevExpress.localization"
const external_DevExpress_localization_namespaceObject = DevExpress.localization;
;// external "DevExpress.Analytics.Localization"
const external_DevExpress_Analytics_Localization_namespaceObject = DevExpress.Analytics.Localization;
;// ./dist/query-builder/binding/eventGenerator.js


class EventGenerator {
    static generateQueryBuilderEvents(fireEvent) {
        const beforeRenderEvent = { publicName: 'BeforeRender', privateName: 'beforeRender' };
        const saveQueryRequestedEvent = { publicName: 'SaveQueryRequested', privateName: 'saveQueryRequested' };
        const customizeActionsEvent = { publicName: 'CustomizeMenuActions', privateName: 'customizeActions' };
        const customizeLocalizationEvent = { publicName: 'CustomizeLocalization', privateName: 'customizeLocalization' };
        const onServerErrorEvent = { publicName: 'OnServerError', 'privateName': 'onServerError' };
        customizeActionsEvent['callback'] = function customizeActions(actions) {
            fireEvent(customizeActionsEvent.publicName, {
                Actions: actions,
                GetById: (actionId) => {
                    return actionId ? actions.filter(function (item) { return actionId === item.id; })[0] : null;
                }
            });
        };
        customizeLocalizationEvent['callback'] = function customizeLocalization(localizationCallbacks) {
            fireEvent(customizeLocalizationEvent.publicName, {
                LoadMessages: (messages) => {
                    if (!messages)
                        return;
                    if (typeof messages.then === 'function') {
                        localizationCallbacks.push(messages);
                    }
                    else {
                        (0,external_DevExpress_Analytics_Localization_namespaceObject.loadMessages)(messages);
                    }
                },
                WidgetLocalization: external_DevExpress_localization_namespaceObject
            });
        };
        onServerErrorEvent['callback'] = function onServerError(args) {
            fireEvent(onServerErrorEvent.publicName, { Error: args });
        };
        beforeRenderEvent['callback'] = function beforeRender(designerModel) {
            fireEvent(beforeRenderEvent.publicName, designerModel);
        };
        saveQueryRequestedEvent['callback'] = function saveQueryRequested(queryModel) {
            fireEvent(saveQueryRequestedEvent.publicName, queryModel);
        };
        return [
            beforeRenderEvent,
            saveQueryRequestedEvent,
            customizeActionsEvent,
            customizeLocalizationEvent,
            onServerErrorEvent
        ];
    }
}

;// ./dist/query-builder/binding/jsQueryBuilderBinding.js











class JSQueryBuilderBinding extends external_DevExpress_Analytics_Internal_namespaceObject.JSDesignerBindingCommon {
    _applyBindings(model, _$element) {
        (0,external_DevExpress_Analytics_Internal_namespaceObject.troubleshootingPageWrapper)(() => {
            this._disposables.push(model);
            _$element.children().remove();
            const child = _$element.append(this._templateHtml).children()[0];
            if (!child)
                return;
            external_ko_namespaceObject.cleanNode(child);
            this._callbacks && this._callbacks.beforeRender && this._callbacks.beforeRender(model);
            external_ko_namespaceObject.applyBindings(model, child);
            const updateSizeTimeout = setTimeout(() => {
                model && model.updateSurfaceSize();
            }, 1);
            this._disposables.push({ dispose: () => clearTimeout(updateSizeTimeout) });
        }, this.developmentMode, _$element);
    }
    _initializeCallbacks() {
        if (this._options.callbacks) {
            const queryBuilderEvents = EventGenerator.generateQueryBuilderEvents((eventName, args) => {
                this._fireEvent(eventName, args);
            });
            this._checkCallbackName(queryBuilderEvents);
            const availableEvents = this._generateCallbackDictionary(queryBuilderEvents);
            return availableEvents;
        }
    }
    _createModel(element) {
        this._callbacks = this._initializeCallbacks();
        return createQueryBuilder(element, this._options, this._callbacks, false);
    }
    constructor(options, customEventRaiser) {
        super(external_ko_namespaceObject.unwrap(options), customEventRaiser);
        this.options = options;
        this._deferreds = [];
        this._templateHtml = (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-querybuilder');
        options.queryBuilderModel = (0,external_DevExpress_Analytics_Internal_namespaceObject._wrapModelInObservable)(options.queryBuilderModel);
        this.sender = new JSQueryBuilder(options.queryBuilderModel);
        options.callbacks && options.callbacks._eventSenderCreated && options.callbacks._eventSenderCreated(this.sender);
    }
    dispose() {
        (this._deferreds || []).forEach((deferred) => {
            deferred.reject();
        });
        super.dispose();
    }
    applyBindings(element) {
        const _$element = (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element);
        const requestOptions = this._options.requestOptions;
        const applyModel = () => {
            if (requestOptions && requestOptions.invokeAction) {
                this._options.handlerUri = this._getServerActionUrl(requestOptions.host, requestOptions.invokeAction);
            }
            this._deferreds.push(this._createModel(element)
                .done((queryBuilderModel) => {
                this.options.queryBuilderModel(queryBuilderModel);
                this.sender.queryBuilderModel = queryBuilderModel;
                this._createDisposeFunction(element);
                this._applyBindings(this.sender.queryBuilderModel, _$element);
            }));
        };
        if (requestOptions) {
            this._getLocalizationRequest().done((localization) => {
                localization && (0,external_DevExpress_Analytics_Utils_namespaceObject.addCultureInfo)(localization);
            }).always(() => {
                applyModel();
            });
        }
        else {
            applyModel();
        }
    }
}
const queryBuilderBindingName = 'dxQueryBuilder';
class DxQueryBuilder extends external_DevExpress_Analytics_Internal_namespaceObject.DxAnalyticsComponentCommon {
    getBindingName() {
        return queryBuilderBindingName;
    }
}
(0,external_DevExpress_Analytics_Widgets_Internal_namespaceObject.registerBaseBinding)(queryBuilderBindingName, '$data');
external_ko_namespaceObject.bindingHandlers[queryBuilderBindingName] = {
    init: function (element, valueAccessor) {
        const values = external_ko_namespaceObject.unwrap(valueAccessor());
        new JSQueryBuilderBinding(values || {}).applyBindings(element);
        return { controlsDescendantBindings: true };
    }
};
external_ko_namespaceObject.bindingHandlers['dxQueryBuilderSurface'] = {
    init: function (element, valueAccessor) {
        const values = external_ko_namespaceObject.unwrap(valueAccessor());
        const options = values.options;
        const templateHtml = (0,external_DevExpress_Analytics_Widgets_namespaceObject.getTemplate)('dxrd-querybuilder'), $element = (0,external_DevExpress_Analytics_Internal_namespaceObject.$dx)(element).append(templateHtml);
        const model = createQueryBuilderSurface($element.children()[0], options, values.creator);
        options.queryBuilderModel(model);
        return { controlsDescendantBindings: true };
    }
};

;// ./dist/querybuilder.js









;// ./dist/bundle/_add-querybuilder-to-bundle.js















function _addQueryBuilderToBundle(bundle) {
    bundle.Analytics = bundle.Analytics || {};
    bundle.QueryBuilder = querybuilder_namespaceObject;
    bundle.Analytics.Diagram = analytics_diagram_namespaceObject;
    bundle.Analytics.Data = analytics_data_namespaceObject;
    bundle.Analytics.Data.Internal = analytics_data_internal_namespaceObject;
    bundle.Analytics.Data.Utils = analytics_data_utils_namespaceObject;
    bundle.Analytics.Data.Metadata = analytics_data_metadata_namespaceObject;
    bundle.Analytics.Wizard = analytics_wizard_namespaceObject;
    bundle.Analytics.Wizard.Internal = analytics_wizard_internal_namespaceObject;
    bundle.QueryBuilder.Widgets = queryBuilder_widgets_namespaceObject;
    bundle.QueryBuilder.Widgets.Internal = queryBuilder_widgets_internal_namespaceObject;
    bundle.QueryBuilder.Metadata = queryBuilder_metadata_namespaceObject;
    bundle.QueryBuilder.Utils = queryBuilder_utils_namespaceObject;
    bundle.QueryBuilder.Internal = queryBuilder_internal_namespaceObject;
    bundle.QueryBuilder.Elements = queryBuilder_elements_namespaceObject;
    bundle.QueryBuilder.Elements.Metadata = queryBuilder_elements_metadata_namespaceObject;
    return bundle;
}

;// ./dist/bundle/query-builder-bundle.js

const query_builder_bundle_DevExpress = window.DevExpress || {};
_addQueryBuilderToBundle(query_builder_bundle_DevExpress);
/* harmony default export */ const query_builder_bundle = ({
    QueryBuilder: query_builder_bundle_DevExpress['QueryBuilder'],
    Analytics: query_builder_bundle_DevExpress['Analytics'],
});

/******/ })()
;