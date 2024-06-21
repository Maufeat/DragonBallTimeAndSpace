using System;
using Framework.Base;
using Framework.Base.MVC;

namespace Framework.Managers
{
    public class ModelManager : ManagerBaseDefault, IManager, IModelManager
    {
        private ModelManager()
        {
            base.ManagerName = "model_manager";
        }

        public static ModelManager Instance
        {
            get
            {
                if (ModelManager._instance == null)
                {
                    ModelManager._instance = new ModelManager();
                }
                return ModelManager._instance;
            }
        }

        public void AddModel(IModel model)
        {
            if (model != null)
            {
                this._models.Add(model.ModelName, model);
                return;
            }
            throw new Exception("model is null");
        }

        public ModelT AddModelByType<ModelT>() where ModelT : IModel, new()
        {
            ModelT modelT = (default(ModelT) == null) ? Activator.CreateInstance<ModelT>() : default(ModelT);
            if (modelT.ModelName == string.Empty)
            {
                throw new Exception("the name of model is null");
            }
            this.AddModel(modelT);
            return modelT;
        }

        public bool RemoveModelByName(string modelName)
        {
            return this._models.Remove(modelName);
        }

        public IModel GetModelByName(string modelName)
        {
            IModel value = this._models.GetValue(modelName);
            if (value == null)
            {
                throw new Exception("Model " + modelName + " is not exists");
            }
            return value;
        }

        public ModelT GetSpecialTypeModelByName<ModelT>(string modelName) where ModelT : IModel
        {
            ModelT result = default(ModelT);
            IModel modelByName = this.GetModelByName(modelName);
            if (modelByName != null)
            {
                result = (ModelT)((object)modelByName);
            }
            return result;
        }

        public ModelT GetModel<ModelT>() where ModelT : IModel
        {
            ModelT result = default(ModelT);
            int count = this._models.Count;
            for (int i = 0; i < count; i++)
            {
                IModel valueAt = this._models.GetValueAt(i);
                if (valueAt != null)
                {
                    result = (ModelT)((object)valueAt);
                    break;
                }
            }
            return result;
        }

        public override void OnUpdate()
        {
        }

        private static ModelManager _instance;

        private DictionaryList<string, IModel> _models = new DictionaryList<string, IModel>();
    }
}
