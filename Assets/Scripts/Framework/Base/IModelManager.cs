using Framework.Base.MVC;

namespace Framework.Base
{
    public interface IModelManager : IManager
    {
        void AddModel(IModel model);

        ModelT AddModelByType<ModelT>() where ModelT : IModel, new();

        bool RemoveModelByName(string modelName);

        IModel GetModelByName(string modelName);

        ModelT GetSpecialTypeModelByName<ModelT>(string modelName) where ModelT : IModel;

        ModelT GetModel<ModelT>() where ModelT : IModel;
    }
}
