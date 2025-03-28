using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF.CRUD
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUsuario" in both code and config file together.
    [ServiceContract]
    public interface IUsuario
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [ServiceKnownType(typeof(SL_WCF.CRUD.Result))]  //Reconocer nuevos modelos, para serializar el modelo a un XML
        //Tipo de retorno - Nombre método - parámetros
        SL_WCF.CRUD.Result Add(ML.Usuario usuario); //Firma de método

        [OperationContract]
        [ServiceKnownType(typeof(SL_WCF.CRUD.Result))]
        SL_WCF.CRUD.Result Delete(int idUsuario); //Firma de método

        [OperationContract]
        [ServiceKnownType(typeof(SL_WCF.CRUD.Result))]
        SL_WCF.CRUD.Result Update(ML.Usuario usuario); //Firma de método

        [OperationContract]
        [ServiceKnownType(typeof(ML.Usuario))]
        SL_WCF.CRUD.Result GetAll(ML.Usuario usuarioObj); //Firma de método


    }
}
