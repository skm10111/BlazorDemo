redirectToCheckout = function (sessionId){
    var stripe = Stripe("pk_test_51LsjygLK7ResHhYBMz9fwv3SFnNPGeMgXXeZlxqYb09Reo6FM4XlhbX1BygTXRiT52CNovO69KucxeMyteAki83h00ON5cuAHH")
    stripe.redirectToCheckout({ sessionId: sessionId });
}