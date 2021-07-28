namespace MesJolisCotillons.Contracts
{
    public enum MessageCode
    {
        // validation
        UserAlreadyExists,
        PasswordIsNotLongEnough,
        PasswordAreNotSame,
        ProductDoesNotExist,
        CategoriesDoNotExist,
        PageParametersNotGreaterThanZero,

        // operation
        CreateProductSuccess,
        DeleteProductSuccess,
        DefaultReponseSuccess,
    }
}
